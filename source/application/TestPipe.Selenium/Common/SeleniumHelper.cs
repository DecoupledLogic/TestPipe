namespace TestPipe.Selenium.Common
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using OpenQA.Selenium;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;

	public class SeleniumHelper
	{
		public static By GetSeleniumBy(ISelect by)
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
	}
}
