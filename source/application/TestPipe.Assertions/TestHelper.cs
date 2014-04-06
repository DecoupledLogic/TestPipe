namespace TestPipe.Assertions
{
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

            throw result.Exception;
        }
    }
}