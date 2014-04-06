namespace TestPipe.Data.Serializers
{
	using System;
	using System.Data.Common;

	public class BaseDataReaderSerializer
	{
		protected static readonly string InvalidNamExMsg = "Parameter dataName cannot be null or white space.";
		protected static readonly string NullReaderExMsg = "reader";
		protected static readonly string RowName = "RowColumnValues";

		protected void ValidateSerializeReader(DbDataReader reader, string dataName)
		{
			if (reader == null)
			{
				throw new ArgumentNullException(NullReaderExMsg);
			}

			if (string.IsNullOrWhiteSpace(dataName))
			{
				throw new ArgumentException(InvalidNamExMsg);
			}
		}
	}
}