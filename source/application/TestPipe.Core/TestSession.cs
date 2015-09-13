namespace TestPipe.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TestPipe.Common;
    using TestPipe.Core.Browser;
    using TestPipe.Core.Enums;
    using TestPipe.Core.Interfaces;
    using TestPipe.Core.Session;
    using TestPipe.Core.VideoRecorder;

    public static class TestSession
    {
        public static readonly string DefaultBrowserKey = "default_browser";
        public static readonly string DriverKey = "driver";
        public static readonly string EnvironmentKey = "environment";
        public static readonly string LogoutUrlKey = "logout_url";
        public static readonly string WaitKey = "wait";
        private static Cache<string, object> cache;

        public static string ApplicationKey { get; set; }

        public static Cache<string, object> Cache
        {
            get
            {
                if (cache == null)
                {
                    cache = new Cache<string, object>();
                }

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
                return (TestEnvironment)TestSession.Cache[EnvironmentKey];
            }
            set
            {
                Cache[EnvironmentKey] = value;
            }
        }

        public static ICollection<SessionFeature> Features { get; set; }

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

        public static SessionSuite Suite { get; set; }

        public static TimeSpan Timeout { get; set; }

        public static IBrowser CreateBrowserDriver(BrowserTypeEnum browserType, BrowserConfiguration config = null)
        {
            ILogManager log = new Logger();
            return BrowserFactory.Create(browserType, log, config);
        }

        public static IVideoRecorder CreateVideoRecorder()
        {
            ILogManager log = new Logger();
            return VideoRecorderFactory.Create(log);
        }

        public static SessionFeature GetFeature(string title)
        {
            if (Features == null)
            {
                throw new NullReferenceException("Features cannot be a null value.");
            }

            SessionFeature feature = TestSession.GetFeature(title, TestSession.Features);

            if (feature == null)
            {
                throw new NullReferenceException("feature cannot be a null value.");
            }

            return feature;
        }

        //TODO: Find elegant way to remove spaces between words
        public static SessionFeature GetFeature(string title, ICollection<SessionFeature> features)
        {
            return features.Where(x => x.Title.Trim().Replace(" ", string.Empty) == title.Trim().Replace(" ", string.Empty)).FirstOrDefault();
        }

        public static SessionScenario GetScenario(string id, SessionFeature feature)
        {
            if (feature == null)
            {
                throw new NullReferenceException("Feature cannot be a null value.");
            }

            SessionScenario scenario = feature.Scenarios.Where(x => x.Id == id).FirstOrDefault();

            if (feature == null)
            {
                throw new NullReferenceException("scenario cannot be a null value.");
            }

            return scenario;
        }

        public static SessionFeature LoadFeature(string path)
        {
            string json = LoadData(path);

            var data = Helpers.JsonSerialization.Deserialize(json, typeof(SessionFeature));

            if (data == null)
            {
                throw new NullReferenceException("data can not be null.");
            }

            SessionFeature feature = (SessionFeature)data;

            TestSession.Features.Add(feature);

            return feature;
        }

        public static void LoadSuite(string path)
        {
            string json = LoadData(path);

            var data = Helpers.JsonSerialization.Deserialize(json, typeof(SessionSuite));

            if (data == null)
            {
                throw new NullReferenceException("data can not be null.");
            }

            TestSession.Suite = (SessionSuite)data;
        }

        private static string Load(string path)
        {
            string data = string.Empty;

            using (StreamReader r = new StreamReader(path))
            {
                data = r.ReadToEnd();
            }

            return data;
        }

        private static string LoadData(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("path can not be null or white space.");
            }

            string json = TestSession.Load(path);

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ApplicationException("json can not be null or white space.");
            }

            return json;
        }
    }
}