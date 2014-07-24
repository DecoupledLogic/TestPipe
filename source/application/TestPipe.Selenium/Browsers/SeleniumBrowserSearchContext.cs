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

	public class SeleniumBrowserSearchContext : IBrowserSearchContext
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
			By seleniumBy = GetSeleniumBy(by);
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

		internal IElement ToElement(IWebElement webElement)
		{
			SeleniumElement element = new SeleniumElement(webElement);
			return element;
		}

		internal ReadOnlyCollection<IElement> ToElements(ReadOnlyCollection<IWebElement> webElements)
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

		private static By GetSeleniumBy(ISelect by)
		{
			By seleniumBy = null;

			switch (by.FindBy)
			{
				case FindByEnum.Id:
					{
						seleniumBy = By.Id(by.EqualTo);
						break;
					}
				case FindByEnum.ClassName:
					{
						seleniumBy = By.ClassName(by.EqualTo);
						break;
					}
				case FindByEnum.CssSelector:
					{
						seleniumBy = By.CssSelector(by.EqualTo);
						break;
					}
				case FindByEnum.LinkText:
					{
						seleniumBy = By.LinkText(by.EqualTo);
						break;
					}
				case FindByEnum.Name:
					{
						seleniumBy = By.Name(by.EqualTo);
						break;
					}
				case FindByEnum.PartialLinkText:
					{
						seleniumBy = By.PartialLinkText(by.EqualTo);
						break;
					}
				case FindByEnum.TagName:
					{
						seleniumBy = By.TagName(by.EqualTo);
						break;
					}
				case FindByEnum.XPath:
					{
						seleniumBy = By.XPath(by.EqualTo);
						break;
					}
				default:
					{
						throw new ArgumentException("Invalid Find By");
					}
			}
			return seleniumBy;
		}

		private IWebElement GetWebElement(ISelect by, uint timeoutInSeconds, bool displayed)
		{
			By seleniumBy = GetSeleniumBy(by);
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