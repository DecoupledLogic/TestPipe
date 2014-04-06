namespace TestPipe.Selenium.Common
{
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;
	using OpenQA.Selenium;
	using OpenQA.Selenium.Support.UI;

	public enum ReadyStateEnum
	{
		unknown,
		Loading,
		Interactive,
		Complete,
		Loaded
	}

	public static class WebDriverExtensions
	{
		private static readonly string JsReadyState = @"return document.readyState";

		public static IWebElement FindElement(this ISearchContext context, By by, uint timeoutInSections = 0, bool displayed = false)
		{
			if (timeoutInSections > 0)
			{
				var wait = new DefaultWait<ISearchContext>(context);
				wait.Timeout = TimeSpan.FromSeconds(timeoutInSections);
				wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
				return wait.Until(ctx =>
				{
					var elem = ctx.FindElement(by);
					if (displayed && !elem.Displayed)
						return null;

					return elem;
				});
			}

			return context.FindElement(by);
		}

		public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, uint timeoutInSeconds)
		{
			if (timeoutInSeconds > 0)
			{
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
				return wait.Until(drv => (drv.FindElements(by).Count > 0) ? drv.FindElements(by) : null);
			}
			return driver.FindElements(by);
		}

		public static void WaitForPageLoad(this IWebDriver driver, uint timeoutInSeconds)
		{
			string state = string.Empty;
			try
			{
				WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

				//Checks every 500 ms whether predicate returns true if returns exit otherwise keep trying till it returns ture
				wait.Until(d =>
				{
					try
					{
						state = ((IJavaScriptExecutor)driver).ExecuteScript(JsReadyState).ToString();
					}
					catch (NoSuchWindowException)
					{
						//when popup is closed, switch to last windows
						driver.SwitchTo().Window(driver.WindowHandles.Last());
					}
					catch (InvalidOperationException)
					{
						//Ignore
					}
					catch (WebDriverException)
					{
						//If we catch a webdriverexception (could be an issue communicating with the driver) we want to wait, 
						//but we don't want to throw a TimeoutException as it would swallow this so we will try to rethrow the error in the TimeoutException catch.
					}

					return IsValidDocumentReadyState(state);
				});
			}
			catch (TimeoutException)
			{
				//Sometimes Page remains in Interactive mode and never becomes Complete, then we can still try to access the controls
				//First we try to aquire state again just incase we swallowed a WebDriverException that is still occuring.
				state = ((IJavaScriptExecutor)driver).ExecuteScript(JsReadyState).ToString();

				if (IsValidInteractiveState(state))
				{
					return;
				}
				
				throw;
			}
			catch (NullReferenceException)
			{
				//Sometimes Page remains in Interactive mode and never becomes Complete, then we can still try to access the controls
				if (IsValidInteractiveState(state))
				{
					return;
				}

				throw;
			}
			catch (WebDriverException)
			{
				if (driver != null && driver.WindowHandles.Count == 1)
				{
					driver.SwitchTo().Window(driver.WindowHandles[0]);
				}

				state = ((IJavaScriptExecutor)driver).ExecuteScript(JsReadyState).ToString();
				
				if (IsValidDocumentReadyState(state))
				{
					return;
				}
				
				throw;
			}
		}

		private static bool IsValidInteractiveState(string state)
		{
			if (state.Equals(ReadyStateEnum.Interactive.ToString(), StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}

			return false;
		}

		private static bool IsValidDocumentReadyState(string state)
		{
			if (state.Equals(ReadyStateEnum.Complete.ToString(), StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}

			//In IE7 there are chances we may get state as loaded instead of complete
			if (state.Equals(ReadyStateEnum.Loaded.ToString(), StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}

			return false;
		}
	}
}