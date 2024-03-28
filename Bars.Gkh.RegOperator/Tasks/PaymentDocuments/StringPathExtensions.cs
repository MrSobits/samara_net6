namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments
{
    using System.IO;
    using System.Linq;

    internal static class StringPathExtension
    {
        public static string GetCorrectPath(this string path)
        {
            var invalidChars = Path.GetInvalidFileNameChars().Union(Path.GetInvalidPathChars()).Union(new[] { '«', '»' });
            var result = invalidChars.Aggregate(path, (current, invalidChar) => current.Replace(invalidChar, '_'));
            return result;
        }

        public static string GetCorrectPath(this string tmpPath, string filename)
        {
            var invalidChars = Path.GetInvalidFileNameChars().Union(Path.GetInvalidPathChars());

            filename = invalidChars.Aggregate(filename, (current, invalidChar) => current.Replace(invalidChar, '_'));

            var pathLength = 255 - tmpPath.Length;
            if (filename.Length > pathLength)
            {
                var extension = Path.GetExtension(filename);
                filename = filename.Substring(0, pathLength - extension.Length) + extension;
            }

            var path = Path.Combine(tmpPath, filename);

            return path;
        }
    }
}