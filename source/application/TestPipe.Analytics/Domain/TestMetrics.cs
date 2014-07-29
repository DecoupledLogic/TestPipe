namespace TestPipe.Analytics.Domain
{
	using System;
	using System.Collections.Generic;

	public class TestMetrics
	{
		public Guid BuildId { get; set; }

		public DateTime DateRan { get; set; }

		public string Name { get; set; }

		public TestRunner Runner { get; set; }

		public ICollection<TestMetric> TestRunMetrics { get; set; }

		public Guid TestRunMetricId { get; set; }

		public int TotalErrors { get; set; }

		public int TotalFailures { get; set; }

		public int TotalIgnored { get; set; }

		public int TotalInconclusive { get; set; }

		public int TotalInvalid { get; set; }

		public int TotalNotRun { get; set; }

		public int TotalRan { get; set; }

		public int TotalSkipped { get; set; }
	}
}