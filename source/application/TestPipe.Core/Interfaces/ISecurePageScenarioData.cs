namespace TestPipe.Core.Interfaces
{
    public interface ISecurePageScenarioData : IPageScenarioData
    {
        string Password { get; }

        string UserName { get; }
    }
}