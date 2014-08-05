namespace TestPipe.Specs.Control
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Configuration;
	using System.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Common;
	using TestPipe.Core.Browser;
	using TestPipe.Core.Control;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;
    using TestPipe.Selenium.Browsers;
	using TestPipe.Specs.Browser;
	
	[TestClass]
	public class GridControlSpec
	{
		private GridControl sut;
		private IBrowser browserInstance;

		private string RootPath
		{
			get
			{
				return ConfigurationManager.AppSettings["htmlsite"];
			}
		}

		private string TableTestPage
		{
			get
			{
				return this.RootPath + "tables.html";
			}
		}

		private string XhtmlTestPage
		{
			get
			{
				return this.RootPath + "xhtmlTest.html";
			}
		}

		[TestInitialize]
		public void SetUp()
		{
			ILogManager log = new Logger();
			this.browserInstance = new Browser(BrowserTypeEnum.IE); 
		}

		[TestCleanup]
		public void TearDown()
		{
			this.browserInstance.Quit();
		}

		[TestMethod]
		public void FindAllShouldFindRowsByXPath()
		{
			this.browserInstance.Open(this.TableTestPage);

			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			ISelect rowSelector = new Select(FindByEnum.XPath, "//*[@id='Results']/tbody/tr");
			ReadOnlyCollection<IControl> controls = this.sut.FindAll(rowSelector);

			int expected = 3;

			Assert.AreEqual(expected, controls.Count);
		}

		[TestMethod]
		public void GetCellByColumnNameShouldReturnControlText()
		{
			string expected = "Row 1 Column A";
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			IControl control = this.sut.GetCellByColumnName(2, "Column A");
			string actual = control.Text;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetCellShouldReturnControlText()
		{
			string expected = "Row 1 Column A";
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			IControl control = this.sut.GetCell(2, 1);
			string actual = control.Text;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetColumnNumberShouldReturnColumnNumber()
		{
			int expected = 1;
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			int actual = this.sut.GetColumnNumber("Column A");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetColumnTextShouldReturnText()
		{
			ICollection<string> expected = this.ExpectedColumnText();
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			ICollection<string> actual = this.sut.GetColumnText(1);

			Assert.IsTrue(expected.SequenceEqual(actual));
		}

		[TestMethod]
		public void GetColumnTextByColumnNameShouldReturnText()
		{
			ICollection<string> expected = this.ExpectedColumnText();
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			ICollection<string> actual = this.sut.GetColumnTextByColumnName("Column A");

			Assert.IsTrue(expected.SequenceEqual(actual));
		}

		[TestMethod]
		public void GetRowTextShouldReturnText()
		{
			ICollection<string> expected = this.ExpectedRowText();
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			ICollection<string> actual = this.sut.GetRowText(2);

			Assert.IsTrue(expected.SequenceEqual(actual));
		}

		[TestMethod]
		public void GridControlShouldFindGridById()
		{
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);
			string expected = "Results";

			Assert.AreEqual(expected, this.sut.Id);
		}

		[TestMethod]
		public void GridControlShouldReturnColumnCount()
		{
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);
			int expected = 3;

			Assert.AreEqual(expected, this.sut.ColumnCount);
		}

		[TestMethod]
		public void GridControlShouldReturnRowCount()
		{
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);
			int expected = 3;

			Assert.AreEqual(expected, this.sut.RowCount);
		}

		[TestMethod]
		public void GetRowNumberByCellTextShouldReturnRowNumber()
		{
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);
			int expected = 3;

			int actual = this.sut.GetRowNumberByCellText(1, "Row 2 Column A");

			Assert.AreEqual(expected, this.sut.RowCount);
		}

		[TestMethod]
		public void GetRowNumberByColumnNameAndCellTextShouldReturnRowNumber()
		{
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);
			int expected = 3;

			int actual = this.sut.GetRowNumberByColumnNameAndCellText("Column A", "Row 2 Column A");

			Assert.AreEqual(expected, this.sut.RowCount);
		}

		[TestMethod]
		public void GetGetColumnHeaderShouldReturnHeader()
		{
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			IControl actual = this.sut.GetColumnHeader(1);

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		public void GetGetColumnHeaderByColumnNameShouldReturnHeader()
		{
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			IControl actual = this.sut.GetColumnHeaderByColumnName("Column A");

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		public void GetGetColumnHeaderWithAnchorShouldReturnHeader()
		{
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			IControl actual = this.sut.GetColumnHeader(1, ControlTypeEnum.Anchor);

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		public void GetGetColumnHeaderWithAnchorByColumnNameShouldReturnHeader()
		{
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			IControl actual = this.sut.GetColumnHeaderByColumnName("Column A", ControlTypeEnum.Anchor);

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		public void GetGetCellWithAnchorShouldReturnCell()
		{
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			IControl actual = this.sut.GetCell(2, 1, ControlTypeEnum.Anchor);

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		public void GetGetCellByColumnNameWithAnchorByColumnNameShouldReturnCell()
		{
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			IControl actual = this.sut.GetCellByColumnName(2, "Column A", ControlTypeEnum.Anchor);

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		public void GetGetCellByColumnNameAndCellTextWithAnchorByColumnNameShouldReturnCell()
		{
			this.browserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.browserInstance, selector);

			IControl actual = this.sut.GetCellByColumnNameAndCellText("Column A", "Row 1 Column A", ControlTypeEnum.Anchor);

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "A selector or id string must be provided to create grid controls.")]
		public void GridControlWithNullSelectorAndNullIdShouldThrowException()
		{
			this.browserInstance.Open(this.XhtmlTestPage);
			this.sut = new GridControl(this.browserInstance, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "If an id string is not provided, the selector find by argument must be FindByEnum.Id to create grid controls.")]
		public void GridControlWithSelectorWithNotFindByIdShouldThrowException()
		{
			this.browserInstance.Open(this.XhtmlTestPage);
			ISelect selector = new Select(FindByEnum.ClassName, "Results");
			this.sut = new GridControl(this.browserInstance, selector);
		}

        private ICollection<string> ExpectedColumnText()
		{
			ICollection<string> list = new List<string>();
			list.Add("Row 1 Column A");
			list.Add("Row 2 Column A");
			return list;
		}

		private ICollection<string> ExpectedRowText()
		{
			ICollection<string> list = new List<string>();
			list.Add("Row 1 Column A");
			list.Add("Row 1 Column B");
			list.Add("Row 1 Column C");
			return list;
		}
	}
}