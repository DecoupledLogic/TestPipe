namespace TestPipe.Core.Interfaces
{
	public interface IControlAssert
	{
		bool Displayed { get; }

		bool Enabled { get; }

		bool Selected { get; }

		bool CanFind(ISelect by, uint timeoutInSeconds = 0);

		bool CanFindById(string id, uint timeoutInSeconds = 0);

		bool CanFindByText(string text, uint timeoutInSeconds = 0);

		bool Exists(uint timeoutInSeconds = 0);

		bool HasFocus(uint timeoutInSeconds = 0);

		bool IsDisplayed(uint timeoutInSeconds = 0);

		bool IsEnabled(uint timeoutInSeconds = 0);

		bool IsSelected(uint timeoutInSeconds = 0);
	}
}