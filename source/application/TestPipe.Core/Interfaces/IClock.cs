namespace TestPipe.Core.Interfaces
{
	using System;

	public interface IClock
	{
		DateTime Now { get; }

		bool IsNowBefore(DateTime otherDateTime);

		DateTime LaterBy(TimeSpan delay);
	}
}