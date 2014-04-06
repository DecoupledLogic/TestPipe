namespace TestPipe.Email
{
    using Microsoft.Exchange.WebServices.Data;

    public class MailFilter
    {
        public MailFilter(string subject, string body, LogicalOperator op = LogicalOperator.Or, WellKnownFolderName parentFolder = WellKnownFolderName.Inbox, int limit = 50)
        {
            this.Subject = subject;
            this.Body = body;
            this.Operator = op;
            this.ParentFolder = parentFolder;
            this.ResultsLimit = limit;
        }

        public string Body { get; private set; }

        public LogicalOperator Operator { get; private set; }

        public WellKnownFolderName ParentFolder { get; private set; }

        public int ResultsLimit { get; private set; }

        public string Subject { get; private set; }
    }
}