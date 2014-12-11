namespace TestPipe.Core.Exceptions
{
	using System;
	using System.Runtime.Serialization;

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

		public TestPipeException(string format, params object[] args)
			: base(string.Format(format, args))
		{
		}

		public TestPipeException(string format, Exception innerException, params object[] args)
			: base(string.Format(format, args), innerException)
		{
		}

		protected TestPipeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}