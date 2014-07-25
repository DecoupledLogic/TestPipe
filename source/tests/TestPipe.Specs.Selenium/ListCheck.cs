namespace TestPipe.Specs.Selenium
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestPipe.Core.Control;
    using TestPipe.Core.Enums;
    using TestPipe.Core.Interfaces;
    using TestPipe.Selenium.Browsers;

    [TestClass]
    public class ListCheck
    {
        [TestMethod]
        public void Check()
        {
            IBrowser browser = new Browser(BrowserTypeEnum.IE);
            browser.Open("http://localhost/testpipe.testsite/childrencheck.html");
            ISelect selector = new Select(FindByEnum.Id, "bubble");
            IListElement recipientSelector = browser.BrowserSearchContext.FindList(selector);
            List<IElement> elements = recipientSelector.GetList("breadcrumb").ToList();
            int count = elements.Count;
            foreach (IElement e in elements)
            {
                e.Click();
            }
        }
    }
}
