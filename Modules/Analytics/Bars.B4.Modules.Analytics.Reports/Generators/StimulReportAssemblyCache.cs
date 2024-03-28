namespace Bars.B4.Modules.Analytics.Reports.Generators
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Application;
    using B4.Utils;
    using B4.Utils.Annotations;

    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Analytics.Reports.Domain;

    using Microsoft.Extensions.Logging;

    using MD5 = System.Security.Cryptography.MD5;

    internal static class StimulReportAssemblyCache
    {
        private static readonly object SyncRoot = new object();

        private static readonly Dictionary<string, Assembly> Storage = new Dictionary<string, Assembly>();

        private static ILogger Log
        {
            get { return ApplicationContext.Current.Container.Resolve<ILogger>(); }
        }

        public static bool Contains(Stream stream, out string hash)
        {
            ArgumentChecker.NotNull(stream, "stream");

            hash = ComputeHash(stream);

            return Storage.ContainsKey(hash);
        }

        public static Assembly Get(string hash)
        {
            ArgumentChecker.NotNull(hash, "hash");

            if (!Storage.ContainsKey(hash))
            {
                throw new ArgumentOutOfRangeException(string.Format("The given key {0} does not present in dictionary", hash));
            }

            return Storage[hash];
        }

        public static void Add(Stream stream)
        {
            ArgumentChecker.NotNull(stream, "stream");

            var hash = ComputeHash(stream);

            try
            {
                lock (SyncRoot)
                {
                    var container = ApplicationContext.Current.Container;
                    var remoteService = container.Resolve<IRemoteReportService>();
                    
                    using (container.Using(remoteService))
                    {
                        var report = new CustomReport(
                            Array.Empty<DataSource>(),
                            Array.Empty<IParam>(),
                            nameof(StimulReportAssemblyCache),
                            hash,
                            stream
                        );

                        remoteService.SaveOrUpdateTemplate(report, stream.ReadAllBytes());
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogError(e, e.Message);
                Storage.Remove(hash);
                throw;
            }

            Log.LogDebug("Добавлена новая сборка в кеш сборок отчетов");
        }

        public static void Clear()
        {
            Storage.Clear();
        }

        private static string ComputeHash(Stream stream)
        {
            using (var algo = MD5.Create())
            {
                var hash = algo.ComputeHash(stream).AsBase64String();

                stream.Seek(0, SeekOrigin.Begin);

                return hash;
            }
        }
    }
}