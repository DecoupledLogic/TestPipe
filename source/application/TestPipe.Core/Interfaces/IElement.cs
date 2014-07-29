namespace TestPipe.Core.Interfaces
{
	using System;
	using System.Drawing;

	public interface IElement : IDomSearchContext
	{
		bool Displayed { get; }

		bool Enabled { get; }

		Point Location { get; }

		dynamic NativeElement { get; }

		bool Selected { get; }

		Size Size { get; }

		string TagName { get; }

		string Text { get; }

		void Clear();

		void Click();

		string GetAttribute(string attributeName);

		string GetCssValue(string propertyName);

		void SendKeys(string text);

		void Submit();
	}
}