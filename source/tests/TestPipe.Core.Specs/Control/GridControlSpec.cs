namespace TestPipe.Specs.Control
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Core.Control;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;
	using TestPipe.Specs.Browser;

	[TestClass]
	public class GridControlSpec : BaseBrowserFixture
	{
		private GridControl sut;

		[TestMethod]
		public void FindAllShouldFindRowsByXPath()
		{
			this.BrowserInstance.Open(this.TableTestPage);

			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			ISelect rowSelector = new Select(FindByEnum.XPath, "//*[@id='Results']/tbody/tr");
			ReadOnlyCollection<IControl> controls = this.sut.FindAll(rowSelector);

			int expected = 3;

			Assert.AreEqual(expected, controls.Count);
		}

		[TestMethod]
		public void GetCellByColumnNameShouldReturnControlText()
		{
			string expected = "Row 1 Column A";
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			IControl control = this.sut.GetCellByColumnName(2, "Column A");
			string actual = control.Text;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetCellShouldReturnControlText()
		{
			string expected = "Row 1 Column A";
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			IControl control = this.sut.GetCell(2, 1);
			string actual = control.Text;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetColumnNumberShouldReturnColumnNumber()
		{
			int expected = 1;
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			int actual = this.sut.GetColumnNumber("Column A");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetColumnTextShouldReturnText()
		{
			ICollection<string> expected = this.ExpectedColumnText();
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			ICollection<string> actual = this.sut.GetColumnText(1);

			Assert.IsTrue(expected.SequenceEqual(actual));
		}

		[TestMethod]
		public void GetColumnTextByColumnNameShouldReturnText()
		{
			ICollection<string> expected = this.ExpectedColumnText();
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			ICollection<string> actual = this.sut.GetColumnTextByColumnName("Column A");

			Assert.IsTrue(expected.SequenceEqual(actual));
		}

		[TestMethod]
		public void GetRowTextShouldReturnText()
		{
			ICollection<string> expected = this.ExpectedRowText();
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			ICollection<string> actual = this.sut.GetRowText(2);

			Assert.IsTrue(expected.SequenceEqual(actual));
		}

		[TestMethod]
		public void GridControlShouldFindGridById()
		{
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);
			string expected = "Results";

			Assert.AreEqual(expected, this.sut.Id);
		}

		[TestMethod]
		public void GridControlShouldReturnColumnCount()
		{
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);
			int expected = 3;

			Assert.AreEqual(expected, this.sut.ColumnCount);
		}

		[TestMethod]
		public void GridControlShouldReturnRowCount()
		{
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);
			int expected = 3;

			Assert.AreEqual(expected, this.sut.RowCount);
		}

		[TestMethod]
		public void GetRowNumberByCellTextShouldReturnRowNumber()
		{
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);
			int expected = 3;

			int actual = this.sut.GetRowNumberByCellText(1, "Row 2 Column A");

			Assert.AreEqual(expected, this.sut.RowCount);
		}

		[TestMethod]
		public void GetRowNumberByColumnNameAndCellTextShouldReturnRowNumber()
		{
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);
			int expected = 3;

			int actual = this.sut.GetRowNumberByColumnNameAndCellText("Column A", "Row 2 Column A");

			Assert.AreEqual(expected, this.sut.RowCount);
		}

		[TestMethod]
		public void GetGetColumnHeaderShouldReturnHeader()
		{
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			IControl actual = this.sut.GetColumnHeader(1);

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		public void GetGetColumnHeaderByColumnNameShouldReturnHeader()
		{
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			IControl actual = this.sut.GetColumnHeaderByColumnName("Column A");

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		public void GetGetColumnHeaderWithAnchorShouldReturnHeader()
		{
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			IControl actual = this.sut.GetColumnHeader(1, ControlTypeEnum.Anchor);

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		public void GetGetColumnHeaderWithAnchorByColumnNameShouldReturnHeader()
		{
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			IControl actual = this.sut.GetColumnHeaderByColumnName("Column A", ControlTypeEnum.Anchor);

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		public void GetGetCellWithAnchorShouldReturnCell()
		{
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			IControl actual = this.sut.GetCell(2, 1, ControlTypeEnum.Anchor);

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		public void GetGetCellByColumnNameWithAnchorByColumnNameShouldReturnCell()
		{
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			IControl actual = this.sut.GetCellByColumnName(2, "Column A", ControlTypeEnum.Anchor);

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		public void GetGetCellByColumnNameAndCellTextWithAnchorByColumnNameShouldReturnCell()
		{
			this.BrowserInstance.Open(this.TableTestPage);
			ISelect selector = new Select(FindByEnum.Id, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);

			IControl actual = this.sut.GetCellByColumnNameAndCellText("Column A", "Row 1 Column A", ControlTypeEnum.Anchor);

			Assert.IsTrue(actual.Exists());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "A selector or id string must be provided to create grid controls.")]
		public void GridControlWithNullSelectorAndNullIdShouldThrowException()
		{
			this.BrowserInstance.Open(this.XhtmlTestPage);
			this.sut = new GridControl(this.BrowserInstance, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "If an id string is not provided, the selector find by argument must be FindByEnum.Id to create grid controls.")]
		public void GridControlWithSelectorWithNotFindByIdShouldThrowException()
		{
			this.BrowserInstance.Open(this.XhtmlTestPage);
			ISelect selector = new Select(FindByEnum.ClassName, "Results");
			this.sut = new GridControl(this.BrowserInstance, selector);
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