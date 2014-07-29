namespace TestPipe.Analytics.Domain
{
	using System;
	using System.Collections.Generic;

	public class TestMetric
	{
		public int AssertsCount { get; set; }

		public ICollection<TestMetric> ChildTestMetrics { get; set; }

		public string Description { get; set; }

		public double Duration { get; set; }

		public bool IsExecuted { get; set; }

		public bool IsSuccess { get; set; }

		public string Message { get; set; }

		public string MetricName { get; set; }

		public Guid? ParentTestMetricId { get; set; }

		public TestMetricResult Result { get; set; }

		public string StackTrace { get; set; }

		public ICollection<TestMetricCategory> TestMetricCategories { get; set; }

		public Guid TestMetricId { get; set; }

		public TestMetricType TestMetricType { get; set; }
	}
}