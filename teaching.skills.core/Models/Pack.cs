using System;
using Newtonsoft.Json;
using System.Collections;
using Teaching.Skills.Models;
using System.Collections.Generic;

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
