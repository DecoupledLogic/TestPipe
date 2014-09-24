namespace TestPipe.Runner
{
	using System;
	using System.Collections.Generic;

	public class Step : Entity
	{
		public string Action { get; set; }

		public string Control { get; set; }

		public int Index { get; set; }

		public string Input { get; set; }

		public ICollection<Step> Steps { get; set; }

		public string Validation { get; set; }

		public uint Wait { get; set; }

        public string Result { get; set; }

        public DateTime Time { get; set; }

	}
}