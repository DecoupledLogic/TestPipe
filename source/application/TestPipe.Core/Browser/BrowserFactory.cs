namespace TestPipe.Core.Browser
{
	using System;
	using System.ComponentModel.Composition;
	using System.IO;
	using TestPipe.Common;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;
	using TestPipe.Selenium;

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class BrowserFactory : LoggedObject
	{
		[Import(typeof(IBrowser))]
		private IBrowser browser;
		private BrowserConfiguration config;

		public BrowserFactory(ILogManager log, BrowserConfiguration config = null)
            : base(log)
		{
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
				this.browser = new Selenium.Browsers.Browser();

				//AggregateCatalog aggregateCatalogue = new AggregateCatalog();
				//aggregateCatalogue.Catalogs.Add(new DirectoryCatalog(ConfigurationManager.AppSettings["browser.plugins"]));
				//CompositionContainer container = new CompositionContainer(aggregateCatalogue);
				//container.ComposeParts(this);
			}
			catch (FileNotFoundException ex)
			{
				this.Log.Error("File not found while composing browser.", ex);
			}
			catch (CompositionException ex)
			{
				this.Log.Error("Composition exception while composing browser.", ex);
			}

			if (this.browser == null)
			{
				string nullBrowserMessage = "Browser is null.";
				this.Log.Error(nullBrowserMessage);
				throw new Exception(nullBrowserMessage);
			}

			this.browser.LoadBrowser(browserType, this.config);
            this.Log.Info(string.Format("Browser Loaded: {0}", browser.BrowserType.ToString()));
			return this.browser;
		}
	}
}