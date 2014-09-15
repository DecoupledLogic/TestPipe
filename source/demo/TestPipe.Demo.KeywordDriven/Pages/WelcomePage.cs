namespace TestPipe.Demo.KeywordDriven.Pages
{
	using System;
	using OpenQA.Selenium;
	using TestPipe.Demo.KeywordDriven.Core;

	//This basically adds properties that gives the tests access to elements on this specific page
	public class WelcomePage : BaseKeywordDrivenPage
	{
		private const string url = "welcome.htm";

		public WelcomePage(IWebDriver driver)
			: base(driver, url)
		{
		}

		public IWebElement Submit
		{
			get
			{
				return this.Driver.FindElement(By.Id("submit"));
			}
		}

		public IWebElement Welcome
		{
			get
			{
				return this.Driver.FindElement(By.Id("welcome"));
			}
		}

		public override IWebElement GetElement(string element)
		{
			//Bad because we have to keep adding elements to the if or switch statement when we have a new element
			//We could use some reflection magic and enforce naming conventions
			switch (element)
			{
				case "Welcome":
					{
						return this.Welcome;
					}
				case "Submit":
					{
						return this.Submit;
					}
			}

			return base.GetElement(element);
		}
	}
}