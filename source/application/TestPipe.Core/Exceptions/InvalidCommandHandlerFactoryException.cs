namespace TestPipe.Core.Exceptions
{
    using System;

    [Serializable]
    public class InvalidCommandHandlerFactoryException : TestPipeException
    {
        public InvalidCommandHandlerFactoryException(string message) 
            : base(message) 
        { 
        }
    }
}