using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Teaching.Skills.Models
{
    public class Indicator
    {

        public Indicator()
        {
            Questions = new HashSet<Question>();
        }

        [JsonProperty("Id")]
        public short Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Questions")]
        public HashSet<Question> Questions { get; set; }

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

            var @ref = (Indicator)obj;

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

