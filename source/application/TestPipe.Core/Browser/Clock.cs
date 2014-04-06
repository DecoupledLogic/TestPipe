namespace TestPipe.Core.Browser
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using TestPipe.Core.Interfaces;

	public class Clock : IClock
	{
		public DateTime Now
		{
			get { return DateTime.Now; }
		}

		public bool IsNowBefore(DateTime otherDateTime)
		{
			return this.Now < otherDateTime;
		}

		public DateTime LaterBy(TimeSpan delay)
		{
			return this.Now.Add(delay);
		}
	}
}