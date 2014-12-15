namespace TestPipe.Runner.Specs
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Core;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;
	using TestPipe.Core.Session;

	[TestClass]
	public class RunnerHelperSpec
	{
		[TestClass]
		public class GetBrowser
		{
			[TestMethod]
			public void GetBrowser_Should_Set_Default_Browser()
			{
				BrowserTypeEnum expected = BrowserTypeEnum.FireFox;
				TestPipe.Core.Session.SessionSuite suite = new TestPipe.Core.Session.SessionSuite();
				TestSession.Suite = suite;
				TestSession.Suite.Browser = "FireFox";

				RunnerHelper.SetTestSessionDefaultBrowser();
				string[] tags = null;
				IBrowser browser = RunnerHelper.GetBrowser(tags);
				BrowserTypeEnum actual = browser.BrowserType;

				browser.Quit();

				Assert.AreEqual(expected, actual);
			}

			[TestMethod]
			public void GetBrowser_Should_Set_Tagged_Browser()
			{
				BrowserTypeEnum expected = BrowserTypeEnum.Chrome;
				TestPipe.Core.Session.SessionSuite suite = new TestPipe.Core.Session.SessionSuite();
				TestSession.Suite = suite;
				TestSession.Suite.Browser = "IE";

				RunnerHelper.SetTestSessionDefaultBrowser();
				string[] tags = new string[] { "browser_Chrome" };
				IBrowser browser = RunnerHelper.GetBrowser(tags);
				BrowserTypeEnum actual = browser.BrowserType;

				browser.Quit();

				Assert.AreEqual(expected, actual);
			}
		}

		[TestClass]
		public class Ignore
		{
			[TestMethod]
			public void Ignore_Given_All_Matching_Should_Return_False()
			{
				//feature file tags
				string[] tags = { "all", "all" };

				//config file tags
				string runTags = "All";

				bool expected = false;

				bool actual = RunnerHelper.Ignore(tags, runTags);

				Assert.AreEqual(expected, actual);
			}

			[TestMethod]
			public void Ignore_Given_Empty_RunTags_Should_Return_False()
			{
				//feature file tags
				string[] tags = { "all", "all" };

				//config file tags
				string runTags = string.Empty;

				bool expected = false;

				bool actual = RunnerHelper.Ignore(tags, runTags);

				Assert.AreEqual(expected, actual);
			}

			[TestMethod]
			[ExpectedException(typeof(TestPipe.Core.Exceptions.IgnoreException), "Ignored")]
			public void Ignore_Given_Tags_Containing_Ignore_Should_Throw_Exception()
			{
				//feature file tags
				string[] tags = { "ignore" };

				//config file tags
				string runTags = "all";

				bool expected = true;

				bool actual = RunnerHelper.Ignore(tags, runTags);

				Assert.AreEqual(expected, actual);
			}

			[TestMethod]
			[ExpectedException(typeof(TestPipe.Core.Exceptions.IgnoreException), "Incomplete")]
			public void Ignore_Given_Tags_Containing_Incomplete_Should_Throw_Exception()
			{
				//feature file tags
				string[] tags = { "incomplete" };

				//config file tags
				string runTags = "all";

				bool expected = true;

				bool actual = RunnerHelper.Ignore(tags, runTags);

				Assert.AreEqual(expected, actual);
			}

			[TestMethod]
			[ExpectedException(typeof(TestPipe.Core.Exceptions.IgnoreException), "Manual")]
			public void Ignore_Given_Tags_Containing_Manual_Should_Throw_Exception()
			{
				//feature file tags
				string[] tags = { "manual" };

				//config file tags
				string runTags = "all";

				bool expected = true;

				bool actual = RunnerHelper.Ignore(tags, runTags);

				Assert.AreEqual(expected, actual);
			}

			[TestMethod]
			public void Ignore_Given_Tags_Containing_RunTags_Should_Return_True()
			{
				//feature file tags
				string[] tags = { "Hello", "world" };

				//config file tags
				string runTags = "hello";

				bool expected = false;

				bool actual = RunnerHelper.Ignore(tags, runTags);

				Assert.AreEqual(expected, actual);
			}

			[TestMethod]
			[ExpectedException(typeof(TestPipe.Core.Exceptions.IgnoreException), "Ignored tags is null.")]
			public void Ignore_Given_Tags_Null_Should_Return_True()
			{
				//feature file tags
				string[] tags = null;

				//config file tags
				string runTags = "all";

				bool expected = true;

				bool actual = RunnerHelper.Ignore(tags, runTags);

				Assert.AreEqual(expected, actual);
			}
		}

		[TestClass]
		public class LoadFeature
		{
			private const string HasFeature = "Has Feature";
			private const string NoFeature = "No Feature";

			[TestMethod]
			[ExpectedException(typeof(TestPipe.Core.Exceptions.TestPipeException), "TestSession.Suite.Features does not contain a feature with title" + NoFeature + "\"{0}\".")]
			public void LoadFeature_Given_Null_Feature_Should_Throw_Exception()
			{
				TestSession.Suite = TestSession.Suite ?? new SessionSuite();

				string title = NoFeature;

				RunnerHelper.LoadFeature(title);
			}

			[TestMethod]
			[ExpectedException(typeof(TestPipe.Core.Exceptions.TestPipeException), "Feature \"" + HasFeature + "\" has a null or empty Feature.Path.")]
			public void LoadFeature_Given_Null_Or_Empty_Feature_Path_Should_Throw_Exception()
			{
				TestSession.Suite = TestSession.Suite ?? new SessionSuite();
				SessionFeature feature = new SessionFeature();
				TestSession.Suite.Features.Add(feature);

				string title = HasFeature;

				RunnerHelper.LoadFeature(title);
			}

			[TestMethod]
			[ExpectedException(typeof(TestPipe.Core.Exceptions.TestPipeException), "Parameter \"title\" can not be null owr white space.")]
			public void LoadFeature_Given_Null_Or_Empty_Title_Should_Throw_Exception()
			{
				string title = string.Empty;

				RunnerHelper.LoadFeature(title);
			}
		}
	}
}