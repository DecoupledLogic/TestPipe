namespace TestPipe.Selenium.Controls
{
	using System;
	using TestPipe.Core.Control;
	using TestPipe.Core.Interfaces;

	public class SeleniumControl : BaseControl
	{
		public SeleniumControl(IBrowser browser)
			: base(browser)
		{
		}

		public SeleniumControl(IBrowser browser, ISelect selector = null, string id = null, uint timeoutInSeconds = 0, bool displayed = false)
			: base(browser, selector, id, timeoutInSeconds, displayed)
		{
		}
	}
}