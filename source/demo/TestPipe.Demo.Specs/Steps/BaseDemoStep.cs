namespace TestPipe.Demo.Steps
{
	using System;
	using NUnit.Framework;
	using TechTalk.SpecFlow;
	using TestPipe.SpecFlow;

	[Binding]
	public class BaseDemoStep
	{
		//Many scenarios need the test user to login, a common login method is an easy reusable solution
		//public static BasePage Login(string username, string password, IBrowser browser, TestEnvironment environment)
		//{
		//	LoginPage login = new LoginPage(browser, environment);
		//	login.Open();
		//	login.EnterUsername(username);
		//	login.EnterPassword(password);
		//	return login.Login();
		//}

		[BeforeFeature]
		public static void SetupFeature()
		{
			//Get feature test data from XML
			//Delete test data from database
			//Save test data to database
			Runner.SetupFeature();
		}

		[AfterFeature]
		public static void TeardownFeature()
		{
			Runner.TeardownFeature();
		}

		[BeforeTestRun]
		public static void SetupSuite()
		{
			Runner.SetupSuite();
		}

		[AfterTestRun]
		public static void TeardownSuite()
		{
			Runner.TeardownSuite();
		}

		public static void SetupScenario()
		{
			try
			{
				Runner.SetupScenario();
			}
			catch (TestPipe.Core.Exceptions.IgnoreException ex)
			{
				Assert.Ignore(ex.Message);
			}
		}
	}
}