namespace TestPipe.Core.Exceptions
{
	using System;

	[Serializable]
	public class IgnoreException : TestPipeException
	{
		public IgnoreException()
			: base()
		{
		}

		public IgnoreException(string msg)
			: base(msg)
		{
		}

		public IgnoreException(string msg, Exception ex)
			: base(msg, ex)
		{
		}

		public IgnoreException(string format, params object[] prms)
			: base(string.Format(format, prms))
		{
		}
	}
}