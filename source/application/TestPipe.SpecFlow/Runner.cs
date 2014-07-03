namespace TestPipe.SpecFlow
{
	using System;
	using TechTalk.SpecFlow;
	using TestPipe.Runner;

	public static class Runner
	{
		public static void SetupFeature()
		{
			FeatureInfo current = FeatureContext.Current.FeatureInfo;
			RunnerBase.SetupFeature(current.Title, current.Tags);
		}

		public static void SetupScenario()
		{
			ScenarioInfo current = ScenarioContext.Current.ScenarioInfo;
			FeatureInfo feature = FeatureContext.Current.FeatureInfo;
			RunnerBase.SetupScenario(current.Title, feature.Title, current.Tags);
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