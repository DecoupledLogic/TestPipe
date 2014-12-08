namespace TestPipe.Core.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class Timing
	{
		public static bool Timeout(uint timeoutInSeconds, Func<bool> action)
		{
			if (action.Invoke())
			{
				return true;
			}

			int interval = 500;
			long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			long timeout = timeoutInSeconds * 1000;

			while (!action.Invoke())
			{
				System.Threading.Thread.Sleep(interval);

				long elapsedTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - startTime;

				if (elapsedTime > timeout)
				{
					return action.Invoke();
				}
			}

			return action.Invoke();
		}
	}
}
