namespace TestPipe.Specs.Browser
{
	using System;
	using System.Configuration;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Common;
	using TestPipe.Core;
	using TestPipe.Core.Browser;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;

	public abstract class BaseBrowserFixture
	{
		private IBrowser browser;

		public BaseBrowserFixture()
		{
			this.browser = GetBrowser();
		}

		public IBrowser BrowserInstance
		{
			get { return this.browser; }
			set { this.browser = value; }
		}

		public string RootPath
		{
			get
			{
				return ConfigurationManager.AppSettings["htmlsite"];
			}
		}

		public string TableTestPage
		{
			get
			{
				return this.RootPath + "tables.html";
			}
		}

		public string XhtmlTestPage
		{
			get
			{
				return this.RootPath + "xhtmlTest.html";
			}
		}

		[TestInitialize]
		public void SetUp()
		{
			
		}

		[TestCleanup]
		public void TearDown()
		{
			//this.browser.Quit();
		}

		public IBrowser GetBrowser()
		{
			ILogManager log = new Logger();
			return BrowserFactory.Create(BrowserTypeEnum.IE, log);
		}
	}
}