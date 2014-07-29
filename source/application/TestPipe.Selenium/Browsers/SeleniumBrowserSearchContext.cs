namespace TestPipe.Selenium.Browsers
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using OpenQA.Selenium;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Exceptions;
	using TestPipe.Core.Interfaces;
	using TestPipe.Selenium.Common;
	using TestPipe.Selenium.Controls;

	public class SeleniumBrowserSearchContext : ISeleniumSearchContext
	{
		private IBrowser browser;

		private IWebDriver driver;

		public SeleniumBrowserSearchContext(IBrowser browser, IWebDriver driver)
		{
			this.browser = browser;
			this.driver = driver;
		}

		public IElement FindElement(ISelect by, uint timeoutInSeconds = 0, bool displayed = false)
		{
			IWebElement element = this.GetWebElement(by, timeoutInSeconds, displayed);

			return this.ToElement(element);
		}

		public IListElement FindList(ISelect by, uint timeoutInSeconds = 0, bool displayed = false)
		{
			IWebElement element = this.GetWebElement(by, timeoutInSeconds, displayed);

			return new SeleniumListElement(this, element);
		}

		public ReadOnlyCollection<IElement> FindElements(ISelect by, uint timeoutInSeconds = 0)
		{
			By seleniumBy = SeleniumHelper.GetSeleniumBy(by);
			ReadOnlyCollection<IWebElement> webElements = this.driver.FindElements(seleniumBy, timeoutInSeconds);
			ReadOnlyCollection<IElement> elements = this.ToElements(webElements);
			return elements;
		}

		public IElement GetActiveElement()
		{
			IWebElement webElement = this.driver.SwitchTo().ActiveElement();
			IElement element = this.ToElement(webElement);
			return element;
		}

		public ISelectElement SelectElement(ISelect by, uint timeoutInSeconds = 0, bool displayed = false)
		{
			IWebElement element = this.GetWebElement(by, timeoutInSeconds, displayed);

			return new SeleniumSelectElement(this, element);
		}

		public IElement ToElement(IWebElement webElement)
		{
			SeleniumElement element = new SeleniumElement(webElement);
			return element;
		}

		public ReadOnlyCollection<IElement> ToElements(ReadOnlyCollection<IWebElement> webElements)
		{
			IList<IElement> elementList = new List<IElement>();

			foreach (var item in webElements)
			{
				IElement element = this.ToElement(item);
				elementList.Add(element);
			}

			ReadOnlyCollection<IElement> elements = new ReadOnlyCollection<IElement>(elementList);
			return elements;
		}

		private IWebElement GetWebElement(ISelect by, uint timeoutInSeconds, bool displayed)
		{
			By seleniumBy = SeleniumHelper.GetSeleniumBy(by);
			IWebElement element;

			try
			{
				element = this.driver.FindElement(seleniumBy, timeoutInSeconds, displayed);
			}
			catch (NoSuchElementException)
			{
				throw new ElementNotFoundException();
			}
			return element;
		}
	}
}