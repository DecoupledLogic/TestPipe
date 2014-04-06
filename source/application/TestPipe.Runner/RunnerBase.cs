namespace TestPipe.Runner
{
	using System;
	using System.Configuration;
	using System.Linq;
	using System.Text;
	using System.Xml.Linq;
	using TestPipe.Assertions;
	using TestPipe.Core;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Exceptions;

	public static class RunnerBase
	{
		public static string GetAppConfigValue(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentException("App config key is empty.");
			}

			string value = ConfigurationManager.AppSettings[key];

			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ApplicationException("Config value for " + key + " not set.");
			}

			return value;
		}

		public static string GetBrowserFromTag(string[] tags)
		{
			string browserName = string.Empty;
			string browserTagPrefix = "browser_";
			foreach (var tag in tags)
			{
				if (tag.StartsWith(browserTagPrefix, StringComparison.InvariantCultureIgnoreCase))
				{
					browserName = tag.Substring(browserTagPrefix.Length);
				}
			}

			return browserName;
		}

		public static BrowserTypeEnum GetBrowserType(string browserName)
		{
			BrowserTypeEnum type;

			if (!Enum.TryParse(browserName, true, out type))
			{
				return BrowserTypeEnum.None;
			}

			if (!Enum.IsDefined(typeof(BrowserTypeEnum), type) | browserName.ToString().Contains(","))
			{
				type = BrowserTypeEnum.None;
			}

			return type;
		}

		public static string GetFeatureConfigFilePathFromTitle(string title)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentException("title is empty.");
			}

			string[] parts = title.Split('.');
			string name = parts[1].Trim();
			string basePath = GetAppConfigValue("file.basePath");

			string path = basePath + name + "_feature.xml";

			return path;
		}

		public static string GetKeyFromTitle(string title)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentException();
			}

			return title.Substring(0, title.IndexOf(".")).Trim();
		}

		public static T GetObject<T>()
		{
			string typeName = typeof(T).Name;
			string idCacheKey = string.Format("{0}{1}{2}{3}", TestSession.Suite.KeyPrefix, TestSession.Feature.KeyPrefix, TestSession.Scenario.KeyPrefix, typeName);
			string id = (string)TestSession.Cache[idCacheKey];
			string objectKey = string.Format("{0}{1}{2}", TestSession.Suite.KeyPrefix, TestSession.Feature.KeyPrefix, id);
			return (T)TestSession.Cache[objectKey];
		}

		public static bool Ignore(string[] tags, string runTags)
		{
			if (string.IsNullOrWhiteSpace(runTags))
			{
				return false;
			}

			//If runTags is configured with a value the tag must match or is ignored
			if (tags == null)
			{
				throw new IgnoreException("Ignored tags is null.");
			}

			if (tags.Contains("ignore", StringComparer.InvariantCultureIgnoreCase))
			{
				throw new IgnoreException("Ignored");
			}

			if (tags.Contains("Incomplete", StringComparer.InvariantCultureIgnoreCase))
			{
				throw new IgnoreException("Incomplete");
			}

			if (tags.Contains("manual", StringComparer.InvariantCultureIgnoreCase))
			{
				throw new IgnoreException("Manual");
			}

			if (runTags == "all" || runTags == "all,all")
			{
				return false;
			}

			if (tags.Contains(runTags, StringComparer.InvariantCultureIgnoreCase))
			{
				return false;
			}

			return true;
		}

		public static bool IgnoreFeature(string[] tags)
		{
			if (tags == null)
			{
				return false;
			}

			string runSuites = GetAppConfigValue("test.suites");

			runSuites = runSuites.Trim().ToLower();

			string runFeatures = GetAppConfigValue("test.features");

			runFeatures = runFeatures.Trim().ToLower();

			string runTags = string.IsNullOrWhiteSpace(runSuites) ? runFeatures : string.Format("{0},{1}", runSuites, runFeatures);

			return Ignore(tags, runTags);
		}

		public static bool IgnoreScenario(string[] tags)
		{
			if (tags == null)
			{
				return false;
			}

			string runTags = GetAppConfigValue("test.scenarios");

			runTags = runTags.Trim().ToLower();

			return Ignore(tags, runTags);
		}

		public static void SetTestSessionBrowser(string browserName = null)
		{
			if (string.IsNullOrWhiteSpace(browserName))
			{
				browserName = (string)TestSession.Cache[TestSession.Suite.SetupKeyPrefix + TestSession.BrowserNameKey];

				if (TestSession.DefaultBrowser == BrowserTypeEnum.None)
				{
					TestSession.DefaultBrowser = RunnerBase.GetBrowserType(browserName);
				}
			}

			BrowserTypeEnum browser = RunnerBase.GetBrowserType(browserName);
			dynamic oldBrowser;
			TestSession.Cache.TryRemove(TestSession.DriverKey, out oldBrowser);

			if (oldBrowser != null)
			{
				oldBrowser.Quit();
			}

			TestSession.Browser = TestSession.CreateBrowserDriver(browser);
		}

		public static void SetupFeature(string title, string[] tags = null)
		{
			Console.WriteLine("\nBegin Feature Setup: " + title);

			if (IgnoreFeature(tags))
			{
				Console.WriteLine("\nFeature Ignored: " + title);
				Console.WriteLine("\nEnd Feature Setup: " + title);
				Asserts.Ignored();
				return;
			}

			TestSession.Cache.Clear();

			TestSession.XmlSuiteConfig.Load(@"Data\" + ConfigurationManager.AppSettings["file.testSuite"]);
			TestSession.XmlSuiteConfig.CacheSuite();
			TestSession.Suite.KeyPrefix = GetKeyPrefix(TestSession.XmlSuiteConfig.Document);

			string featureKey = RunnerBase.GetKeyFromTitle(title);
			XElement feature = TestSession.XmlSuiteConfig.GetElementByName(XmlTool.FeatureTag, featureKey);

			TestSession.XmlFeatureConfig.Load(@"Data\" + feature.Value);
			TestSession.XmlFeatureConfig.CacheFeature(TestSession.Suite.KeyPrefix, featureKey);

			TestSession.Feature.KeyPrefix = GetKeyPrefix(TestSession.XmlFeatureConfig.Document);

			TestSession.Suite.SetupKeyPrefix = GetSetupKeyPrefix(TestSession.Suite.KeyPrefix);

			string browserTest = ConfigurationManager.AppSettings["browserTest"];

			if (string.IsNullOrWhiteSpace(browserTest) || browserTest.ToLower() != "false")
			{
				SetTestSessionBrowser();
				TestSession.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(TestSession.Cache[TestSession.Suite.SetupKeyPrefix + "Wait_TimeInSeconds"]));
				SetTimeout();
			}

			SetEnvironment(TestSession.Suite.SetupKeyPrefix);

			Console.WriteLine("\nEnd Feature Setup: " + title);
		}

		public static void SetupScenario(string title, string[] tags = null)
		{
			if (IgnoreScenario(tags))
			{
				throw new IgnoreException();
			}

			string browserName = GetBrowserFromTag(tags);

			if (!string.IsNullOrWhiteSpace(browserName))
			{
				SetTestSessionBrowser(browserName);
				SetTimeout();
			}
			else
			{
				//if browser doesn't match config browser set it to config browser
				if (TestSession.Browser.BrowserType != TestSession.DefaultBrowser)
				{
					SetTestSessionBrowser();
				}
			}

			string scenarioNumber = RunnerBase.GetKeyFromTitle(title);

			TestSession.Scenario.KeyPrefix = string.Format("{0}_{1}_", XmlTool.ScenarioTag, scenarioNumber);
			TestSession.Cache.Clear(TestSession.Scenario.KeyPrefix);

			string suiteKey = TestSession.XmlSuiteConfig.GetElementCacheKey(XmlTool.SuiteTag);
			TestSession.XmlFeatureConfig.CacheScenario(suiteKey, scenarioNumber);
		}

		public static void SetupSuite()
		{
		}

		public static void TeardownFeature()
		{
			TestSession.Browser.Quit();

			TestSession.Cache.Clear();
		}

		public static void TeardownScenario()
		{
			TestSession.Browser.Open(string.Format("{0}{1}", TestSession.BaseUrl, TestSession.LogoutUrl));
			TestSession.Scenario.KeyPrefix = null;
		}

		public static void TeardownSuite()
		{
		}

		private static string GetKeyPrefix(XElement element, string suffix = "")
		{
			StringBuilder key = new StringBuilder();
			key.Append(element.Name.ToString());
			key.Append("_");

			if (element.Attribute("name") != null)
			{
				key.Append(element.Attribute("name").Value);
				key.Append("_");
			}

			if (string.IsNullOrWhiteSpace(suffix))
			{
				return key.ToString();
			}

			key.Append(suffix);
			key.Append("_");

			return key.ToString();
		}

		private static string GetSetupKeyPrefix(string prefix)
		{
			return string.Format("{0}{1}_", prefix, "Setup");
		}

		private static void SetEnvironment(string keyPrefix)
		{
			TestEnvironment environment = new TestEnvironment();
			string appKey = TestSession.Cache[keyPrefix + "ApplicationKey"] == null ? string.Empty : TestSession.Cache[keyPrefix + "ApplicationKey"].ToString();
			Guid key = Guid.Empty;
			Guid.TryParse(appKey, out key);
			environment.ApplicationKey = key;
			environment.BaseUrl = TestSession.BaseUrl;
			environment.Title = TestSession.Cache[keyPrefix + "Environment"] == null ? string.Empty : TestSession.Cache[keyPrefix + "Environment"].ToString();

			TestSession.Environment = environment;
		}

		private static void SetTimeout()
		{
			if (TestSession.Timeout == null)
			{
				return;
			}

			dynamic oldWait;
			TestSession.Cache.TryRemove(TestSession.WaitKey, out oldWait);

			TestSession.Wait = TestSession.CreateBrowserWait(TestSession.Browser, TestSession.Timeout);
		}
	}
}