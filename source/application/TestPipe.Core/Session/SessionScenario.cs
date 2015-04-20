namespace TestPipe.Core.Session
{
	using System;
	using System.Collections.Generic;
	using TestPipe.Core.Interfaces;

	public class SessionScenario
	{
		public string KeyPrefix { get; set; }

		public dynamic Data { get; set; }

		public string Id { get; set; }

		public IBrowser Browser { get; set; }

        public IAsserts Asserts { get; set; }
	}
}