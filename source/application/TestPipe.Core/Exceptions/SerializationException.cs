namespace TestPipe.Core.Exceptions
{
    using System;

    [Serializable]
    public class SerializationException : TestPipeException
    {
        public SerializationException(string message) 
            : base(message) 
        { 
        }

        public SerializationException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
