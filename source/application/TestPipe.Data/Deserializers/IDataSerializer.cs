namespace TestPipe.Data.Deserializers
{
	using System;

	public interface IDataDeserializer
	{
		string DeerializeReader(string serialized, string dataName);
	}
}