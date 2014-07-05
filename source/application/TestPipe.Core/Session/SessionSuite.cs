namespace TestPipe.Core.Session
{
	using System;
	using System.Collections.Generic;

	public class SessionSuite
	{
		public static string KeyPrefix { get; set; }

		public static string SetupKeyPrefix { get; set; }

		public string ApplicationKey { get; set; }

		public string BaseUrl { get; set; }

		public string Browser { get; set; }

		public ICollection<DbConnection> DbConnections { get; set; }

		public string Environment { get; set; }

		public ICollection<SessionFeature> Features { get; set; }

		public string LoginUrl { get; set; }

		public string LogoutUrl { get; set; }

		public string Name { get; set; }

		public int PageTimeout { get; set; }

		public int Timeout { get; set; }

		public int WaitTime { get; set; }
	}
}