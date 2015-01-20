namespace TestPipe.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class TestPipeTimeOutException : TestPipeException
    {
        public TestPipeTimeOutException()
            : base()
        {

        }
        public TestPipeTimeOutException(string message)
            : base(message)
        {

        }
        public TestPipeTimeOutException(string format, params object[] args)
            : base(string.Format(format, args))
        {

        }
        public TestPipeTimeOutException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
        public TestPipeTimeOutException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {

        }
        public TestPipeTimeOutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
