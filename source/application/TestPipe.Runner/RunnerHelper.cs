namespace TestPipe.Runner
{
	using System;
	using System.Configuration;
	using System.Linq;
	using TestPipe.Core;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Exceptions;
	using TestPipe.Core.Interfaces;
	using TestPipe.Core.Session;

	public class RunnerHelper
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

		public static string GetDataFilePath(string file)
		{
			string basePath = ConfigurationManager.AppSettings["file.basePath"];

			if (!basePath.EndsWith(@"\"))
			{
				basePath += @"\";
			}

			return string.Format(@"{0}{1}", basePath, file);
		}

		public static string GetIdFromTitle(string title)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentException();
			}

			return title.Substring(0, title.IndexOf(".")).Trim();
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

			return RunnerHelper.Ignore(tags, runTags);
		}

		public static bool IgnoreScenario(string[] tags)
		{
			if (tags == null)
			{
				return false;
			}

			string runTags = GetAppConfigValue("test.scenarios");

			runTags = runTags.Trim().ToLower();

			return RunnerHelper.Ignore(tags, runTags);
		}

		public static string LoadFeature(string title)
		{
            /* Uncomment below if ids are significant for features */
			//string featureKey = RunnerHelper.GetIdFromTitle(title);
            /* Uncomment below if ids are significant for features */
			//SessionFeature feature = TestSession.Suite.Features.Where(x => x.Id == featureKey).FirstOrDefault();
            /* Comment below if ids are significant for features */
            SessionFeature feature = TestSession.Suite.Features.Where(x => x.Title == title).FirstOrDefault();
			string path = RunnerHelper.GetDataFilePath(feature.Path);
			TestSession.LoadFeature(path);
            /* Uncomment below if ids are significant for features */
			//return featureKey;
            /* Comment below if ids are significant for features */
            return title;
		}

		public static IBrowser SetBrowser(string browserName)
		{
			string isBrowserTest = ConfigurationManager.AppSettings["browserTest"];

			if (!string.IsNullOrWhiteSpace(isBrowserTest) && isBrowserTest.ToLower() == "false")
			{
				return null;
			}

			BrowserTypeEnum browserType = RunnerHelper.GetBrowserType(browserName);

			IBrowser browser = TestSession.CreateBrowserDriver(browserType);

			RunnerHelper.SetBrowserWait();

			return browser;
		}

		public static IBrowser SetBrowserFromTag(string[] tags)
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

		public static void SetBrowserWait()
		{
			if (TestSession.Suite.Timeout == 0)
			{
				return;
			}

			TestSession.Wait = TestSession.CreateBrowserWait(TestSession.Browser, TestSession.Timeout);
		}

		public static void SetEnvironment()
		{
			TestEnvironment environment = new TestEnvironment();
			Guid key = Guid.Empty;
			Guid.TryParse(TestSession.Suite.ApplicationKey, out key);
			environment.ApplicationKey = key;
			environment.BaseUrl = TestSession.Suite.BaseUrl;
			environment.Title = TestSession.Suite.Environment;

			TestSession.Environment = environment;
		}

		public static void SetFeatureBrowser(string[] tags, SessionFeature currentFeature)
		{
			IBrowser browser = SetBrowserFromTag(tags);

			currentFeature.Browser = browser == null
				? TestSession.Browser
				: browser;
		}

		public static void SetScenarioBrowser(string[] tags, SessionFeature currentFeature, SessionScenario currentScenario)
		{
			IBrowser browser = SetBrowserFromTag(tags);

			currentScenario.Browser = browser == null
				? currentFeature.Browser
				: browser;
		}

		public static void SetTestSessionBrowser()
		{
			string browserName = TestSession.Suite.Browser;

			BrowserTypeEnum browser = RunnerHelper.GetBrowserType(browserName);

			TestSession.DefaultBrowser = browser;

			dynamic oldBrowser;

			TestSession.Cache.TryRemove(TestSession.DriverKey, out oldBrowser);

			if (oldBrowser != null)
			{
				oldBrowser.Quit();
			}

			TestSession.Browser = TestSession.CreateBrowserDriver(browser);
		}
	}
}