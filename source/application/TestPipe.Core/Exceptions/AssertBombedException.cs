namespace TestPipe.Core.Exceptions
{
    using System;

    [Serializable]
    public class AssertBombedException : TestPipeException
    {
        public AssertBombedException(string msg)
            : base(msg)
        {
        }

        public AssertBombedException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

        public AssertBombedException(string format, params object[] prms)
            : base(string.Format(format, prms))
        {
        }
    }
}