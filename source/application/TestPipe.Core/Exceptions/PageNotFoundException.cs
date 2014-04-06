namespace TestPipe.Core.Exceptions
{
    using System;

    [Serializable]
    public class PageNotFoundException : TestPipeException
    {
        public PageNotFoundException(string msg)
            : base(msg)
        {
        }

        public PageNotFoundException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

        public PageNotFoundException(string format, params object[] prms)
            : base(string.Format(format, prms))
        {
        }
    }
}