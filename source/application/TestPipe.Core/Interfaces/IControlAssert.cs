namespace TestPipe.Core.Interfaces
{
	public interface IControlAssert
	{
		bool Displayed { get; }

		bool Enabled { get; }

		bool Selected { get; }

		bool CanFind(ISelect by);

		bool CanFindById(string id);

		bool CanFindByText(string text);

		bool Exists();
	}
}