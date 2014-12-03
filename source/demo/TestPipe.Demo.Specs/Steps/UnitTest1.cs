using System;
using TechTalk.SpecFlow;
using TestPipe.Assertions;
using TestPipe.Core;
using TestPipe.Core.Page;
using TestPipe.Core.Session;
using TestPipe.Demo.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestPipe.Demo.Steps
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void Open2Browsers()
		{
			TestSession.Environment = new TestEnvironment() { Title = "d1", BaseUrl = "https://www.google.com/" };
			TestSession.DefaultBrowser = Core.Enums.BrowserTypeEnum.IE;
			TestSession.Suite = new SessionSuite();
			TestSession.Suite.BaseUrl = "https://www.google.com/";
			TestSession.Suite.Timeout = 60;
			SessionFeature feature1 = new SessionFeature() { Title = "Demo Search" };
			SessionScenario sessionScenario1 = new SessionScenario() { Id = "1" };
			SessionScenario sessionScenario2 = new SessionScenario() { Id = "2" };
			feature1.Scenarios.Add(sessionScenario1);
			feature1.Scenarios.Add(sessionScenario2);
			TestSession.Features = new List<SessionFeature>();
			TestSession.Features.Add(feature1);
			
			string[] tags = new string[]{};
			SessionScenario scenario1 = StepHelper.SetupScenario(tags, "Demo Search","1. Search for Something");
			BasePage testPage = new SearchPage(scenario1.Browser, TestSession.Environment);

			SessionScenario scenario2 = StepHelper.SetupScenario(tags, "Demo Search", "2. Search for Something");
			BasePage testPage2 = new SearchPage(scenario2.Browser, TestSession.Environment);

			testPage.Open();
			testPage2.Open();

			Assert.IsTrue(testPage.IsOpen());
			Assert.IsTrue(testPage2.IsOpen());

			testPage.Browser.Quit();
			testPage2.Browser.Quit();
		}
	}
}
