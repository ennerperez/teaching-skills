using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using Teaching.Skills.Core;

namespace Teaching.Skills.Models
{
	public class Indicator : Base
	{

		public Indicator()
		{
			Questions = new HashSet<Question>();
		}

		[JsonProperty("Description")]
		public string Description { get; set; }

		[JsonProperty("Questions")]
		public HashSet<Question> Questions { get; set; }

	}
}

