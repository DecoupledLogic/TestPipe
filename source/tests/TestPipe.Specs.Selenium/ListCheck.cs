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
        IBrowser browser = null;
        List<IControl> li = new List<IControl>();

        [TestInitialize]
        public void setup()
        {
            this.browser = new Browser(BrowserTypeEnum.IE);
            this.browser.Open("http://localhost/testpipe.testsite/childrencheck.html");
        }
        private GridControl gridControlColumns = null;

        public GridControl allColumnsControl
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
        public void ColumnsInTable()
        {
            this.li = allColumnsControl.GetSelectedColumn(0);
        }

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

        /*private void GetSelectedColumn(int columnNumber, GridControl allColumnsControl)
        {
            ReadOnlyCollection<IControl> columnsControl;
            List<IControl> columnControl = new List<IControl>();
            columnsControl = allColumnsControl.ColumnHeaders;
            int numOfRows = allColumnsControl.RowCount;
            int numOfColums = allColumnsControl.ColumnCount / allColumnsControl.RowCount;
            columnsControl.ToArray<IControl>();
            for (int i = 0; i < numOfRows; i++)
            {
                columnControl.Add(columnsControl[(i * numOfColums) + columnNumber]);
            }
        }*/
    }
}
