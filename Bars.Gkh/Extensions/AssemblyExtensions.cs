namespace Bars.Gkh.Extensions
{
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Класс методов-расширений для работы со сборкой
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Метод возвращает дату сборки текущего экземпляра <see cref="Assembly"/>
        /// </summary>
        /// <param name="assembly">Сборка</param>
        /// <param name="target">Временная зона</param>
        /// <returns></returns>
        public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            const int PeHeaderOffset = 60;
            const int LinkerTimestampOffset = 8;

            var filePath = assembly.Location;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                stream.Read(buffer, 0, 2048);
            }

            var offset = BitConverter.ToInt32(buffer, PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }
    }
}