namespace TestPipe.Core.Exceptions
{
	using System;

	[Serializable]
	public class ManualOnlyException : TestPipeException
	{
		public ManualOnlyException()
			: base()
		{
		}

		public ManualOnlyException(string msg)
			: base(msg)
		{
		}

		public ManualOnlyException(string msg, Exception ex)
			: base(msg, ex)
		{
		}

		public ManualOnlyException(string format, params object[] prms)
			: base(string.Format(format, prms))
		{
		}
	}
}