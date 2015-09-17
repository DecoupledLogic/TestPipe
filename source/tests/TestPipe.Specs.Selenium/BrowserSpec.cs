using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TestPipe.Core.Control;
using TestPipe.Core.Enums;
using TestPipe.Core.Interfaces;
using TestPipe.Selenium.Browsers;

namespace TestPipe.Specs.Selenium
{
    [TestClass]
    public class BrowserSpec
    {
        [TestMethod]
        public void CanHoverElement()
        {
            IBrowser browser = new Browser(BrowserTypeEnum.Chrome);

            browser.Open("http://www.google.com");

            ISelect selector = new Select(FindByEnum.Id, "gs_st0");
            BaseControl show = new BaseControl(browser, selector);

            show.Hover();

            ISelect bodySelector = new Select(FindByEnum.TagName, "body");
            BaseControl body = new BaseControl(browser, bodySelector);

            Assert.IsTrue(body.Text.Contains("Search by voice"));

            browser.Quit();
        }

        [TestMethod]
        [TestCategory("Slow")]
        public void ClickWaitsForControlToDisplay()
        {
            IBrowser browser = new Browser(BrowserTypeEnum.IE);

            browser.Open("http://localhost/testpipe.testsite/javascriptPage.html");

            ISelect selector = new Select(FindByEnum.Id, "clickToShow");
            BaseControl show = new BaseControl(browser, selector);

            show.Click();

            ISelect selector2 = new Select(FindByEnum.Id, "clickToHide", 10);

            //User the browser to find control "clickToHide" and wait 10 seconds for it to display (true).
            BaseControl hide = new BaseControl(browser, null, "clickToHide", 10, true);
            string result = hide.GetCssValue("display");

            browser.Quit();
            Assert.IsTrue(result == "block", result);
        }

        [TestMethod]
        [Ignore]
        public void CreateRemoteBrowser()
        {
            //System.Environment.SetEnvironmentVariable("webdriver.chrome.driver", @"C:\_TestPipe\packages\WebDriver.ChromeDriver.win32.2.12.0.0\content\chromedriver.exe");
            DesiredCapabilities capabilities = OpenQA.Selenium.Remote.DesiredCapabilities.Chrome();
            //capabilities.SetCapability(CapabilityType.Platform, "WINDOWS");
            //capabilities.SetCapability(CapabilityType.BrowserName, "internet explorer");
            //@"C:\_TestPipe\packages\WebDriver.IEDriver.2.29.1.1\tools\IEDriverServer.exe"
            Uri u = new Uri("http://127.0.0.1:4444/wd/hub");
            //IWebDriver webdriver = new RemoteWebDriver(u, capabilities, new TimeSpan(0,0,59));
            IWebDriver driver = new RemoteWebDriver(u, DesiredCapabilities.Chrome());
        }
    }
}