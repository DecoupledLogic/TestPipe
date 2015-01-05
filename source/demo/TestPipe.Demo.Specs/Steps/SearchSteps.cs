﻿namespace TestPipe.Demo.Steps
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
			testPage = new SearchPage(scenario.Browser, TestSession.Environment);
		}

		[BeforeFeature]
		public static void SetupSearchFeature()
		{
			feature = StepHelper.SetupFeature();
		}

		[Given(@"I am on the search page")]
		public void GivenIAmOnTheSearchPage()
		{
			testPage.Open();
		}

		[Then(@"results should be displayed")]
		public void ThenResultsShouldBeDisplayed()
		{
			string pageState = string.Format("Page Title: {0}, Browser Title: {1}, Page Url: {2}, Browser Page: {3}", resultPage.Title, resultPage.Browser.Title, resultPage.PageUrl, resultPage.Browser.Url);
			bool isOpen = resultPage.IsOpen();
			Asserts.IsTrue(isOpen, pageState);
		}

		[When(@"I submit a search")]
		public void WhenISubmitASearch()
		{
            searchText = scenario.Data.Q;

            testPage.EnterText(testPage.Search, searchText);

            testPage = new SearchPage(scenario.Browser, TestSession.Environment);

			resultPage = testPage.Submit(searchText);
		}
	}
}