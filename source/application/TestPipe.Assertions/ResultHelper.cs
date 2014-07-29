namespace TestPipe.Assertions
{
	using TestPipe.Core.Enums;
	using TestPipe.Core.Exceptions;
	using TestPipe.Core.Session;

	public static class ResultHelper
	{
		public static Result AreEqual(object expected, object actual)
		{
			Result result = new Result();

			try
			{
				Asserts.Equal(expected, actual, null);
				result.AssertStatus = AssertStatusEnum.Pass;
			}
			catch (AssertBombedException ex)
			{
				result.AssertStatus = AssertStatusEnum.Fail;
				result.Exception = ex;
			}

			return result;
		}

		public static Result Ignore()
		{
			Result result = new Result();

			try
			{
				Asserts.Ignored();
			}
			catch (IgnoreException ex)
			{
				result.AssertStatus = AssertStatusEnum.NotRan;
				result.Exception = ex;
			}

			return result;
		}

		public static Result Manual()
		{
			Result result = new Result();

			try
			{
				Asserts.Manual();
			}
			catch (ManualOnlyException ex)
			{
				result.AssertStatus = AssertStatusEnum.ManualOnly;
				result.Exception = ex;
			}

			return result;
		}
	}
}