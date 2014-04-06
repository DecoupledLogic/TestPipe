namespace TestPipe.Core.Browser
{
	using System;
	using System.ComponentModel.Composition;
	using System.ComponentModel.Composition.Hosting;
	using System.Configuration;
	using System.IO;
	using TestPipe.Common;
	using TestPipe.Core;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;

	public class BrowserFactory
	{
		[Import(typeof(IBrowser))]
		private IBrowser browser;

		private ILogManager log;

		public BrowserFactory(ILogManager log)
		{
			this.log = log;
		}

		public static IBrowser Create(BrowserTypeEnum browserType, ILogManager log)
		{
			BrowserFactory factory = new BrowserFactory(log);
			return factory.Compose(browserType);
		}

		private IBrowser Compose(BrowserTypeEnum browserType)
		{
			this.browser = null;

			try
			{
				AggregateCatalog aggregateCatalogue = new AggregateCatalog();
				aggregateCatalogue.Catalogs.Add(new DirectoryCatalog(ConfigurationManager.AppSettings["browser.plugins"]));

				CompositionContainer container = new CompositionContainer(aggregateCatalogue);
				container.ComposeParts(this);
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

			this.browser.LoadBrowser(browserType);
			return this.browser;
		}
	}
}