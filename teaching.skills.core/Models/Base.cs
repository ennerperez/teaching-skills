using Newtonsoft.Json;

namespace Teaching.Skills.Models
{
	public abstract class Base : Base<short>
	{
	}

	public abstract class Base<T>
	{
		[JsonProperty("Id")]
		public T Id { get; set; }

		[JsonProperty("Name")]
		public string Name { get; set; }

		#region IParcelable

		public int DescribeContents()
		{
			return 0;
		}

		#endregion IParcelable

		public override bool Equals(object obj)
		{
			if (this == obj)
				return true;

			if (obj == null || GetType() != obj.GetType())
				return false;

			var @ref = (Base)obj;

			return (!Id.Equals(@ref.Id));
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