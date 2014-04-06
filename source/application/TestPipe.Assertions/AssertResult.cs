namespace TestPipe.Assertions
{
    using TestPipe.Core.Enums;
    using TestPipe.Core.Exceptions;

    public class AssertResult
    {
        public AssertStatusEnum AssertStatus { get; set; }

        public AssertBombedException Exception { get; set; }
    }
}