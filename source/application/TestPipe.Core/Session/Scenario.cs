namespace TestPipe.Core.Session
{
	using System;
	using System.Collections.Generic;

	public class Scenario
	{
		public string KeyPrefix { get; set; }

		public ICollection<KeyValue> Data { get; set; }

		public string Id { get; set; }

		public ICollection<DataObject> Objects { get; set; }

		public Interfaces.IBrowser Browser { get; set; }
	}
}