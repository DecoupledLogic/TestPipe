namespace TestPipe.Selenium.Pages
{
	using TestPipe.Core;
	using TestPipe.Core.Interfaces;
	using TestPipe.Core.Page;

	public class SeleniumBasePage : BasePage
	{
		public SeleniumBasePage(IBrowser browser, TestEnvironment testEnvironment)
			: base(browser, testEnvironment)
		{
		}
	}
}