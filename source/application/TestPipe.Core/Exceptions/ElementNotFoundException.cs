namespace TestPipe.Core.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class ElementNotFoundException : Exception
	{
		public ElementNotFoundException()
			: base()
		{
		}

		public ElementNotFoundException(string message)
			: base(message)
		{
		}

		public ElementNotFoundException(string format, params object[] args)
			: base(string.Format(format, args))
		{
		}

		public ElementNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ElementNotFoundException(string format, Exception innerException, params object[] args)
			: base(string.Format(format, args), innerException)
		{
		}

		protected ElementNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}