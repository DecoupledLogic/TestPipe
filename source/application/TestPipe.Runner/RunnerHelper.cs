namespace TestPipe.Runner
{
    using System;
    using System.Configuration;
    using System.Linq;
    using TestPipe.Core;
    using TestPipe.Core.Browser;
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

        public static IBrowser GetBrowser(string[] tags)
        {
            IBrowser browser = SetBrowserFromTag(tags);

            string defaultBrowserName = GetBrowserName(TestSession.DefaultBrowser);

            IBrowser currentScenarioBrowser = browser == null
                ? SetBrowser(defaultBrowserName)
                : browser;

            return currentScenarioBrowser;
        }

        public static BrowserConfiguration GetBrowserConfiguration(BrowserTypeEnum browserType)
        {
            if (browserType == BrowserTypeEnum.Remote)
            {
                BrowserConfiguration config = null;

                string driverName = TestSession.Suite.BrowserDriver;
                if (string.IsNullOrWhiteSpace(driverName) || driverName.ToLower() == "remote")
                {
                    driverName = "IE";
                }

                BrowserTypeEnum driverType = RunnerHelper.GetBrowserType(driverName);

                if (driverType == BrowserTypeEnum.None || driverType == BrowserTypeEnum.Remote)
                {
                    driverType = BrowserTypeEnum.IE;
                }

                TimeSpan timeout = TestSession.Timeout;
                string version = TestSession.Suite.DefaultBrowserVersion;
                string remoteDriverPath = TestSession.Suite.RemoteDriverPath;

                config = new BrowserConfiguration(driverType, timeout, version, remoteDriverPath);

                return config;
            }

            return null;
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

        public static string GetBrowserName(BrowserTypeEnum browserTypeEnum)
        {
            if (browserTypeEnum == BrowserTypeEnum.None || browserTypeEnum == BrowserTypeEnum.Other)
            {
                return BrowserTypeEnum.IE.ToString();
            }

            return browserTypeEnum.ToString();
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

        public static SessionFeature LoadFeature(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new TestPipeException("Parameter \"title\" can not be null owr white space.");
            }

            SessionFeature feature = TestSession.GetFeature(title, TestSession.Suite.Features);

            if (feature == null)
            {
                throw new TestPipeException(string.Format("TestSession.Suite.Features does not contain a feature with title \"{0}\".", title));
            }

            string path = RunnerHelper.GetDataFilePath(feature.Path);

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new TestPipeException(string.Format("Feature \"{0}\" has a null or empty Feature.Path.", title));
            }

            feature = TestSession.LoadFeature(path);

            return feature;
        }

        public static IBrowser SetBrowser(string browserName)
        {
            string isBrowserTest = ConfigurationManager.AppSettings["browserTest"];

            if (!string.IsNullOrWhiteSpace(isBrowserTest) && isBrowserTest.ToLower() == "false")
            {
                return null;
            }

            BrowserTypeEnum browserType = RunnerHelper.GetBrowserType(browserName);

            BrowserConfiguration config = RunnerHelper.GetBrowserConfiguration(browserType);

            IBrowser browser = TestSession.CreateBrowserDriver(browserType, config);

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

        public static void SetEnvironment()
        {
            TestEnvironment environment = new TestEnvironment();
            Guid key = Guid.Empty;
            Guid.TryParse(TestSession.Suite.ApplicationKey, out key);
            environment.ApplicationKey = key;
            environment.BaseUrl = TestSession.Suite.BaseUrl;
            environment.Title = TestSession.Suite.Environment;

            TestSession.Environment = environment;
            Console.WriteLine("Env Set in Set Env");
        }

        public static void SetTestSessionDefaultBrowser()
        {
            string browserName = TestSession.Suite.Browser;

            BrowserTypeEnum browser = RunnerHelper.GetBrowserType(browserName);

            if (TestSession.DefaultBrowser != BrowserTypeEnum.None)
            {
                return;
            }

            TestSession.DefaultBrowser = browser;
        }
    }
}