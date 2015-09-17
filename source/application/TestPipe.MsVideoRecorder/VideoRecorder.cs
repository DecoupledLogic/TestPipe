namespace TestPipe.MsVideoRecorder
{
    using System;
    using Microsoft.Expression.Encoder.ScreenCapture;
    using TestPipe.Core.Interfaces;

    public class VideoRecorder : IVideoRecorder
    {
        private ScreenCaptureJob recorder;

        public VideoRecorder()
        {
            this.recorder = new ScreenCaptureJob();
        }

        public bool IsDebug { get; set; }

        public bool IsStarted { get; set; }

        public string OutputDirectory
        {
            get
            {
                return this.recorder.OutputPath;
            }
            set
            {
                this.recorder.OutputPath = value;
            }
        }

        public void Start()
        {
            this.recorder.Start();
            this.IsStarted = true;
        }

        public void Stop()
        {
            this.recorder.Stop();
            this.IsStarted = false;
        }
    }
}