using Azure.Cosmos;
using Azure.Cosmos.Serialization;
using System;
using System.IO;
using System.Text.Json;


namespace CSE.Helium.DataAccessLayer
{
    /// <summary>
    /// Internal class for Cosmos config
    /// </summary>
    internal class CosmosConfig
    {
        public CosmosClient Client = null;
        public CosmosContainer Container = null;

        // default values for Cosmos Options
        public int MaxRows = 1000;
        public int Timeout = 60;
        public int Retries = 10;

        // Cosmos connection fields
        public string CosmosUrl;
        public string CosmosKey;
        public string CosmosDatabase;
        public string CosmosCollection;

        // member variables
        private QueryRequestOptions queryRequestOptions = null;
        private CosmosClientOptions cosmosClientOptions = null;

        // CosmosDB query request options
        public QueryRequestOptions QueryRequestOptions
        {
            get
            {
                if (queryRequestOptions == null)
                {
                    queryRequestOptions = new QueryRequestOptions { MaxItemCount = MaxRows };
                }

                return queryRequestOptions;
            }
        }

        // default protocol is tcp, default connection mode is direct
        public CosmosClientOptions CosmosClientOptions
        {
            get
            {
                if (cosmosClientOptions == null)
                {
                    cosmosClientOptions = new CosmosClientOptions
                    {
                        RequestTimeout = TimeSpan.FromSeconds(Timeout),
                        MaxRetryAttemptsOnRateLimitedRequests = Retries,
                        MaxRetryWaitTimeOnRateLimitedRequests = TimeSpan.FromSeconds(Timeout)
                    };

                    //CosmosClientOptions.SerializerOptions = new CosmosSerializationOptions
                    //{
                    //    IgnoreNullValues = true,
                    //    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    //};

                    // TODO - use SerializerOptions once the Cosmos bug is fixed
                    CosmosClientOptions.Serializer = new HeliumSerializer();

                }

                return cosmosClientOptions;
            }
        }
    }

    public class HeliumSerializer : CosmosSerializer
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
        };
        
        // TODO - remove this when the Cosmos bug is fixed
        public override T FromStream<T>(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var len = stream.Length;
            byte[] buff = new byte[len];
            stream.Read(buff, 0, (int)len);


            T value = JsonSerializer.Deserialize<T>(System.Text.UTF8Encoding.UTF8.GetString(buff), options);

            stream.Close();

            return value;
        }

        public override Stream ToStream<T>(T input)
        {
            byte[] buff = JsonSerializer.SerializeToUtf8Bytes<T>(input);

            var stream = new MemoryStream(buff);

            return stream;
        }
    }
}