namespace TestPipe.Runner
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Text;
	using System.Xml.Linq;
	using TestPipe.Assertions;
	using TestPipe.Core;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Exceptions;
	using TestPipe.Core.Interfaces;

	public static class RunnerBase
	{
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

		public static void SetTestSessionBrowser()
		{
			string browserName = TestSession.Suite.Browser;

			BrowserTypeEnum browser = RunnerBase.GetBrowserType(browserName);

			TestSession.DefaultBrowser = browser;

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
			RunnerBase.SetupSuite();

			if (TestSession.Features == null)
			{
				TestSession.Features = new List<TestPipe.Core.Session.Feature>();
			}

			Console.WriteLine("\nBegin Feature Setup: " + title);

			if (IgnoreFeature(tags))
			{
				Console.WriteLine("\nFeature Ignored: " + title);
				Console.WriteLine("\nEnd Feature Setup: " + title);
				Asserts.Ignored();
				return;
			}

			string featureKey = RunnerBase.GetKeyFromTitle(title);
			TestPipe.Core.Session.Feature feature = TestSession.Suite.Features.Where(x => x.Id == featureKey).FirstOrDefault();

			string path = RunnerBase.GetDataFilePath(feature.Path);
			TestSession.LoadFeature(path);

			TestPipe.Core.Session.Feature currentFeature = TestSession.Features.Where(x => x.Id == featureKey).FirstOrDefault();

			SetFeatureBrowser(tags, currentFeature);

			Console.WriteLine("\nEnd Feature Setup: " + title);
		}

		public static void SetupScenario(string title, string featureTitle, string[] tags = null)
		{
			if (IgnoreScenario(tags))
			{
				throw new IgnoreException();
			}

			string featureKey = RunnerBase.GetKeyFromTitle(featureTitle);
			TestPipe.Core.Session.Feature currentFeature = TestSession.Features.Where(x => x.Id == featureKey).FirstOrDefault();

			string scenarioKey = RunnerBase.GetKeyFromTitle(title);
			TestPipe.Core.Session.Scenario currentScenario = currentFeature.Scenarios.Where(x => x.Id == scenarioKey).FirstOrDefault();

			SetScenarioBrowser(tags, currentFeature, currentScenario);
		}

		public static void SetupSuite()
		{
			if (TestSession.Suite != null)
			{
				return;
			}
			string file = ConfigurationManager.AppSettings["file.testSuite"];
			string path = RunnerBase.GetDataFilePath(file);
			TestSession.LoadSuite(path);

			RunnerBase.SetEnvironment();

			TestSession.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(TestSession.Suite.Timeout));

			TestSession.Browser = SetBrowser(TestSession.Suite.Browser);
		}

		private static string GetDataFilePath(string file)
		{
			string basePath = ConfigurationManager.AppSettings["file.basePath"];

			if(!basePath.EndsWith(@"\"))
			{
				basePath += @"\";
			}

			return string.Format(@"{0}{1}", basePath, file);
		}

		public static void TeardownFeature()
		{
			TestSession.Browser.Quit();

			TestSession.Cache.Clear();
		}

		public static void TeardownScenario()
		{
			TestSession.Browser.Open(string.Format("{0}{1}", TestSession.Suite.BaseUrl, TestSession.Suite.LogoutUrl));
		}

		public static void TeardownSuite()
		{
			TestSession.Suite = null;
		}

		private static string GetAppConfigValue(string key)
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

		private static string GetBrowserFromTag(string[] tags)
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

		private static BrowserTypeEnum GetBrowserType(string browserName)
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

		private static string GetKeyFromTitle(string title)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentException();
			}

			return title.Substring(0, title.IndexOf(".")).Trim();
		}

		private static bool IgnoreFeature(string[] tags)
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

		private static bool IgnoreScenario(string[] tags)
		{
			if (tags == null)
			{
				return false;
			}

			string runTags = GetAppConfigValue("test.scenarios");

			runTags = runTags.Trim().ToLower();

			return Ignore(tags, runTags);
		}

		private static IBrowser SetBrowser(string browserName)
		{
			string isBrowserTest = ConfigurationManager.AppSettings["browserTest"];

			if (!string.IsNullOrWhiteSpace(isBrowserTest) && isBrowserTest.ToLower() == "false")
			{
				return null;
			}

			BrowserTypeEnum browserType = RunnerBase.GetBrowserType(browserName);

			IBrowser browser = TestSession.CreateBrowserDriver(browserType);

			RunnerBase.SetBrowserWait();

			return browser;
		}

		private static IBrowser SetBrowserFromTag(string[] tags)
		{
			if (tags == null)
			{
				return null;
			}

			string browserName = GetBrowserFromTag(tags);

			if (string.IsNullOrWhiteSpace(browserName))
			{
				return null;
			}

			IBrowser browser = SetBrowser(browserName);

			return browser;
		}

		private static void SetBrowserWait()
		{
			if (TestSession.Suite.Timeout == 0)
			{
				return;
			}

			TestSession.Wait = TestSession.CreateBrowserWait(TestSession.Browser, TestSession.Timeout);
		}

		private static void SetEnvironment()
		{
			TestEnvironment environment = new TestEnvironment();
			Guid key = Guid.Empty;
			Guid.TryParse(TestSession.Suite.ApplicationKey, out key);
			environment.ApplicationKey = key;
			environment.BaseUrl = TestSession.Suite.BaseUrl;
			environment.Title = TestSession.Suite.Environment;

			TestSession.Environment = environment;
		}

		private static void SetFeatureBrowser(string[] tags, TestPipe.Core.Session.Feature currentFeature)
		{
			IBrowser browser = SetBrowserFromTag(tags);

			currentFeature.Browser = browser == null
				? TestSession.Browser
				: browser;
		}

		private static void SetScenarioBrowser(string[] tags, TestPipe.Core.Session.Feature currentFeature, TestPipe.Core.Session.Scenario currentScenario)
		{
			IBrowser browser = SetBrowserFromTag(tags);

			currentScenario.Browser = browser == null
				? currentFeature.Browser
				: browser;
		}
	}
}