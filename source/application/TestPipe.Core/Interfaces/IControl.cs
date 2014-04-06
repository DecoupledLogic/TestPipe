namespace TestPipe.Core.Interfaces
{
	using System;
	using System.Drawing;
	using TestPipe.Core.Enums;

	public interface IControl : IControlContext, IControlAction, IControlAssert
	{
		string Class { get; }

		ControlTypeEnum ControlType { get; }

		IElement Element { get; }

		string Id { get; }

		Point Location { get; }

		Size Size { get; }

		string TagName { get; }

		string Text { get; }

		string GetAttribute(string attributeName);

		string GetCssValue(string propertyName);
	}
}