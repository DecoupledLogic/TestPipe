namespace TestPipe.Email.Authentication
{
    using System;
    using System.Security;
    using Microsoft.Exchange.WebServices.Data;

    public interface IUserData
    {
        Uri AutodiscoverUrl { get; set; }

        string EmailAddress { get; }

        SecureString Password { get; }

        ExchangeVersion Version { get; }
    }
}