namespace TestPipe.Core.Control
{
	using System;
	using System.Text.RegularExpressions;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;

	public class CheckBox : BaseControl
	{
		public CheckBox()
			: base()
		{ 
		}

		public CheckBox(IBrowser browser, IElement element)
			: base(browser, null, null)
		{
		}

		public CheckBox(IBrowser browser, ISelect selector = null, string id = null, uint timeoutInSeconds = 0, bool displayed = false)
			: base(browser, selector, id, timeoutInSeconds, displayed)
		{
		}

		public void Check()
		{
			if (!this.Element.Selected)
			{
				this.Element.Click();
			}
		}

		public void Uncheck()
		{
			if (this.Element.Selected)
			{
				this.Element.Click();
			}
		}

        public bool IsChecked()
        {
            return this.Element.Selected;
        }
    }
}