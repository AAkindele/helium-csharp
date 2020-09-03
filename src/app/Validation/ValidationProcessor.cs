﻿using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace CSE.Helium.Validation
{
    internal static class ValidationProcessor
    {
        /// <summary>
        /// creates JSON response using StringBuilder given inputs
        /// </summary>
        /// <param name="context"></param>
        /// <param name="target"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static string WriteJsonWithStringBuilder(ActionContext context, ILogger logger)
        {
            var modelStateEntries = context.ModelState.Where(
                e => e.Value.Errors.Count > 0).ToArray();
            var last = modelStateEntries.Last();

            var sb = new StringBuilder();

            sb.Append($"{{\"type\": \"http://www.example.com/validation-error\",\n");
            sb.Append($"\"title\": \"Your request parameters did not validate.\",\n");
            sb.Append($"\"detail\": \"One or more invalid parameters were specified.\",\n");
            sb.Append($"\"status\": 400,");
            sb.Append($"\"instance\": \"{context.HttpContext.Request.Path}\",\n");
            sb.Append($"\"validationErrors\": [\n");
            
            // write and log details collection
            foreach (var state in modelStateEntries)
            {
                logger.LogWarning($"InvalidParameter|{context.HttpContext.Request.Path}|{state.Value.Errors[0].ErrorMessage}");

                sb.Append("{\n");
                sb.Append($"\"code\": \"InvalidValue\",\n");
                sb.Append($"\"target\": \"{state.Key}\",\n");
                sb.Append($"\"message\": \"{state.Value.Errors[0].ErrorMessage}\"\n");

                if (state.Equals(last))
                {
                    sb.Append("}\n");
                    sb.Append("]");
                }
                else
                {
                    // more results
                    sb.Append("},\n");
                }
            }

            sb.Append("}\n");

            return sb.ToString();
        }
        
        /// <summary>
        /// creates JSON response using ValidationProblemDetails given inputs
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static string WriteJsonUsingObjects(ActionContext context, ILogger logger)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Type = "http://www.example.com/validation-error",
                Title = "Your request parameters did not validate.",
                Detail = "One or more invalid parameters were specified.",
                Status = 400,
                Instance = context.HttpContext.Request.GetEncodedPathAndQuery(),
            };

            // collect all errors for iterative string/json representation
            var validationErrors = context.ModelState.Where(m => m.Value.Errors.Count > 0).ToArray();

            foreach (var validationError in validationErrors)
            {
                // skip empty validation error
                if(string.IsNullOrEmpty(validationError.Key))
                    continue;

                // log each validation error in the collection
                logger.LogWarning($"InvalidParameter|{context.HttpContext.Request.Path}|{validationError.Value.Errors[0].ErrorMessage}");

                var error = new ValidationError("InvalidValue", validationError.Key, validationError.Value.Errors[0].ErrorMessage);
                
                // add error object to problemDetails
                problemDetails.ValidationErrors.Add(error);
            }

            return JsonSerializer.Serialize(problemDetails);
        }
    }
}
