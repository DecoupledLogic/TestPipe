namespace TestPipe.Core.Session
{
	using System;
	using System.Collections.Generic;

	public class Feature
	{
		public string KeyPrefix { get; set; }

		public string Id { get; set; }

		public string Path { get; set; }

		public ICollection<Scenario> Scenarios { get; set; }

		public string Title { get; set; }

		public Interfaces.IBrowser Browser { get; set; }
	}
}