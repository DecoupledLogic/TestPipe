namespace TestPipe.Core.VideoRecorder
{
    using System;
    using Interfaces;

    public class FakeVideoRecorder : IVideoRecorder
    {
        public bool IsDebug { get; set; }

        public bool IsStarted { get; set; }

        public string OutputDirectory { get; set; }

        public void Start()
        {
            this.IsStarted = true;
        }

        public void Stop()
        {
            this.IsStarted = false;
        }
    }
}