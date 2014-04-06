namespace TestPipe.Email.Authentication
{
    using System;
    using System.IO;
    using Microsoft.Exchange.WebServices.Data;

    public class TraceListener : ITraceListener
    {
        public void Trace(string traceType, string traceMessage)
        {
            this.CreateXMLTextFile(traceType, traceMessage);
        }

        private void CreateXMLTextFile(string fileName, string traceContent)
        {
            try
            {
                if (!Directory.Exists(@"..\\TraceOutput"))
                {
                    Directory.CreateDirectory(@"..\\TraceOutput");
                }

                System.IO.File.WriteAllText(@"..\\TraceOutput\\" + fileName + DateTime.Now.Ticks + ".txt", traceContent);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}