namespace TestPipe.Demo.Steps
{
	using System;
	using NUnit.Framework;
	using TechTalk.SpecFlow;
	using TestPipe.Core;
	using TestPipe.Core.Page;
	using TestPipe.Demo.Pages;

	[Binding]
	public class SearchSteps : BaseDemoStep
	{
		private static SearchPage testPage;
		private static BasePage resultPage;
		private static string searchText;

		[BeforeScenario("@Search")]
		public static void SetupLoginScenario()
		{
			BaseDemoStep.SetupScenario();
			testPage = new SearchPage(TestSession.Browser, TestSession.Environment);
		}

		[Given(@"I am on the search page")]
		public void GivenIAmOnTheSearchPage()
		{
			testPage.Open();
		}

		[Given(@"when I enter ""(.*)""")]
		public void GivenWhenIEnter(string p0)
		{
			searchText = p0;

			testPage.EnterText(testPage.Search, searchText);
			
			testPage = new SearchPage(TestSession.Browser, TestSession.Environment);
		}

		[When(@"I submit the search")]
		public void WhenISubmitTheSearch()
		{
			resultPage = testPage.Submit(searchText);
		}

		[Then(@"results should be displayed")]
		public void ThenResultsShouldBeDisplayed()
		{
			Assert.IsTrue(resultPage.IsOpen());
		}
	}
}