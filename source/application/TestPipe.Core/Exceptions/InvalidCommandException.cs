namespace TestPipe.Core.Exceptions
{
    using System;

    [Serializable]
    public class InvalidCommandException : TestPipeException
    {
        public InvalidCommandException(string msg)
            : base(msg)
        {
        }

        public InvalidCommandException(string format, params object[] prms)
            : base(string.Format(format, prms))
        {
        }
    }
}