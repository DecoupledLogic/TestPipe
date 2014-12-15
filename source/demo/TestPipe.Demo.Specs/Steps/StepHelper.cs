namespace TestPipe.Demo.Steps
{
	using System;
	using NUnit.Framework;
	using TestPipe.Core.Session;
	using TestPipe.SpecFlow;

	internal sealed class StepHelper
	{
		internal static SessionFeature SetupFeature()
		{
			SessionFeature feature = new SessionFeature();

			try
			{
				feature = Runner.SetupFeature();
			}
			catch (TestPipe.Core.Exceptions.IgnoreException ex)
			{
				Assert.Ignore(ex.Message);
			}

			return feature;
		}

		internal static SessionFeature SetupFeature(string[] tags, string title)
		{
			SessionFeature feature = new SessionFeature();

			try
			{
				feature = Runner.SetupFeature(tags, title);
			}
			catch (TestPipe.Core.Exceptions.IgnoreException ex)
			{
				Assert.Ignore(ex.Message);
			}

			return feature;
		}

		internal static SessionScenario SetupScenario()
		{
			SessionScenario scenario = new SessionScenario();
			try
			{
				scenario = Runner.SetupScenario();
			}
			catch (TestPipe.Core.Exceptions.IgnoreException ex)
			{
				Assert.Ignore(ex.Message);
			}

			return scenario;
		}

		internal static SessionScenario SetupScenario(string[] tags, string featureTitle, string scenarioTitle)
		{
			SessionScenario scenario = new SessionScenario();
			try
			{
				scenario = Runner.SetupScenario(tags, featureTitle, scenarioTitle);
			}
			catch (TestPipe.Core.Exceptions.IgnoreException ex)
			{
				Assert.Ignore(ex.Message);
			}

			return scenario;
		}
	}
}