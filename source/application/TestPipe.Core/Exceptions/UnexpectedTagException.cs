namespace TestPipe.Core.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class UnexpectedTagException : Exception
	{
		public UnexpectedTagException()
			: base() 
		{ 
		}

		public UnexpectedTagException(string message)
			: base(message) 
		{ 
		}

		public UnexpectedTagException(string format, params object[] args)
			: base(string.Format(format, args)) 
		{
		}

		public UnexpectedTagException(string message, Exception innerException)
			: base(message, innerException) 
		{
		}

		public UnexpectedTagException(string format, Exception innerException, params object[] args)
			: base(string.Format(format, args), innerException) 
		{ 
		}

		protected UnexpectedTagException(SerializationInfo info, StreamingContext context)
			: base(info, context) 
		{ 
		}
	}
}