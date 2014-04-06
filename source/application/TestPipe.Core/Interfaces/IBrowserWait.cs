namespace TestPipe.Core.Interfaces
{
	using System;

	public interface IBrowserWait
	{
	}

	public class BrowserWait
	{
		public BrowserWait(IBrowser browser, TimeSpan timeout)
		{
		}

		public BrowserWait(IClock clock, IBrowser browser, TimeSpan timeout, TimeSpan sleepInterval)
		{
		}
	}
}