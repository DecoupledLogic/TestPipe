namespace TestPipe.Core.Exceptions
{
    using System;

    [Serializable]
    public class ConcurrencyException : TestPipeException
    {
        public ConcurrencyException(string message) 
            : base(message) 
        { 
        }
    }
}
