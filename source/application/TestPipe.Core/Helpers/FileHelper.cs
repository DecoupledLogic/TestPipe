namespace TestPipe.Core.Helpers
{
    using System.IO;

    public static class FileHelper
    {
        public static string GetDirectory(string parent, string child)
        {
            if (string.IsNullOrWhiteSpace(parent))
            {
                return string.Empty;
            }

            string directory = FileHelper.GetDirectoryFromName(child);

            if (string.IsNullOrWhiteSpace(directory))
            {
                return string.Empty;
            }

            return Path.Combine(parent, directory);
        }

        public static string GetDirectoryFromName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            foreach (var c in System.IO.Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c.ToString(), string.Empty);
            }

            foreach (var c in System.IO.Path.GetInvalidPathChars())
            {
                name = name.Replace(c.ToString(), string.Empty);
            }

            return name.Replace(" ", "_").Trim();
        }
    }
}