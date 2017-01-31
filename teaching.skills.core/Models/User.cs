using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Teaching.Skills.Models
{
    public class User 
	{

		public User()
		{
			Answers = new HashSet<Answer>();
		}

		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("Answers")]
		public HashSet<Answer> Answers { get; set; }

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

			var @ref = (User)obj;

			return (Name != @ref.Name);
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

	}
}

