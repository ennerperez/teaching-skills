using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Teaching.Skills.Models
{
    public class Question 
    {

        public Question()
        {
        }

        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        public Indicator Indicator { get; set; }

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

            var @ref = (Question)obj;

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

