﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
    public class MeterReading
    {
        public int Id { get; set; }
        public string Reading { get; set; }
        public DateTime? ReadingDate { get; set; } = DateTime.Now;
        public int? MeterId { get; set; }  
        public Meter? Meter { get; set; }
    }

    public class TokenResponse
    {
        [JsonPropertyName("$id")]
        public string Id { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }

}
