namespace TestPipe.Core.Browser
{
	using System;
	using System.ComponentModel.Composition;
	using System.IO;
	using TestPipe.Common;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
	public class BrowserFactory
	{
		[Import(typeof(IBrowser))]
		private IBrowser browser;

		private BrowserConfiguration config;
		private ILogManager log;

		public BrowserFactory(ILogManager log, BrowserConfiguration config = null)
		{
			this.log = log;
			this.config = config;
		}

		public static IBrowser Create(BrowserTypeEnum browserType, ILogManager log, BrowserConfiguration config = null)
		{
			BrowserFactory factory = new BrowserFactory(log, config);
			return factory.Compose(browserType);
		}

		private IBrowser Compose(BrowserTypeEnum browserType)
		{
			this.browser = null;

			try
			{
				this.browser = new TestPipe.Selenium.Browsers.Browser();
			}
			catch (FileNotFoundException ex)
			{
				this.log.Error("File not found while composing browser.", ex);
			}
			catch (CompositionException ex)
			{
				this.log.Error("Composition exception while composing browser.", ex);
			}

			if (this.browser == null)
			{
				string nullBrowserMessage = "Browser is null.";
				this.log.Error(nullBrowserMessage);
				throw new Exception(nullBrowserMessage);
			}

			this.browser.LoadBrowser(browserType, config);
			return this.browser;
		}
	}
}