﻿using System.Text.Json.Serialization;

namespace Helium.Model
{
    public class FeaturedMovie
    {
        public string MovieId { get; set; }
        public int Weight { get; set; } = 1;
    }
}
