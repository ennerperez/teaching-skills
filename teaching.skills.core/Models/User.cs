using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Teaching.Skills.Models
{
	public class User : Base<string>
	{
		public User()
		{
			Answers = new HashSet<Answer>();
		}

		[JsonProperty("Answers")]
		public HashSet<Answer> Answers { get; set; }
	}
}