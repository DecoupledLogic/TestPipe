namespace TestPipe.Runner
{
	using System;
	using System.Collections.Generic;

	public class Application : Entity
	{
		public Application()
		{
			this.Environments = new List<Environment>();
			this.Suites = new List<Suite>();
		}

		public ICollection<Environment> Environments { get; set; }

		public ICollection<Suite> Suites { get; set; }
	}
}