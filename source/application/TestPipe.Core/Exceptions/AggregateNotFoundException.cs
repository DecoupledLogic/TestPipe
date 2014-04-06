namespace TestPipe.Core.Exceptions
{
    using System;

    [Serializable]
    public class AggregateNotFoundException : TestPipeException
    {
        public AggregateNotFoundException(string msg)
            : base(msg)
        {
        }
    }
}