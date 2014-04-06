namespace TestPipe.Selenium.Browsers
{
	using System;
	using TestPipe.Core.Interfaces;

	public class SeleniumBrowserWait : IBrowserWait
	{
			public SeleniumBrowserWait(IBrowser browser, TimeSpan timeout)
			{
			}

			public SeleniumBrowserWait(IClock clock, IBrowser browser, TimeSpan timeout, TimeSpan sleepInterval)
			{
			}
	}
}