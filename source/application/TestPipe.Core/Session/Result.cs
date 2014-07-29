namespace TestPipe.Core.Session
{
	using TestPipe.Core.Enums;
	using TestPipe.Core.Exceptions;

	public class Result
	{
		public AssertStatusEnum AssertStatus { get; set; }

		public TestPipeException Exception { get; set; }
	}
}