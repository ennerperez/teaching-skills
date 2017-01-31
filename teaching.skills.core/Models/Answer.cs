using Newtonsoft.Json;
using System;

namespace Teaching.Skills.Models
{
    public class Answer
    {

        public Answer()
        {
        }

        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("Date")]
        public DateTime Date { get; set; }

        public User User { get; set; }

        [JsonProperty("Value")]
        public int Value { get; set; }

        [JsonProperty("Question")]
        internal Guid QuestionId { get; set; }

        private Question question;
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

            var @ref = (Answer)obj;

            return (Id != @ref.Id);
        }

        public override int GetHashCode()
        {
            return QuestionId.GetHashCode();
        }
    }
}

