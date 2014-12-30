using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestPipe.Assertions;
using TestPipe.Core.Control;
using TestPipe.Core.Enums;
using TestPipe.Core.Exceptions;
using TestPipe.Core.Interfaces;
using TestPipe.Selenium.Browsers;

namespace TestPipe.Specs.Selenium.Controls
{
	[TestClass]
	public class SeleniumElementTest
	{
		private IBrowser browser;

		[TestInitialize]
		public void TestSetup()
		{ 
			browser = new Browser(BrowserTypeEnum.IE);
		}

		[TestCleanup]
		public void TestTeardown()
		{
			browser.Quit();
		}

		[TestMethod]
        [TestCategory("Slow")]
		public void FindChild_Can_Find_Child_Control()
		{
			string id = "test_id";
			IControl parent = GetParent("test_id_div");
			ISelect selector = new Select(FindByEnum.Id, id);

			IControl child = parent.FindChild(selector);

			Asserts.IsTrue(child.Id == id);
		}

        [TestMethod]
        [TestCategory("Slow")]
		public void FindChild_Does_Not_Find_Control_Out_Of_Context()
		{
			string id = "2";
			IControl parent = GetParent("test_id_div");
			ISelect selector = new Select(FindByEnum.Id, id);

			IControl child = parent.FindChild(selector);
			
			Asserts.Null(child.Element);
		}

        [TestMethod]
        [TestCategory("Slow")]
		public void Find_Can_Find_Control()
		{
			browser.Open("http://localhost/testpipe.testsite/nestedElements.html");
			string id = "2";
			ISelect selector = new Select(FindByEnum.Id, id);

			IControl control = new BaseControl(browser, selector);

			Asserts.IsTrue(control.Id == id);
		}

        [TestMethod]
        [TestCategory("Slow")]
		public void FindAllChildren_Can_Find_Child_Controls()
		{
			string id = "a";
			IControl parent = GetParent("div_2_anch");
			ISelect selector = new Select(FindByEnum.TagName, id);

			ReadOnlyCollection<IControl> child = parent.FindAllChildren(selector);

			Asserts.IsTrue(child.Count == 2);
		}

        [TestMethod]
        [TestCategory("Slow")]
		public void FindAllChildren_Can_Find_Child_Controls_In_Grid()
		{
			browser.Open("http://localhost/testpipe.testsite/tables.html");
			ISelect selector = new Select(FindByEnum.Id, "Results2");
			GridControl grid = new GridControl(browser, selector);
			
			string id = "a";

			var parent = grid.GetCell(2,1);
			ISelect selector2 = new Select(FindByEnum.TagName, id);

			ReadOnlyCollection<IControl> child = parent.FindAllChildren(selector2);

			Asserts.IsTrue(child.Count == 1);
			Asserts.Equal<string>(child[0].TagName, id);
			Asserts.Equal<string>(child[0].Text, "Row 1 Column A");
		}

        [TestMethod]
        [TestCategory("Slow")]
		public void FindAllChildren_Does_Not_Find_Controls_Out_Of_Context()
		{
			string id = "select";
			IControl parent = GetParent("div_2_anch");
			ISelect selector = new Select(FindByEnum.Id, id);

			ReadOnlyCollection<IControl> child = parent.FindAllChildren(selector);

			Asserts.Null(child);
		}

        [TestMethod]
        [TestCategory("Slow")]
		public void FindAll_Can_Find_Controls()
		{
			browser.Open("http://localhost/testpipe.testsite/nestedElements.html");
			string id = "select";
			ISelect selector = new Select(FindByEnum.TagName, id);

			IControl control = new BaseControl(browser, new Select(FindByEnum.TagName, "body"));
			ReadOnlyCollection<IControl> child = control.FindAll(selector);

			Asserts.Equal<int>(12, child.Count);
		}

		public IControl GetParent(string id)
		{ 
			browser.Open("http://localhost/testpipe.testsite/nestedElements.html");

			ISelect selector = new Select(FindByEnum.Id, id);
			IControl control = new BaseControl(browser, selector);

			return control;
		}
	}
}
