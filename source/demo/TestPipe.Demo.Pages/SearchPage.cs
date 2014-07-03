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
		private static readonly ISelect SearchSelector = new Select(FindByEnum.Id, "gbqfq");
		private static readonly ISelect SearchButtonSelector = new Select(FindByEnum.Id, "gbqfb");
		private static readonly string PageTitle = "Google Search";
		private static readonly string Url = "/";

		private BaseControl search;
		private BaseControl searchButton;

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
				if (this.search == null)
				{
					this.search = new BaseControl(this.Browser, SearchSelector);
				}
				return this.search;
			}
		}

		public BaseControl SearchButton
		{
			get
			{
				if (this.searchButton == null)
				{
					this.searchButton = new BaseControl(this.Browser, SearchButtonSelector);
				}
				return this.searchButton;
			}
		}

		public BasePage Submit(string text)
		{
			this.SearchButton.Click();
			//System.Threading.
			BasePage resultPage = new SearchPage(this.Browser, this.TestEnvironment);

			resultPage.Title = text + " - " + SearchPage.PageTitle;
			return resultPage;
		}
	}
}