namespace TestPipe.Email.Authentication
{
    using System;
    using System.Net;
    using Microsoft.Exchange.WebServices.Data;

    public static class Service
    {
        static Service()
        {
            CertificateCallback.Initialize();
        }

        public static ExchangeService ConnectToService(IUserData userData)
        {
            return ConnectToService(userData, null);
        }

        public static ExchangeService ConnectToService(IUserData userData, ITraceListener listener)
        {
            ExchangeService service = new ExchangeService(userData.Version);

            if (listener != null)
            {
                service.TraceListener = listener;
                service.TraceFlags = TraceFlags.All;
                service.TraceEnabled = true;
            }

            service.Credentials = new NetworkCredential(userData.EmailAddress, userData.Password);

            if (userData.AutodiscoverUrl == null)
            {
                Console.Write(string.Format("Using Autodiscover to find EWS URL for {0}. Please wait... ", userData.EmailAddress));

                service.AutodiscoverUrl(userData.EmailAddress, RedirectionUrlValidationCallback);
                userData.AutodiscoverUrl = service.Url;

                Console.WriteLine("Complete");
            }
            else
            {
                service.Url = userData.AutodiscoverUrl;
            }

            return service;
        }

        public static ExchangeService ConnectToServiceWithImpersonation(
                  IUserData userData,
                  string impersonatedUserSMTPAddress)
        {
            return ConnectToServiceWithImpersonation(userData, impersonatedUserSMTPAddress, null);
        }

        public static ExchangeService ConnectToServiceWithImpersonation(
                  IUserData userData,
                  string impersonatedUserSMTPAddress,
                  ITraceListener listener)
        {
            ExchangeService service = new ExchangeService(userData.Version);

            if (listener != null)
            {
                service.TraceListener = listener;
                service.TraceFlags = TraceFlags.All;
                service.TraceEnabled = true;
            }

            service.Credentials = new NetworkCredential(userData.EmailAddress, userData.Password);

            ImpersonatedUserId impersonatedUserId =
              new ImpersonatedUserId(ConnectingIdType.SmtpAddress, impersonatedUserSMTPAddress);

            service.ImpersonatedUserId = impersonatedUserId;

            if (userData.AutodiscoverUrl == null)
            {
                service.AutodiscoverUrl(userData.EmailAddress, RedirectionUrlValidationCallback);
                userData.AutodiscoverUrl = service.Url;
            }
            else
            {
                service.Url = userData.AutodiscoverUrl;
            }

            return service;
        }

        // Testing Only - should be more robust for production
        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }

            return result;
        }
    }
}