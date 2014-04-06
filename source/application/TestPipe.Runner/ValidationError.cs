namespace TestPipe.Runner
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class ValidationError
	{
		public ValidationError()
		{
		}

		public string Message { get; set; }

		public string PropertyName { get; set; }
	}
}