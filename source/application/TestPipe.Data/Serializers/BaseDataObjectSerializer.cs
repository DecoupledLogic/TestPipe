namespace TestPipe.Data.Serializers
{
	using System;

	public class BaseDataObjectSerializer
	{
		protected static readonly string InvalidNamExMsg = "Parameter dataName cannot be null or white space.";
		protected static readonly string NullObjectExMsg = "obj";
		protected static readonly string RowName = "RowColumnValues";

		protected void ValidateSerializeObject(object obj, string dataName)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(NullObjectExMsg);
			}

			if (string.IsNullOrWhiteSpace(dataName))
			{
				throw new ArgumentException(InvalidNamExMsg);
			}
		}
	}
}