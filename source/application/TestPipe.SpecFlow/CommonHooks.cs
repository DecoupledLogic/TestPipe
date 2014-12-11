namespace TestPipe.SpecFlow
{
	using System;
	using TechTalk.SpecFlow;

	[Binding]
	public sealed class CommonHooks
	{
		[AfterFeature]
		public static void TeardownFeature()
		{
      Console.WriteLine("TestPipeFeatureTearDown");
			Runner.TeardownFeature();
		}

		[BeforeTestRun]
		public static void SetupSuite()
		{ 
			Runner.SetupSuite();
		}

		[AfterTestRun]
		public static void TeardownSuite()
		{ 
			Runner.TeardownSuite();
		}
	}
}