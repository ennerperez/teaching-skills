using Newtonsoft.Json;
using System;
using Teaching.Skills.Core;

namespace Teaching.Skills.Models
{
	public class Answer : Base<Guid>
	{

		public Answer()
		{
		}

		[JsonProperty("Date")]
		public DateTime Date { get; set; }

		public User User { get; set; }

		[JsonProperty("Value")]
		public int Value { get; set; }

		[JsonProperty("Question")]
		internal Guid QuestionId { get; set; }

		private Question question;
		[JsonIgnore]
		public Question Question
		{
			get { return question; }
			set
			{
				question = value;
				if (question != null)
					QuestionId = question.Id;
				else
					question = null;
			}
		}

		public override int GetHashCode()
		{
			return QuestionId.GetHashCode();
		}
	}
}

