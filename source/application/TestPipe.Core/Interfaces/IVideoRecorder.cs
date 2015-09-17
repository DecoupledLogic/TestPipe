namespace TestPipe.Core.Interfaces
{
    using System;

    public interface IVideoRecorder
    {
        bool IsStarted { get; set; }

        bool IsDebug { get; set; }

        string OutputDirectory { get; set; }

        void Start();

        void Stop();
    }
}