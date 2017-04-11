using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using Teaching.Skills.Core;

namespace Teaching.Skills.Models
{
	public class Question : Base<Guid>
	{

		public Question()
		{
		}

		[JsonProperty("Description")]
		public string Description { get; set; }

		[JsonIgnore]
		public Indicator Indicator { get; set; }

	}
}

