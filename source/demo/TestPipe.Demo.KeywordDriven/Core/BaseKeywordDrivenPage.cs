namespace TestPipe.Demo.KeywordDriven.Core
{
	using System;
	using OpenQA.Selenium;

	//This class provides base methods that are used to drive actions
	public class BaseKeywordDrivenPage
	{
		protected string BaseUrl = "http://localhost/testpipe.testsite/";
		protected IWebDriver Driver;
		protected string Url;

		public BaseKeywordDrivenPage(IWebDriver driver, string url)
		{
			this.Driver = driver;
			this.Url = url;
		}

		public void Click(string element)
		{
			IWebElement webElement = this.GetElement(element);
			webElement.Click();
		}

		public void EnterText(string element, string text)
		{
			IWebElement webElement = this.GetElement(element);
			webElement.Clear();
			webElement.SendKeys(text);
		}

		public virtual IWebElement GetElement(string element)
		{
			throw new ApplicationException(string.Format("Element, {0}, not found.", element));
		}

		public bool HasText(string element, string text)
		{
			IWebElement webElement = this.GetElement(element);
			return webElement.Text == text;
		}

		public bool IsOpen()
		{
			if (this.Driver.Url.IndexOf(this.Url) > -1)
			{
				return true;
			}

			return false;
		}

		public void Open()
		{
			this.Driver.Navigate().GoToUrl(this.BaseUrl + this.Url);
		}
	}
}