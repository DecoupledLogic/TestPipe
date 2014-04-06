namespace TestPipe.Selenium.Controls
{
	using System;
	using System.Drawing;
	using OpenQA.Selenium;
	using TestPipe.Core.Interfaces;

	public class SeleniumElement : IElement, IEquatable<SeleniumElement>
	{
		public SeleniumElement(IWebElement webElement)
		{
			this.NativeElement = webElement;
		}

		public bool Displayed
		{
			get
			{
				return this.NativeElement.Displayed;
			}
		}

		public bool Enabled
		{
			get
			{
				return this.NativeElement.Enabled;
			}
		}

		public Point Location
		{
			get
			{
				return this.NativeElement.Location;
			}
		}

		public dynamic NativeElement
		{
			get;
			private set;
		}

		public bool Selected
		{
			get
			{
				return this.NativeElement.Selected;
			}
		}

		public Size Size
		{
			get
			{
				return this.NativeElement.Size;
			}
		}

		public string TagName
		{
			get
			{
				return this.NativeElement.TagName;
			}
		}

		public string Text
		{
			get
			{
				return this.NativeElement.Text;
			}
		}

		public void Clear()
		{
			this.NativeElement.Clear();
		}

		public void Click()
		{
			this.NativeElement.Click();
		}

		public override bool Equals(object obj)
		{
			if (obj is SeleniumElement)
			{
				return this.Equals((SeleniumElement)obj);
			}
			return false;
		}

		public bool Equals(SeleniumElement p)
		{
			return this.NativeElement.Equals(p.NativeElement);
		}

		public string GetAttribute(string attributeName)
		{
			return this.NativeElement.GetAttribute(attributeName);
		}

		public string GetCssValue(string propertyName)
		{
			return this.NativeElement.GetCssValue(propertyName);
		}

		public override int GetHashCode()
		{
			return this.NativeElement.GetHashCode();
		}

		public void SendKeys(string text)
		{
			this.NativeElement.SendKeys(text);
		}

		public void Submit()
		{
			this.NativeElement.Submit();
		}
	}
}