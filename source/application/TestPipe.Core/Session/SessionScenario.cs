namespace TestPipe.Core.Session
{
    using System;
    using TestPipe.Core.Interfaces;

    public class SessionScenario
    {
        public IAsserts Asserts { get; set; }
        public IBrowser Browser { get; set; }
        public dynamic Data { get; set; }
        public string Id { get; set; }
        public string KeyPrefix { get; set; }
        public Result Result { get; set; }
        public string[] Tags { get; set; }
        public IVideoRecorder VideoRecorder { get; set; }
    }
}