namespace TestPipe.Runner
{
	using System;
	using System.Collections.Generic;

	public class Suite : Entity
	{
		public Suite()
		{
			this.Features = new List<Feature>();
		}

		public ICollection<Feature> Features { get; set; }
	}
}