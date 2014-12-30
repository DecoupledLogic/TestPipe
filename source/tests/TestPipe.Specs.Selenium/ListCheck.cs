namespace TestPipe.Specs.Selenium
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestPipe.Core.Control;
    using TestPipe.Core.Enums;
    using TestPipe.Core.Interfaces;
    using TestPipe.Selenium.Browsers;

    [TestClass]
    public class ListCheck
    {
        private IBrowser browser = null;
        private GridControl gridControlColumns = null;
        private List<IControl> li = new List<IControl>();

        public GridControl AllColumnsControl
        {
            get
            {
                if (this.gridControlColumns == null)
                {
                    ISelect selector = new Select(FindByEnum.Id, "test");
                    this.gridControlColumns = new GridControl(this.browser, selector);
                }

                return this.gridControlColumns;
            }
        }

        [TestMethod]
        [TestCategory("Slow")]
        public void Check()
        {
            ISelect selector = new Select(FindByEnum.Id, "bubble");
            IListElement recipientSelector = browser.BrowserSearchContext.FindList(selector);
            List<IElement> elements = recipientSelector.GetList("breadcrumb").ToList();
            int count = elements.Count;

            foreach (IElement e in elements)
            {
                e.Click();
            }

            browser.Quit();
        }

        [TestMethod]
        [TestCategory("Slow")]
        public void ColumnsInTable()
        {
            this.li = AllColumnsControl.GetSelectedColumn(0);

            browser.Quit();
        }

        [TestInitialize]
        public void Setup()
        {
            this.browser = new Browser(BrowserTypeEnum.IE);
            this.browser.Open("http://localhost/testpipe.testsite/childrencheck.html");
        }
    }
}