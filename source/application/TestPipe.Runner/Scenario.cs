namespace TestPipe.Runner
{
	using System;
	using System.Collections.Generic;

	public class Scenario : Entity
	{
		public Scenario()
		{
			this.Steps = new List<Step>();
		}

		public ICollection<Step> Steps { get; set; }
	}
}