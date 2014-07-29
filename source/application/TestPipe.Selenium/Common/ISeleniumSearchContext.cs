namespace TestPipe.Selenium.Common
{
	using System;
	using System.Collections.ObjectModel;
	using OpenQA.Selenium;
	using TestPipe.Core.Interfaces;

	public interface ISeleniumSearchContext : IDomSearchContext
	{
		IElement ToElement(IWebElement webElement);

		ReadOnlyCollection<IElement> ToElements(ReadOnlyCollection<IWebElement> webElements);
	}
}