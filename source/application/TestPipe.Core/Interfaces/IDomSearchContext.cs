namespace TestPipe.Core.Interfaces
{
	using System;
	using System.Collections.ObjectModel;

	public interface IDomSearchContext
	{
		IElement FindElement(ISelect by, uint timeoutInSeconds = 0, bool displayed = false);

		ReadOnlyCollection<IElement> FindElements(ISelect by, uint timeoutInSeconds = 0);

		ISelectElement SelectElement(ISelect by, uint timeoutInSeconds = 0, bool displayed = false);

        IListElement FindList(ISelect by, uint timeoutInSeconds = 0, bool displayed = false);

		IElement GetActiveElement();
	}
}