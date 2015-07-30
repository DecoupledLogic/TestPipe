namespace TestPipe.Runner
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using TestPipe.Assertions;
    using TestPipe.Core;
    using TestPipe.Core.Exceptions;
    using TestPipe.Core.Interfaces;
    using TestPipe.Core.Session;

    public static class RunnerBase
	{
        public static SessionFeature SetupFeature(string title, string[] tags = null)
		{
            Console.WriteLine("Entered Setup Feature");
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

			currentFeature = RunnerHelper.LoadFeature(title);

			Console.WriteLine("\nEnd Feature Setup: " + title);

			return currentFeature;
		}

		public static SessionScenario SetupScenario(string title, string featureTitle, string[] tags = null)
		{
			if (RunnerHelper.IgnoreScenario(tags))
			{
				throw new IgnoreException();
			}

            SessionFeature currentFeature = TestSession.GetFeature(featureTitle, TestSession.Features);

			string scenarioId = RunnerHelper.GetIdFromTitle(title);

			SessionScenario currentScenario = TestSession.GetScenario(scenarioId, currentFeature);

			currentScenario.Browser = RunnerHelper.GetBrowser(tags);

            currentScenario.Asserts = new StepAsserts(currentScenario.Browser, featureTitle, title);

            RunnerHelper.IgnoreBrowserCertificateError(currentScenario.Browser);

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
            Console.WriteLine("Env in Setup Suite");
			RunnerHelper.SetEnvironment();
			TestSession.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(TestSession.Suite.Timeout));
            RunnerHelper.SetTestSessionDefaultBrowser();
		}

		public static void TeardownFeature()
		{
		}

		public static void TeardownScenario()
		{
		}

		public static void TeardownScenario(SessionScenario scenario)
		{
            if (scenario == null)
            {
                return;
            }

            if (scenario.Browser == null)
            {
                return;
            }
            
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
            TestSession.Suite = null;
            TestSession.Cache.Clear();
		}
	}
}