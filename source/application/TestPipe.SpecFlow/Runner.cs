namespace TestPipe.SpecFlow
{
	using System;
	using TechTalk.SpecFlow;
	using TestPipe.Core.Session;
	using TestPipe.Runner;

	public static class Runner
	{
        public static SessionFeature SetupFeature()
        {
            FeatureInfo current = FeatureContext.Current.FeatureInfo;
            return RunnerBase.SetupFeature(current.Title, current.Tags);
        }

        public static SessionScenario SetupScenario()
        {
            ScenarioInfo current = ScenarioContext.Current.ScenarioInfo;
            FeatureInfo feature = FeatureContext.Current.FeatureInfo;
            return RunnerBase.SetupScenario(current.Title, feature.Title, current.Tags);
        }

		public static SessionFeature SetupFeature(string[] tags, string title)
		{
			//FeatureInfo current = FeatureContext.Current.FeatureInfo;
			//return RunnerBase.SetupFeature(current.Title, current.Tags);
            return RunnerBase.SetupFeature(title, tags);
		}

		public static SessionScenario SetupScenario(string[] tags, string fTitle, string sTitle)
		{
			//ScenarioInfo current = ScenarioContext.Current.ScenarioInfo;
			//FeatureInfo feature = FeatureContext.Current.FeatureInfo;
			return RunnerBase.SetupScenario(sTitle, fTitle, tags);
		}

		public static void SetupSuite()
		{
			RunnerBase.SetupSuite();
		}

		public static void TeardownFeature()
		{
			RunnerBase.TeardownFeature();
		}

		public static void TeardownScenario()
		{
			RunnerBase.TeardownScenario();
		}

		public static void TeardownSuite()
		{
			RunnerBase.TeardownSuite();
		}
	}
}