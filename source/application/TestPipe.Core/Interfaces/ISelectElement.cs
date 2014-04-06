namespace TestPipe.Core.Interfaces
{
	using System;
	using System.Collections.ObjectModel;
	using TestPipe.Core.Interfaces;

	public interface ISelectElement : IElement
	{
		ReadOnlyCollection<IElement> AllSelectedOptions { get; }

		bool IsMultiple { get; }

		ReadOnlyCollection<IElement> Options { get; }

		IElement SelectedOption { get; }

		void DeselectAll();

		void DeselectByIndex(int index);

		void DeselectByText(string text);

		void DeselectByValue(string value);

		void SelectByIndex(int index);

		void SelectByText(string text);

		void SelectByValue(string value);
	}
}