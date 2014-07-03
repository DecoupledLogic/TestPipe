﻿namespace TestPipe.Core
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using TestPipe.Common;
	using TestPipe.Core.Browser;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;
	using TestPipe.Core.Session;

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
				Cache[DriverKey] = value;
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

		public static ICollection<Feature> Features { get; set; }

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

		public static Suite Suite { get; set; }

		public static IBrowserWait Wait
		{
			get
			{
				return (IBrowserWait)TestSession.Cache[WaitKey];
			}
			set
			{
				Cache[WaitKey] = value;
			}
		}

		public static TimeSpan Timeout { get; set; }

		public static void LoadFeature(string path)
		{
			string json = LoadData(path);

			var data = Helpers.JsonSerialization.Deserialize(json, typeof(Feature));

			if (data == null)
			{
				throw new NullReferenceException("data can not be null.");
			}

			Feature feature = (Feature)data;

			TestSession.Features.Add(feature);
		}
		
		public static void LoadSuite(string path)
		{
			string json = LoadData(path);

			var data = Helpers.JsonSerialization.Deserialize(json, typeof(Suite));

			if (data == null)
			{
				throw new NullReferenceException("data can not be null.");
			}

			TestSession.Suite = (Suite)data;
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