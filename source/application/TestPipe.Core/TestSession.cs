namespace TestPipe.Core
{
	using System;
	using TestPipe.Common;
	using TestPipe.Core.Browser;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;

	public static class TestSession
	{
		public static readonly string BaseUrlKey = "base_url";
		public static readonly string BrowserNameKey = "Browser";
		public static readonly string DefaultBrowserKey = "default_browser";
		public static readonly string DriverKey = "driver";
		public static readonly string EnvironmentKey = "environment";
		public static readonly string LogoutUrlKey = "logout_url";
		public static readonly string WaitKey = "wait";
		private static Cache<string, object> cache;
		private static XmlTool featureConfig;
		private static XmlTool suiteConfig;

		public static string BaseUrl
		{
			get
			{
				return TestSession.Cache[BaseUrlKey].ToString();
			}
			set
			{
				TestSession.Cache[BaseUrlKey] = value;
			}
		}

		public static IBrowser Browser
		{
			get
			{
				return (IBrowser)cache[DriverKey];
			}
			set
			{
				cache[DriverKey] = value;
			}
		}

		public static Cache<string, object> Cache
		{
			get
			{
				if (cache == null)
					cache = new Cache<string, object>();

				return cache;
			}

			set
			{
				cache = value;
			}
		}

		public static BrowserTypeEnum DefaultBrowser
		{
			get
			{
				BrowserTypeEnum browserType = BrowserTypeEnum.None;

				try
				{
					if (TestSession.Cache[DefaultBrowserKey].GetType() == typeof(BrowserTypeEnum))
					{
						browserType = (BrowserTypeEnum)TestSession.Cache[DefaultBrowserKey];
					}
				}
				catch (System.Collections.Generic.KeyNotFoundException)
				{
				}
				catch (NullReferenceException)
				{
				}

				return browserType;
			}
			set
			{
				TestSession.Cache[DefaultBrowserKey] = value;
			}
		}

		public static TestEnvironment Environment
		{
			get
			{
				return (TestEnvironment)cache[EnvironmentKey];
			}
			set
			{
				cache[EnvironmentKey] = value;
			}
		}

		public static string LogoutUrl
		{
			get
			{
				return TestSession.Cache[LogoutUrlKey].ToString();
			}
			set
			{
				TestSession.Cache[LogoutUrlKey] = value;
			}
		}

		public static TimeSpan Timeout { get; set; }

		public static IBrowserWait Wait
		{
			get
			{
				return (IBrowserWait)cache[WaitKey];
			}
			set
			{
				cache[WaitKey] = value;
			}
		}

		public static XmlTool XmlFeatureConfig
		{
			get
			{
				if (featureConfig == null)
					featureConfig = new XmlTool();
				return featureConfig;
			}
			set
			{
				featureConfig = value;
			}
		}

		public static XmlTool XmlSuiteConfig
		{
			get
			{
				if (suiteConfig == null)
					suiteConfig = new XmlTool();
				return suiteConfig;
			}
			set
			{
				suiteConfig = value;
			}
		}

		public static IBrowser CreateBrowserDriver(BrowserTypeEnum browserType)
		{
			ILogManager log = new Logger();
			return BrowserFactory.Create(browserType, log);
		}

		public static IBrowserWait CreateBrowserWait(IBrowser browser, TimeSpan timeout, TimeSpan? sleepInterval = null, IClock clock = null)
		{
			return null;
		}

		public static class Feature
		{
			public static string KeyPrefix { get; set; }
		}

		public static class Scenario
		{
			public static string KeyPrefix { get; set; }
		}

		public static class Suite
		{
			public static string KeyPrefix { get; set; }

			public static string SetupKeyPrefix { get; set; }
		}
	}
}