namespace TestPipe.Demo.KeywordDriven
{
	using System;
	using NUnit.Framework;
	using OpenQA.Selenium;
	using OpenQA.Selenium.IE;
	using TechTalk.SpecFlow;
	using TestPipe.Demo.KeywordDriven.Core;

	[Binding]
	public class KeywordDrivenSteps
	{
		private IWebDriver driver;
		private BaseKeywordDrivenPage testPage;

		[Given(@"I am on the ""(.*)"" page")]
		public void GivenIAmOnThePage(string p0)
		{
			this.SetPage(p0);
			this.testPage.Open();
		}

		[Given(@"I enter ""(.*)"" in ""(.*)""")]
		public void GivenIEnterTextIn(string p0, string p1)
		{
			string text = p0;
			this.testPage.EnterText(p1, text);
		}

		[BeforeScenario]
		public void SetupScenario()
		{
			this.driver = new InternetExplorerDriver();
		}

		[Then(@"I should be on the ""(.*)"" page")]
		public void ThenIShouldBeOnThePage(string p0)
		{
			this.SetPage(p0);
			Assert.IsTrue(this.testPage.IsOpen());
		}

		[Then(@"""(.*)"" text should be ""(.*)""")]
		public void ThenTheElementShouldHaveText(string p0, string p1)
		{
			Assert.IsTrue(this.testPage.HasText(p0, p1));
		}

		[When(@"I click ""(.*)""")]
		public void WhenIClick(string p0)
		{
			this.testPage.Click(p0);
		}

		//Instantiate page object from string;
		//Bad because we have to keep adding pages to the if or switch statement when we have a new page
		//We could use some reflection magic and enforce naming conventions
		private void SetPage(string page)
		{
			switch (page)
			{
				case "Welcome":
					{
						this.testPage = new WelcomePage(driver);
						return;
					}
				case "Success":
					{
						this.testPage = new SuccessPage(driver);
						return;
					}
			}

			throw new ApplicationException("Invalid page.");
		}
	}
}