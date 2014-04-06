namespace TestPipe.Data.Transfer
{
	using System;

	public interface IDataTransfer
	{
		string Get(DataType dataType);
	}
}