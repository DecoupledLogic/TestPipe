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
        private BasePage resultPage;
        private SessionScenario scenario;
        private string searchText;
        private SearchPage testPage;

        [BeforeFeature("@Search")]
        public static void SetupFeature()
        {
            feature = StepHelper.SetupFeature();
        }

        [Given(@"I am on the search page")]
        public void GivenIAmOnTheSearchPage()
        {
            this.testPage.Open();
        }

        [BeforeScenario("@Search")]
        public void SetupScenario()
        {
            this.scenario = StepHelper.SetupScenario();
            this.testPage = new SearchPage(this.scenario.Browser, TestSession.Environment);
        }

        [AfterScenario("@Search")]
        public void TeardownScenario()
        {
            StepHelper.TearDownScenario(this.scenario);
        }

        [Then(@"results should be displayed")]
        public void ThenResultsShouldBeDisplayed()
        {
            string pageState = string.Format("Page Title: {0}, Browser Title: {1}, Page Url: {2}, Browser Page: {3}", this.resultPage.Title, this.resultPage.Browser.Title, this.resultPage.PageUrl, this.resultPage.Browser.Url);
            bool isOpen = this.resultPage.IsOpen();
            Asserts.IsTrue(isOpen, pageState);
        }

        [When(@"I submit a search")]
        public void WhenISubmitASearch()
        {
            this.searchText = this.scenario.Data.Q;

            this.testPage.EnterText(this.testPage.Search, this.searchText);

            this.testPage = new SearchPage(this.scenario.Browser, TestSession.Environment);

            this.resultPage = this.testPage.Submit(this.searchText);
        }
    }
}