namespace TestPipe.Common
{
    using System;

    public class LoggedObject
    {
        public LoggedObject(ILogManager log)
        {
            if (log == null)
            {
                this.Log = new Logger();
            }
            else
            {
                this.Log = log;
            }
        }

        public ILogManager Log { get; private set; }
    }
}