namespace TestPipe.Core.Exceptions
{
    using System;

    [Serializable]
    public class UnregisteredCommandException : TestPipeException
    {
        public UnregisteredCommandException(string message) 
            : base(message) 
        { 
        }
    }
}