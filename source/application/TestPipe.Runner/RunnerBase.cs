﻿namespace TestPipe.Runner
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using TestPipe.Assertions;
	using TestPipe.Core;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Exceptions;
	using TestPipe.Core.Session;

	public static class RunnerBase
	{
		public static SessionFeature SetupFeature(string title, string[] tags = null)
		{
			RunnerBase.SetupSuite();

			if (TestSession.Features == null)
			{
				TestSession.Features = new List<SessionFeature>();
			}

			Console.WriteLine("\nBegin Feature Setup: " + title);

			if (RunnerHelper.IgnoreFeature(tags))
			{
				Console.WriteLine("\nFeature Ignored: " + title);
				Console.WriteLine("\nEnd Feature Setup: " + title);
				Asserts.Ignored();
				return new SessionFeature();
			}

			string featureId = RunnerHelper.LoadFeature(title);

			SessionFeature currentFeature = TestSession.GetFeature(featureId);

			RunnerHelper.SetFeatureBrowser(tags, currentFeature);

			Console.WriteLine("\nEnd Feature Setup: " + title);

			return currentFeature;
		}

		public static SessionScenario SetupScenario(string title, string featureTitle, string[] tags = null)
		{
			if (RunnerHelper.IgnoreScenario(tags))
			{
				throw new IgnoreException();
			}

			string featureId = RunnerHelper.GetIdFromTitle(featureTitle);

			SessionFeature currentFeature = TestSession.Features.Where(x => x.Id == featureId).FirstOrDefault();

			string scenarioId = RunnerHelper.GetIdFromTitle(title);

			SessionScenario currentScenario = TestSession.GetScenario(scenarioId, currentFeature);

			RunnerHelper.SetScenarioBrowser(tags, currentFeature, currentScenario);

			return currentScenario;
		}

		public static void SetupSuite()
		{
			if (TestSession.Suite != null)
			{
				return;
			}
			string file = ConfigurationManager.AppSettings["file.testSuite"];
			string path = RunnerHelper.GetDataFilePath(file);
			TestSession.LoadSuite(path);

			RunnerHelper.SetEnvironment();

			TestSession.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(TestSession.Suite.Timeout));

			TestSession.Browser = RunnerHelper.SetBrowser(TestSession.Suite.Browser);
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
	}
}