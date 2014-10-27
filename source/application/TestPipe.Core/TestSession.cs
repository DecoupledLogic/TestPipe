﻿namespace TestPipe.Core
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

	public static class TestSession
	{
		public static readonly string DefaultBrowserKey = "default_browser";
		public static readonly string DriverKey = "driver";
		public static readonly string EnvironmentKey = "environment";
		public static readonly string LogoutUrlKey = "logout_url";
		public static readonly string WaitKey = "wait";
		private static Cache<string, object> cache;

		public static string ApplicationKey { get; set; }

		public static IBrowser Browser
		{
			get
			{
				try
				{
					if (TestSession.Cache[DriverKey] == null)
					{
						return null;
					}
				}
				catch (KeyNotFoundException)
				{
					return null;
				}

				return TestSession.Cache[DriverKey] as IBrowser;
			}
			set
			{
				if (Cache.ContainsKey(DriverKey))
				{
					object o = null;
					Cache.TryRemove(DriverKey, out o);
					Cache[DriverKey] = value;
				}
				else
				{
					Cache[DriverKey] = value;
				}
			}
		}

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

		public static IBrowserWait Wait
		{
			get
			{
				return (IBrowserWait)TestSession.Cache[WaitKey];
			}
			set
			{
				if (Cache.ContainsKey(WaitKey))
				{
					object o = null;
					Cache.TryRemove(WaitKey, out o);
					Cache[WaitKey] = value;
				}
				else
				{
					Cache[WaitKey] = value;
				}
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

		// Uncomment below if ids are significant for features 
		//public static SessionFeature GetFeature(string id)
		// Comment below if ids are significant for features 
		public static SessionFeature GetFeature(string title)
		{
			if (Features == null)
			{
				throw new NullReferenceException("Features cannot be a null value.");
			}

			SessionFeature feature = TestSession.Features.Where(x => x.Title == title).FirstOrDefault();

			if (feature == null)
			{
				throw new NullReferenceException("feature cannot be a null value.");
			}

			return feature;
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

		public static void LoadFeature(string path)
		{
			string json = LoadData(path);

			var data = Helpers.JsonSerialization.Deserialize(json, typeof(SessionFeature));

			if (data == null)
			{
				throw new NullReferenceException("data can not be null.");
			}

			SessionFeature feature = (SessionFeature)data;

			TestSession.Features.Add(feature);
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