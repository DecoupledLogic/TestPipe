using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
	}
}
