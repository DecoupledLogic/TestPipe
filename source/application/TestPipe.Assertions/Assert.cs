namespace TestPipe.Assertions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.VisualStudio.QualityTools.UnitTestFramework;
	using TestPipe.Core.Exceptions;

	public static class Asserts
	{
		public static void Ignored()
		{
			throw new IgnoreException();
		}
	}
}
