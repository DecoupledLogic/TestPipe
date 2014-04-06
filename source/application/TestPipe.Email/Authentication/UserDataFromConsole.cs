namespace TestPipe.Email.Authentication
{
    using System;
    using System.Security;
    using Microsoft.Exchange.WebServices.Data;

    public class UserDataFromConsole : IUserData
    {
        private static UserDataFromConsole userData;

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
                GetUserDataFromConsole();
            }

            return userData;
        }

        private static void GetUserDataFromConsole()
        {
            userData = new UserDataFromConsole();

            Console.Write("Enter email address: ");
            userData.EmailAddress = Console.ReadLine();

            userData.Password = new SecureString();

            Console.Write("Enter password: ");

            while (true)
            {
                ConsoleKeyInfo userInput = Console.ReadKey(true);
                if (userInput.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (userInput.Key == ConsoleKey.Escape)
                {
                    return;
                }
                else if (userInput.Key == ConsoleKey.Backspace)
                {
                    if (userData.Password.Length != 0)
                    {
                        userData.Password.RemoveAt(userData.Password.Length - 1);
                    }
                }
                else
                {
                    userData.Password.AppendChar(userInput.KeyChar);
                    Console.Write("*");
                }
            }

            Console.WriteLine();

            userData.Password.MakeReadOnly();
        }
    }
}