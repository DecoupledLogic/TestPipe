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
	public class BrowserFactory
	{
		[Import(typeof(IBrowser))]
		private IBrowser browser;
        private BrowserConfiguration config;
		private BrowserConfiguration config;
		private ILogManager log;

		public BrowserFactory(ILogManager log, BrowserConfiguration config = null)
		{
            this.config = config;
            //TODO: Uncomment
			//this.log = log;
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
                //TODO: Uncomment
				//this.log.Error("File not found while composing browser.", ex);
			}
			catch (CompositionException ex)
			{
                //TODO: Uncomment
				//this.log.Error("Composition exception while composing browser.", ex);
			}

			if (this.browser == null)
			{
				string nullBrowserMessage = "Browser is null.";
                //TODO: Uncomment
				//this.log.Error(nullBrowserMessage);
				throw new Exception(nullBrowserMessage);
			}

			this.browser.LoadBrowser(browserType, this.config);
			return this.browser;
		}
	}
}