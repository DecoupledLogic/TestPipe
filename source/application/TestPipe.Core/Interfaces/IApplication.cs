namespace TestPipe.Core.Interfaces
{
    public interface IApplication
    {
        string Title { get; }

        System.Collections.Generic.ICollection<TestEnvironment> Environments { get; }
    }
}