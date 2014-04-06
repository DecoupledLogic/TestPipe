namespace TestPipe.Data.Serializers
{
	using System;

	public interface IDataSerializer
	{
		string Serialize(object obj, string dataName);
	}
}