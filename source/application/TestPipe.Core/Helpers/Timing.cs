namespace TestPipe.Core.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
    using TestPipe.Core.Exceptions;

	public class Timing
	{
		public static bool TimeoutPredicate(uint timeoutInSeconds, Func<bool> predicate)
		{
			if (predicate.Invoke())
			{
				return true;
			}

			int interval = 500;
			long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			long timeout = timeoutInSeconds * 1000;

			while (!predicate.Invoke())
			{
				System.Threading.Thread.Sleep(interval);

				long elapsedTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - startTime;

				if (elapsedTime > timeout)
				{
					bool lastChance = predicate.Invoke();
                    if (!lastChance)
                        throw new TestPipeTimeOutException("Timeout Predicate Timed Out");
				}
			}
			return predicate.Invoke();
		}
	}
}
