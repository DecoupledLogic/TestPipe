namespace TestPipe.Demo.Steps
{
	using System;
	using NUnit.Framework;
	using TechTalk.SpecFlow;
	using TestPipe.SpecFlow;

	[Binding]
	public sealed class CommonDemoStep
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

		//These bindings are global to all features and scenarios, meaning this runs before and after every feature and before every scenario.
		[BeforeScenario]
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