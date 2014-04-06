namespace TestPipe.Core.Exceptions
{
	using System;

	[Serializable]
	public class TestPipeException : Exception
	{
		public TestPipeException()
			: base()
		{
		}

		public TestPipeException(string msg)
			: base(msg)
		{
		}

		public TestPipeException(string msg, Exception ex)
			: base(msg, ex)
		{
		}
	}
}