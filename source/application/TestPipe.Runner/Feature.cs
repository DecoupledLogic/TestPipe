namespace TestPipe.Runner
{
	using System;
	using System.Collections.Generic;

	public class Feature : Entity
	{
		public Feature()
		{
			this.After = new List<Scenario>();
			this.Before = new List<Scenario>();
			this.Scenarios = new List<Scenario>();
		}

		public ICollection<Scenario> After { get; set; }

		public ICollection<Scenario> Before { get; set; }

		public string Description { get; set; }

		public ICollection<Scenario> Scenarios { get; set; }
	}
}