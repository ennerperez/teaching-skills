using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Teaching.Skills.Models;

namespace Teaching.Skills.Models
{
	public class Package : Base<string>
	{
		public Package()
		{
			Content = new HashSet<Category>();
		}

		[JsonProperty("Language")]
		public string Language { get; set; }

		[JsonProperty("Version")]
		public Version Version { get; set; }

		[JsonProperty("Date")]
		public DateTime Date { get; set; }

		[JsonProperty("Author")]
		public string Author { get; set; }

		[JsonProperty("Content")]
		public HashSet<Category> Content { get; set; }

		[JsonProperty("Description")]
		public string Description { get; set; }

		[JsonProperty("Url")]
		public string Url { get; set; }

		public string Key()
		{
			return string.Join(".", this.Id, this.Language);
		}

	}
}