namespace TestPipe.Assertions
{
	using System;
	using TestPipe.Core.Enums;

	public static class TestHelper
	{
		public static void EvaluateAssert(AssertResult result)
		{
			if (result == null)
			{
				return;
			}

			if (result.Exception == null)
			{
				return;
			}

			if (result.AssertStatus != AssertStatusEnum.Fail)
			{
				return;
			}

			throw result.Exception;
		}
	}
}