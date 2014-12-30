namespace TestPipe.SpecFlow.Specs
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestPipe.Core;
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
        [TestCategory("Slow")]
        public void OpenMultipleBrowserTypes()
        {
            TestSession.Suite = new Core.Session.SessionSuite();
            IBrowser browser1 = RunnerHelper.SetBrowser("IE");
            IBrowser browser2 = RunnerHelper.SetBrowser("FireFox");
            string url1 = "https://www.google.com";
            string url2 = "http://www.bing.com";

            browser1.Open(url1);
            browser2.Open(url2);

            Assert.IsTrue(browser1.Url.Contains(url1));
            Assert.IsTrue(browser2.Url.Contains(url2));

            browser1.Quit();
            browser2.Quit();
        }
    }
}