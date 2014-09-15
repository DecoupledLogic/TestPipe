namespace TestPipe.Demo.KeywordDriven.Pages
{
	using System;
	using OpenQA.Selenium;
	using TestPipe.Demo.KeywordDriven.Core;

	public class SuccessPage : BaseKeywordDrivenPage
	{
		private const string url = "success.htm";

		public SuccessPage(IWebDriver driver)
			: base(driver, url)
		{
		}

		public IWebElement Header
		{
			get
			{
				return this.Driver.FindElement(By.Id("header"));
			}
		}

		public override IWebElement GetElement(string element)
		{
			//Bad because we have to keep adding elements to the if or switch statement when we have a new element
			//We could use some reflection magic and enforce naming conventions
			switch (element)
			{
				case "Header":
					{
						return this.Header;
					}
			}

			return base.GetElement(element);
		}
	}
}