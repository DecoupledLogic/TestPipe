﻿namespace TestPipe.SpecFlow
{
	using System;
	using TechTalk.SpecFlow;

	[Binding]
	public sealed class CommonHooks
	{
        //[BeforeFeature]
        //public static void SetupFeature()
        //{
        //    //Get feature test data from XML
        //    //Delete test data from database
        //    //Save test data to database
        //    Console.WriteLine("TestPipeBeforeFeatureSetup");
        //    Runner.SetupFeature();
        //}

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