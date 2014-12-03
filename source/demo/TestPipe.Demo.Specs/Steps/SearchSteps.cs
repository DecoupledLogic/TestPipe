namespace TestPipe.Demo.Steps
{
	using System;
	using TechTalk.SpecFlow;
	using TestPipe.Assertions;
	using TestPipe.Core;
	using TestPipe.Core.Page;
	using TestPipe.Core.Session;
	using TestPipe.Demo.Pages;

	[Binding]
	public class SearchSteps
	{
		private static SessionFeature feature;
		private static BasePage resultPage;
		private static SessionScenario scenario;
		private static string searchText;
		private static SearchPage testPage;

		[BeforeScenario("@Search")]
		public static void SetupScenario()
		{
			scenario = StepHelper.SetupScenario();
			testPage = new SearchPage(TestSession.Browser, TestSession.Environment);
		}

		[BeforeFeature]
		public static void SetupSearchFeature()
		{
			feature = StepHelper.SetupFeature();
		}

		[BeforeScenario("@Search")]
		public static void SetupScenario(string[] tags, string featureTitle, string scenarioTitle)
		{
			scenario = StepHelper.SetupScenario(tags, featureTitle, scenarioTitle);
			testPage = new SearchPage(TestSession.Browser, TestSession.Environment);
		}

		[BeforeFeature]
		public static void SetupSearchFeature(string[] tags, string title)
		{
			feature = StepHelper.SetupFeature(tags, title);
		}

		[Given(@"I am on the search page")]
		public void GivenIAmOnTheSearchPage()
		{
			testPage.Open();
		}

		[Given(@"when I do a search")]
		public void GivenWhenIDoASearch()
		{
			searchText = scenario.Data.Q;

			testPage.EnterText(testPage.Search, searchText);

			testPage = new SearchPage(TestSession.Browser, TestSession.Environment);
		}

		[Then(@"results should be displayed")]
		public void ThenResultsShouldBeDisplayed()
		{
			//Thread.Sleep(300);
			string pageState = string.Format("Page Title: {0}, Browser Title: {1}, Page Url: {2}, Browser Page: {3}", resultPage.Title, resultPage.Browser.Title, resultPage.PageUrl, resultPage.Browser.Url);
			bool isOpen = resultPage.IsOpen();
			Asserts.IsTrue(isOpen, pageState);
		}

		[When(@"I submit the search")]
		public void WhenISubmitTheSearch()
		{
			resultPage = testPage.Submit(searchText);
		}
	}
}