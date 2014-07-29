using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TestPipe.Core.Control;
using TestPipe.Core.Enums;
using TestPipe.Core.Interfaces;
using TestPipe.Selenium.Browsers;
using TestPipe.Selenium.Controls;

namespace TestPipe.Specs.Selenium.Controls
{
	[TestClass]
	public class SeleniumSelectElementSpecs
	{
		private Browser browser;

		[TestInitialize]
		public void TestSetup()
		{ 
			this.browser = new Browser(BrowserTypeEnum.IE);
		}

		[TestCleanup]
		public void TestTeardown()
		{
			browser.Quit();
		}
		
		[TestMethod]
		public void AllSelectedOptionsReturnsSelectedOptions()
		{
			SeleniumSelectElement sut = this.GetSut("multi");

			ReadOnlyCollection<IElement> selectedOptions = sut.AllSelectedOptions;
			int actual = selectedOptions.Count;

			Assert.IsTrue(actual == 2);
		}

		[TestMethod]
		public void IdentifiesMultiSelect()
		{
			SeleniumSelectElement sut = this.GetSut("multi");

			bool actual = sut.IsMultiple;

			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void OptionsReturnsOptions()
		{
			SeleniumSelectElement sut = this.GetSut("selectomatic");

			ReadOnlyCollection<IElement> options = sut.Options;

			bool actual = true;
			string expected = "one, two, four, still learning how to count, apparently";

			foreach (var element in options)
			{
				if (!expected.Contains(element.Text.ToLower()))
				{
					actual = false;
				}
			}

			Assert.IsTrue(options.Count == 4);

			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void SelectedOptionReturnsSelectedOption()
		{
			SeleniumSelectElement sut = this.GetSut("selectomatic");

			IElement selectedOption = sut.SelectedOption;
			string actual = selectedOption.Text;

			Assert.IsTrue(actual == "One");
		}

		[TestMethod]
		public void DeselectAllDeselectsAllOptions()
		{
			SeleniumSelectElement sut = this.GetSut("multi");
			int expected = 0; //Expect zero selected options

			sut.DeselectAll();

			int actual = sut.AllSelectedOptions.Count;

			Assert.AreEqual(expected, actual);
		}
		
		[TestMethod]
		public void DeselectByIndexDeselectsOption()
		{
			SeleniumSelectElement sut = this.GetSut("multi");
			bool expected = false;
			int index = 2;

			Assert.AreEqual(true, sut.Options[index].Selected);

			sut.DeselectByIndex(index);

			bool actual = sut.Options[index].Selected;

			Assert.AreEqual(expected, actual);
		}
		
		[TestMethod]
		public void DeselectByTextDeselectsOptions()
		{
			SeleniumSelectElement sut = this.GetSut("multi");
			bool expected = false;
			string text = "Sausages";
			IElement element = sut.Options.Where(x => x.Text == text).FirstOrDefault();

			Assert.AreEqual(true, element.Selected);

			sut.DeselectByText(text);

			bool actual = sut.Options.Where(x => x.Text == text).FirstOrDefault().Selected;

			Assert.AreEqual(expected, actual);
		}
		
		[TestMethod]
		public void DeselectByValueDeselectsOptions()
		{
			SeleniumSelectElement sut = this.GetSut("multi");
			bool expected = false;
			string text = "sausages";
			IElement element = sut.Options.Where(x => x.GetAttribute("value") == text).FirstOrDefault();

			Assert.AreEqual(true, element.Selected);

			sut.DeselectByValue(text);

			bool actual = sut.Options.Where(x => x.GetAttribute("value") == text).FirstOrDefault().Selected;

			Assert.AreEqual(expected, actual);
		}
		
		[TestMethod]
		public void SelectByIndexSelectsOptions()
		{
			SeleniumSelectElement sut = this.GetSut("multi");
			bool expected = true;
			int index = 1;

			Assert.AreEqual(false, sut.Options[index].Selected);

			sut.SelectByIndex(index);

			bool actual = sut.Options[index].Selected;

			Assert.AreEqual(expected, actual);
		}
		
		[TestMethod]
		public void SelectByTextSelectsOptions()
		{
			SeleniumSelectElement sut = this.GetSut("multi");
			bool expected = true;
			string text = "Ham";
			IElement element = sut.Options.Where(x => x.Text == text).FirstOrDefault();

			Assert.AreEqual(false, element.Selected);

			sut.SelectByText(text);

			bool actual = sut.Options.Where(x => x.Text == text).FirstOrDefault().Selected;

			Assert.AreEqual(expected, actual);
		}
		
		[TestMethod]
		public void SelectByValueSelectsOptions()
		{
			SeleniumSelectElement sut = this.GetSut("multi");
			bool expected = true;
			string text = "ham";
			IElement element = sut.Options.Where(x => x.GetAttribute("value") == text).FirstOrDefault();

			Assert.AreEqual(false, element.Selected);

			sut.SelectByValue(text);

			bool actual = sut.Options.Where(x => x.GetAttribute("value") == text).FirstOrDefault().Selected;

			Assert.AreEqual(expected, actual);
		}

		public SeleniumSelectElement GetSut(string id)
		{
			browser.Open("http://localhost/testpipe.testsite/formpage.html");
			IWebDriver driver = (IWebDriver)browser.Driver;
			SeleniumBrowserSearchContext context = new SeleniumBrowserSearchContext(browser, driver);
			IWebElement element = driver.FindElement(By.Id(id));
			SeleniumSelectElement sut = new SeleniumSelectElement(context, element);
			return sut;
		}
	}
}
