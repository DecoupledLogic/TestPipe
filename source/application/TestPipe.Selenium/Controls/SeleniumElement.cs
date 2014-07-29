namespace TestPipe.Selenium.Controls
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using OpenQA.Selenium;
	using TestPipe.Core.Exceptions;
	using TestPipe.Core.Interfaces;
	using TestPipe.Selenium.Common;

	public class SeleniumElement : IElement, IEquatable<SeleniumElement>, ISeleniumSearchContext
	{
		public SeleniumElement(IWebElement webElement)
		{
			this.NativeElement = webElement;
		}

		public bool Displayed
		{
			get
			{
				return this.NativeElement.Displayed;
			}
		}

		public bool Enabled
		{
			get
			{
				return this.NativeElement.Enabled;
			}
		}

		public Point Location
		{
			get
			{
				return this.NativeElement.Location;
			}
		}

		public dynamic NativeElement
		{
			get;
			private set;
		}

		public bool Selected
		{
			get
			{
				return this.NativeElement.Selected;
			}
		}

		public Size Size
		{
			get
			{
				return this.NativeElement.Size;
			}
		}

		public string TagName
		{
			get
			{
				return this.NativeElement.TagName;
			}
		}

		public string Text
		{
			get
			{
				return this.NativeElement.Text;
			}
		}

		public void Clear()
		{
			this.NativeElement.Clear();
		}

		public void Click()
		{
			this.NativeElement.Click();
		}

		public override bool Equals(object obj)
		{
			if (obj is SeleniumElement)
			{
				return this.Equals((SeleniumElement)obj);
			}
			return false;
		}

		public bool Equals(SeleniumElement p)
		{
			return this.NativeElement.Equals(p.NativeElement);
		}

		public string GetAttribute(string attributeName)
		{
			return this.NativeElement.GetAttribute(attributeName);
		}

		public string GetCssValue(string propertyName)
		{
			return this.NativeElement.GetCssValue(propertyName);
		}

		public override int GetHashCode()
		{
			return this.NativeElement.GetHashCode();
		}

		public void SendKeys(string text)
		{
			this.NativeElement.SendKeys(text);
		}

		public void Submit()
		{
			this.NativeElement.Submit();
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

			IWebElement parent = (IWebElement)this.NativeElement;
			ReadOnlyCollection<IWebElement> webElements = parent.FindElements(seleniumBy);
			ReadOnlyCollection<IElement> elements = this.ToElements(webElements);
			return elements;
		}

		public IElement GetActiveElement()
		{
			IWebElement webElement = this.NativeElement.SwitchTo().ActiveElement();
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
			IWebElement parent = (IWebElement)this.NativeElement;
			try
			{
				element = parent.FindElement(seleniumBy, timeoutInSeconds, displayed);
			}
			catch (NoSuchElementException)
			{
				throw new ElementNotFoundException();
			}

			return element;
		}
	}
}