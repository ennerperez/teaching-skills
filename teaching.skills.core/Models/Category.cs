using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Teaching.Skills.Models
{
    public class Category
    {

        public Category()
        {
            Indicators = new HashSet<Indicator>();
        }

        [JsonProperty("Id")]
        public short Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Indicators")]
        public HashSet<Indicator> Indicators { get; set; }

        #region IParcelable

        public int DescribeContents()
        {
            return 0;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || GetType() != obj.GetType())
                return false;

            var @ref = (Category)obj;

            return (Id != @ref.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

