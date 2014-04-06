namespace TestPipe.SpecFlow.Specs
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Core;
	using TestPipe.Core.Enums;
	using TestPipe.Runner;

	[TestClass]
	public class RunnerBaseSpec
	{
		private TestContext testContextInstance;

		public RunnerBaseSpec()
		{
		}

		public TestContext TestContext
		{
			get
			{
				return this.testContextInstance;
			}
			set
			{
				this.testContextInstance = value;
			}
		}

		[TestMethod]
		public void Ignore_Given_All_Matching_Then_Return_False()
		{
			//feature file tags
			string[] tags = { "all", "all" };

			//config file tags
			string runTags = "All";

			bool expected = false;

			bool actual = RunnerBase.Ignore(tags, runTags);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Ignore_Given_Empty_RunTags_Then_Return_False()
		{
			//feature file tags
			string[] tags = { "all", "all" };

			//config file tags
			string runTags = string.Empty;

			bool expected = false;

			bool actual = RunnerBase.Ignore(tags, runTags);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(TestPipe.Core.Exceptions.IgnoreException), "Ignored")]
		public void Ignore_Given_Tags_Containing_Ignore_Then_Throw_Exception()
		{
			//feature file tags
			string[] tags = { "ignore" };

			//config file tags
			string runTags = "all";

			bool expected = true;

			bool actual = RunnerBase.Ignore(tags, runTags);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(TestPipe.Core.Exceptions.IgnoreException), "Incomplete")]
		public void Ignore_Given_Tags_Containing_Incomplete_Then_Throw_Exception()
		{
			//feature file tags
			string[] tags = { "incomplete" };

			//config file tags
			string runTags = "all";

			bool expected = true;

			bool actual = RunnerBase.Ignore(tags, runTags);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(TestPipe.Core.Exceptions.IgnoreException), "Manual")]
		public void Ignore_Given_Tags_Containing_Manual_Then_Throw_Exception()
		{
			//feature file tags
			string[] tags = { "manual" };

			//config file tags
			string runTags = "all";

			bool expected = true;

			bool actual = RunnerBase.Ignore(tags, runTags);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Ignore_Given_Tags_Containing_RunTags_Then_Return_True()
		{
			//feature file tags
			string[] tags = { "Hello", "world" };

			//config file tags
			string runTags = "hello";

			bool expected = false;

			bool actual = RunnerBase.Ignore(tags, runTags);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(TestPipe.Core.Exceptions.IgnoreException), "Ignored tags is null.")]
		public void Ignore_Given_Tags_Null_Then_Return_True()
		{
			//feature file tags
			string[] tags = null;

			//config file tags
			string runTags = "all";

			bool expected = true;

			bool actual = RunnerBase.Ignore(tags, runTags);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SetTestSessionBrowser_Should_Set_Default_Browser()
		{
			BrowserTypeEnum expected = BrowserTypeEnum.IE;
			TestSession.Suite.SetupKeyPrefix = "Test";
			TestSession.Cache[TestSession.Suite.SetupKeyPrefix + TestSession.BrowserNameKey] = "IE";

			RunnerBase.SetTestSessionBrowser();

			BrowserTypeEnum actual = TestSession.Browser.BrowserType;

			TestSession.Browser.Quit();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SetTestSessionBrowser_Should_Set_Named_Browser()
		{
			BrowserTypeEnum expected = BrowserTypeEnum.Chrome;

			RunnerBase.SetTestSessionBrowser("Chrome");

			BrowserTypeEnum actual = TestSession.Browser.BrowserType;

			TestSession.Browser.Quit();

			Assert.AreEqual(expected, actual);
		}
	}
}