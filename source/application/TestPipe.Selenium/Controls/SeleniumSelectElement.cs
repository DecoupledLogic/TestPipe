namespace TestPipe.Selenium.Controls
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using OpenQA.Selenium;
	using OpenQA.Selenium.Support.UI;
	using TestPipe.Core.Exceptions;
	using TestPipe.Core.Interfaces;
	using TestPipe.Selenium.Browsers;
	using TestPipe.Selenium.Common;

	public class SeleniumSelectElement : SeleniumElement, ISelectElement
	{
		private ISeleniumSearchContext context;

		private SelectElement selectElement;

		public SeleniumSelectElement(ISeleniumSearchContext context, IWebElement element)
			: base(element)
		{
			if (element.TagName.ToLower() != "select")
			{
				throw new UnexpectedTagException();
			}

			this.context = context;
			this.selectElement = new SelectElement(element);
		}

		public ReadOnlyCollection<IElement> AllSelectedOptions
		{
			get
			{
				ReadOnlyCollection<IWebElement> selectElements = new ReadOnlyCollection<IWebElement>(this.selectElement.AllSelectedOptions);

				ReadOnlyCollection<IElement> elements = this.context.ToElements(selectElements);
				return elements;
			}
		}

		public IElement SelectedOption
		{
			get
			{
				IWebElement webElement = this.selectElement.SelectedOption;
				IElement element = this.context.ToElement(webElement);
				return element;
			}
		}

		public bool IsMultiple
		{
			get
			{
				return this.selectElement.IsMultiple;
			}
		}

		public ReadOnlyCollection<IElement> Options
		{
			get
			{
				ReadOnlyCollection<IWebElement> selectElements = new ReadOnlyCollection<IWebElement>(this.selectElement.Options);

				ReadOnlyCollection<IElement> elements = this.context.ToElements(selectElements);
				return elements;
			}
		}

		public void DeselectAll()
		{
			this.selectElement.DeselectAll();
		}

		public void DeselectByIndex(int index)
		{
			this.selectElement.DeselectByIndex(index);
		}

		public void DeselectByText(string text)
		{
			this.selectElement.DeselectByText(text);
		}

		public void DeselectByValue(string value)
		{
			this.selectElement.DeselectByValue(value);
		}

		public void SelectByIndex(int index)
		{
			this.selectElement.SelectByIndex(index);
		}

		public void SelectByText(string text)
		{
			this.selectElement.SelectByText(text);
		}

		public void SelectByValue(string value)
		{
			this.selectElement.SelectByValue(value);
		}
	}
}