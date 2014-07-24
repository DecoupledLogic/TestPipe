namespace TestPipe.Selenium.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using TestPipe.Core.Exceptions;
    using TestPipe.Core.Interfaces;
    using TestPipe.Selenium.Browsers;

    public class SeleniumListElement : SeleniumElement, IListElement
    {
        private readonly IWebElement element;
        private SeleniumBrowserSearchContext context;
        
        public SeleniumListElement(SeleniumBrowserSearchContext context, IWebElement element)
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
            //get
           // {
                /* Future work : if the element has anchor tag and checkboxes then it will only take checkboxes need to fix this. */
                //string xpathUlLiCheckbox = "//li/input[@type='checkbox']";
                //string xpathDivCheckbox = ".//input[@type='checkbox']";
                //string xpathUlLiAnchor = ".//ul/li/a";
                ReadOnlyCollection<IElement> elements = null;
                if(selectedElement.ToLower() == "radio")
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
                //string xpathUlLiCheckbox = "//li/input[@type='checkbox']";
                //string xpathDivCheckbox = ".//input[@type='checkbox']";
                //string xpathUlLiAnchor = ".//ul/li/a";
                //string xpathRadio = "//input[@type='radio']";
                //ReadOnlyCollection<IElement> elements = this.context.ToElements(this.element.FindElements(By.XPath(xpathUlLiCheckbox)));
                //if (elements.Count != 0) { return elements; }
                //elements = this.context.ToElements(this.element.FindElements(By.XPath(xpathDivCheckbox)));
                //if (elements.Count != 0) { return elements; }
                //elements = this.context.ToElements(this.element.FindElements(By.XPath(xpathUlLiAnchor)));
                //if (elements.Count != 0) { return elements; }
                //elements = this.context.ToElements(this.element.FindElements(By.XPath(xpathRadio)));
                //if (elements.Count != 0) { return elements; }
                //return null;
           // }
        }
    }
}
