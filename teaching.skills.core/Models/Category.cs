using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using Teaching.Skills.Core;

namespace Teaching.Skills.Models
{
    public class Category : Base<int>
    {
        public Category()
        {
            Indicators = new HashSet<Indicator>();
        }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Indicators")]
        public HashSet<Indicator> Indicators { get; set; }
    }
}