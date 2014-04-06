namespace TestPipe.Core.Interfaces
{
	using TestPipe.Core.Enums;

	public interface ISelect
	{
		bool Displayed { get; }

		string EqualTo { get; }

		FindByEnum FindBy { get; }

		uint Timeout { get; }

		bool Equals(object obj);

		int GetHashCode();

		string ToString();
	}
}