namespace TestPipe.Demo.Pages
{
	using System;
	using TestPipe.Core;
	using TestPipe.Core.Control;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;
	using TestPipe.Core.Page;

	public class SearchPage : BasePage
	{
		private static readonly string PageTitle = "Google Search";
		private static readonly string Url = "/";
        
        public SearchPage(IBrowser browser, TestEnvironment testEnvironment)
			: base(browser, testEnvironment)
		{
			this.PageRelativeUrl = Url;
			this.Title = PageTitle;
		}

        public BaseControl Search 
        { 
            get
            {
                return this.GetBaseControl(FindByEnum.Id, "gbqfq");
            }
        }

        public BaseControl SearchButton
        {
            get
            {
                return this.GetBaseControl(FindByEnum.Id, "gbqfb");
            }
        }

		public BasePage Submit(string text)
		{
			this.SearchButton.Click();

			BasePage resultPage = new SearchPage(this.Browser, this.TestEnvironment);

			resultPage.Title = text + " - " + SearchPage.PageTitle;
			
			this.Browser.WaitForPageLoad(4, resultPage.Title);
			
			return resultPage;
		}
	}
}