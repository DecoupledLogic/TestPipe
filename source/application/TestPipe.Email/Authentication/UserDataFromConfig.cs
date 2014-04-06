namespace TestPipe.Email.Authentication
{
    using System;
    using System.Configuration;
    using System.Security;
    using Microsoft.Exchange.WebServices.Data;

    public class UserDataFromConfig : IUserData
    {
        private static UserDataFromConfig userData;

        public Uri AutodiscoverUrl
        {
            get;
            set;
        }

        public string EmailAddress
        {
            get;
            private set;
        }

        public SecureString Password
        {
            get;
            private set;
        }

        public ExchangeVersion Version
        {
            get
            {
                return ExchangeVersion.Exchange2010_SP2;
            }
        }

        public static IUserData GetUserData()
        {
            if (userData == null)
            {
                GetUserDataFromConfig();
            }

            return userData;
        }

        private static void GetUserDataFromConfig()
        {
            userData = new UserDataFromConfig();

            userData.EmailAddress = ConfigurationManager.AppSettings["email"];

            SymetricEncryption s = new SymetricEncryption();
            string password = ConfigurationManager.AppSettings["password"];
            string unsecurePassword = s.DecryptFromBase64(password, EncryptionAlgorithm.TripleDes);

            //Using NetworkCredentials to convert the unsecurePassword String to SecureString
            userData.Password = new System.Net.NetworkCredential(string.Empty, unsecurePassword).SecurePassword;
        }
    }
}