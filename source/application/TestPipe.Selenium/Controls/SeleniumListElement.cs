namespace TestPipe.Selenium.Controls
{
	using System;
	using System.Collections.ObjectModel;
	using OpenQA.Selenium;
	using TestPipe.Core.Exceptions;
	using TestPipe.Core.Interfaces;
	using TestPipe.Selenium.Browsers;
	using TestPipe.Selenium.Common;

	public class SeleniumListElement : SeleniumElement, IListElement
	{
		private readonly IWebElement element;
		private ISeleniumSearchContext context;

		public SeleniumListElement(ISeleniumSearchContext context, IWebElement element)
			: base(element)
		{
			if (element.TagName.ToLower() != "ul" && element.TagName.ToLower() != "div" && element.TagName.ToLower() != "input")
			{
				throw new UnexpectedTagException();
			}

			this.context = context;
			this.element = element;
		}

		public ReadOnlyCollection<IElement> GetList(string selectedElement)
		{
			ReadOnlyCollection<IElement> elements = null;

			if (selectedElement.ToLower() == "radio")
			{
				string xpathRadio = "//input[@type='radio']";
				elements = this.context.ToElements(this.element.FindElements(By.XPath(xpathRadio)));
			}
			else if (selectedElement.ToLower() == "checkbox")
			{
				string xpathUlLiCheckbox = "//li/input[@type='checkbox']";
				elements = this.context.ToElements(this.element.FindElements(By.XPath(xpathUlLiCheckbox)));
				if (elements.Count != 0)
				{
					return elements;
				}
				string xpathDivCheckbox = ".//input[@type='checkbox']";
				elements = this.context.ToElements(this.element.FindElements(By.XPath(xpathDivCheckbox)));
				if (elements.Count != 0)
				{
					return elements;
				}
			}
			else if (selectedElement.ToLower() == "breadcrumb")
			{
				string xpathUlLiAnchor = ".//ul/li/a";
				elements = this.context.ToElements(this.element.FindElements(By.XPath(xpathUlLiAnchor)));
			}

			return elements;
		}
	}
}