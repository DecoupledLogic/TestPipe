namespace TestPipe.Core.Session
{
	using System;
	using System.Collections.Generic;

	public class DataObject
	{
		public string Database { get; set; }

		public string Name { get; set; }

		public string Namespace { get; set; }

		public ICollection<IDictionary<string, dynamic>> Properties { get; set; }

		public bool Seed { get; set; }

		public string Type { get; set; }
	}
}