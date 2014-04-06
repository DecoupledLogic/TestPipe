namespace TestPipe.Core.Interfaces
{
	using System.Collections.ObjectModel;

	public interface IControlContext
	{
		IControl Find();

		IControl Find(ISelect by, uint timeout = 0, bool displayed = false);

		ReadOnlyCollection<IControl> FindAll(ISelect by, uint timeout = 0);

		IControl FindById(string id, uint timeout = 0, bool displayed = false);

		IControl FindByText(string text, uint timeout = 0, bool displayed = false);
	}
}