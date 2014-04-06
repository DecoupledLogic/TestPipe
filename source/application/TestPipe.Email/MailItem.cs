namespace TestPipe.Email
{
    public class MailItem
    {
        public MailItem(string from, string[] recipients, string subject, string body)
        {
            this.From = from;
            this.Recipients = recipients;
            this.Subject = subject;
            this.Body = body;
        }

        public string Body { get; private set; }

        public string From { get; private set; }

        public string[] Recipients { get; private set; }

        public string Subject { get; private set; }
    }
}