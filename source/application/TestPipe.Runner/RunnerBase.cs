namespace TestPipe.Runner
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using TestPipe.Assertions;
	using TestPipe.Core;
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

			SessionFeature currentFeature;

			try
			{
				currentFeature = RunnerHelper.LoadFeature(title);
			}
			catch (TestPipeException)
			{

				throw;
			}

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

			//Uncommnet below if ids are significant for features
			//string featureId = RunnerHelper.GetIdFromTitle(featureTitle);
			//Uncomment below if ids are significant for features
			//SessionFeature currentFeature = TestSession.Features.Where(x => x.Id == featureId).FirstOrDefault();
			//Commnet below if ids are significant for features
			SessionFeature currentFeature = TestSession.Features.Where(x => x.Title == featureTitle).FirstOrDefault();

			string scenarioId = RunnerHelper.GetIdFromTitle(title);

			SessionScenario currentScenario = TestSession.GetScenario(scenarioId, currentFeature);

			RunnerHelper.SetScenarioBrowser(tags, currentFeature, currentScenario);

			return currentScenario;
		}

		public static void SetupSuite()
		{
			if (TestSession.Suite != null)
			{
				if (TestSession.Browser == null)
				{
					TestSession.Browser = RunnerHelper.SetBrowser(TestSession.Suite.Browser);
				}
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
		}

		public static void TeardownScenario()
		{
			TestSession.Browser.Open(string.Format("{0}{1}", TestSession.Suite.BaseUrl, TestSession.Suite.LogoutUrl));
		}

		public static void TeardownScenario(SessionScenario scenario)
		{
			scenario.Browser.Open(string.Format("{0}{1}", TestSession.Suite.BaseUrl, TestSession.Suite.LogoutUrl));
			scenario.Browser.Quit();
			scenario.Browser = null;
		}

		public static void TeardownSuite()
		{
			if (TestSession.Features.Count > 0)
			{
				foreach (var feature in TestSession.Features)
				{
					if (feature.Scenarios.Count < 0)
					{
						return;
					}

					foreach (var scenario in feature.Scenarios)
					{
						if (scenario.Browser != null)
						{
							scenario.Browser.Quit();
							scenario.Browser = null;
						}
					}

					if (feature.Browser != null)
					{
						feature.Browser.Quit();
						feature.Browser = null;
					}
				}
			}

			if (TestSession.Browser != null)
			{
				TestSession.Browser.Quit();
				TestSession.Browser = null;
			}
			
			TestSession.Suite = null;
		}
	}
}