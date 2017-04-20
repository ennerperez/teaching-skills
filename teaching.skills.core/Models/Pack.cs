using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Teaching.Skills.Models;

namespace Teaching.Skills.Core
{
    public class Pack : Base
    {
        public Pack()
        {
            Content = new HashSet<Category>();
        }

        [JsonProperty("Version")]
        public Version Version { get; set; }

        [JsonProperty("Date")]
        public DateTime Date { get; set; }

        [JsonProperty("Author")]
        public string Author { get; set; }

        [JsonProperty("Content")]
        public HashSet<Category> Content { get; set; }
    }
}