namespace TestPipe.Core.VideoRecorder
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using TestPipe.Common;
    using TestPipe.Core.Interfaces;

    public class VideoRecorderFactory : LoggedObject
    {
        public VideoRecorderFactory(ILogManager log)
            : base(log)
        {
        }

        public static IVideoRecorder Create(ILogManager log)
        {
            VideoRecorderFactory factory = new VideoRecorderFactory(log);
            return factory.Compose();
        }

        private IVideoRecorder Compose()
        {
            IVideoRecorder recorder = new FakeVideoRecorder();

            string path = ConfigurationManager.AppSettings["videorecorder.plugins"];

            if (string.IsNullOrWhiteSpace(path))
            {
                return recorder;
            }

            DirectoryInfo directory = new DirectoryInfo(path);
            string fileName = directory.GetFiles("*.dll", SearchOption.TopDirectoryOnly).Select(x => x.Name).FirstOrDefault();

            if (string.IsNullOrEmpty(fileName))
            {
                return recorder;
            }

            Assembly assembly = Assembly.LoadFrom(Path.Combine(path, fileName));

            if (assembly == null)
            {
                return recorder;
            }

            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (!typeof(IVideoRecorder).IsAssignableFrom(type) || type.IsInterface)
                {
                    continue;
                }

                recorder = assembly.CreateInstance(type.FullName) as IVideoRecorder;

                if (recorder != null)
                {
                    break;
                }

                recorder = new FakeVideoRecorder();
            }

            return recorder;
        }
    }
}