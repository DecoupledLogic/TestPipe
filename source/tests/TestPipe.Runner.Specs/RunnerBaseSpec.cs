namespace TestPipe.SpecFlow.Specs
{
	using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestPipe.Core;
using TestPipe.Core.Enums;
using TestPipe.Core.Interfaces;
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
		public void OpenMultipleBrowsers()
		{
			TestSession.Suite = new Core.Session.SessionSuite();
			IBrowser browser1 = RunnerHelper.SetBrowser("IE");
			IBrowser browser2 = RunnerHelper.SetBrowser("FireFox");

			browser2.Open("http://www.bing.com");
			browser1.Open("http://www.google.com");
		}
	}
}