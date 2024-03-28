namespace Bars.GisIntegration.Base.Package.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Web;

    using Bars.B4;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.B4.Logging;
    using Bars.GisIntegration.Base.B4Events;

    using Castle.Windsor;

    /// <summary>
    /// Менеджер временных пакетов
    /// </summary>
    public class TempPackageManager<TPackageInfo> : PackageManagerBase<TPackageInfo, Guid>
        where TPackageInfo : ITempPackageInfo, new()
    {
        private readonly IWindsorContainer container;

        private readonly IDictionary<string, IDictionary<Guid, PackageHolder>> packages;

        public TempPackageManager(IWindsorContainer container, IEventAggregator eventAggregator)
        {
            this.container = container;
            this.packages = new ConcurrentDictionary<string, IDictionary<Guid, PackageHolder>>();

            eventAggregator.GetEvent<SessionEndEvent>().Subscribe(e => this.PruneSessionData(e.SessionId));
            eventAggregator.GetEvent<SessionStartEvent>().Subscribe(e => this.InitSessionData(e.SessionId));
        }

        /// <summary>
        /// Сохранить подписанные данные
        /// </summary>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signedData">Подписанные данные</param>
        public override void SaveSignedData(Guid packageId, string signedData)
        {
            var holder = this.GetHolder(packageId);

            lock (holder)
            {
                holder.SignedData = this.SaveTempFile(Encoding.UTF8.GetBytes(signedData));
                holder.Package.Signed = true;
            }
        }

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <param name="formatted">Признак - вернуть форматированную/неформатированную xml строку</param>
        /// <returns>Данные xml строкой</returns>
        public override string GetData(Guid packageId, bool signed, bool formatted)
        {
            var holder = this.GetHolder(packageId);

            lock (holder)
            {
                var data = File.ReadAllBytes(signed ? holder.SignedData : holder.Data);
                return formatted ? this.GetFormattedXmlDataString(data) : this.GetXmlDataString(data);
            }
        }

        /// <summary>
        /// Создать пакет
        /// </summary>
        /// <param name="packageName">Имя пакета</param>
        /// <param name="notSignedData">Неподписанные данные</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Описание пакета</returns>
        public override TPackageInfo CreatePackage(
            string packageName,
            object notSignedData,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary = null)
        {
            var sessionId = this.GetSessionId();
            var sessDict = this.GetOrCreateSessDict(sessionId);

            lock (sessDict)
            {
                var identity = this.container.Resolve<IUserIdentity>();
                using (this.container.Using(identity))
                {
                    var package = new TPackageInfo
                                      {
                                          Name = packageName,
                                          UserName = identity.Name,
                                          SessionId = sessionId,
                                          PackageId = Guid.NewGuid()
                                      };
                    var holder = new PackageHolder
                                     {
                                         Package = package,
                                         Data = this.SaveTempFile(this.XmlToBytes(this.SerializeRequest(notSignedData))),
                                         TransportGuidDictionary = transportGuidDictionary
                                     };

                    sessDict.Add(package.PackageId, holder);

                    return package;
                }
            }
        }

        private string SaveTempFile(byte[] data)
        {
            var path = Path.GetTempFileName();
            File.WriteAllBytes(path, data);

            return path;
        }

        /// <summary>
        /// Сохранить неподписанные данные
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="notSignedData">Неподписанные данные</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        public override void SaveNotSignedData(
            TPackageInfo packageInfo,
            object notSignedData,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary = null)
        {
            var holder = this.GetHolder(packageInfo.PackageId);

            lock (holder)
            {
                File.Delete(holder.Data);
                holder.Data = this.SaveTempFile(this.XmlToBytes(this.SerializeRequest(notSignedData)));
                holder.TransportGuidDictionary = transportGuidDictionary;
            }
        }

        /// <summary>
        /// Сохранить подписанные данные
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="signedData">Подписанные данные</param>
        public override void SaveSignedData(TPackageInfo packageInfo, string signedData)
        {
            this.SaveSignedData(packageInfo.PackageId, signedData);
            packageInfo.Signed = true;
        }

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <param name="formatted">Признак - вернуть форматированную/неформатированную xml строку</param>
        /// <returns>Данные xml строкой</returns>
        public override string GetData(TPackageInfo packageInfo, bool signed, bool formatted)
        {
            return this.GetData(packageInfo.PackageId, signed, formatted);
        }

        /// <summary>
        /// Получить словать транспортных идентификаторов
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <returns>Словарь транспортных идентификаторов</returns>
        public override Dictionary<Type, Dictionary<string, long>> GetTransportGuidDictionary(TPackageInfo packageInfo)
        {
            var holder = this.GetHolder(packageInfo.PackageId);

            lock (holder)
            {
                return holder.TransportGuidDictionary;
            }
        }

        private void InitSessionData(string sessionId)
        {
            this.GetOrCreateSessDict(sessionId);
        }

        private void PruneSessionData(string sessionId)
        {
            if (this.packages.ContainsKey(sessionId))
            {
                lock (this.packages)
                {
                    IDictionary<Guid, PackageHolder> sessDict;
                    if (this.packages.TryGetValue(sessionId, out sessDict))
                    {
                        lock (sessDict)
                        {
                            foreach (var sessItem in sessDict)
                            {
                                var holder = sessItem.Value;
                                lock (holder)
                                {
                                    this.SafeDeleteFile(holder.Data);
                                    this.SafeDeleteFile(holder.SignedData);
                                }
                            }

                            this.packages.Remove(sessionId);
                        }
                    }
                }
            }
        }

        private void SafeDeleteFile(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private string GetSessionId()
        {
            if (HttpContext.Current == null)
            {
                throw new InvalidOperationException("Невозможно определить идентификатор сессии. HttpContext не представлен в текущем окружении");
            }

            return HttpContext.Current.Session.SessionID;
        }

        private IDictionary<Guid, PackageHolder> GetOrCreateSessDict(string sessionId = null)
        {
            var sid = sessionId ?? this.GetSessionId();

            IDictionary<Guid, PackageHolder> sessDict;
            if (!this.packages.TryGetValue(sid, out sessDict))
            {
                lock (this.packages)
                {
                    if (!this.packages.TryGetValue(sid, out sessDict))
                    {
                        this.packages[sid] = sessDict = new Dictionary<Guid, PackageHolder>();
                    }
                }
            }

            return sessDict;
        }

        private PackageHolder GetHolder(Guid packageId)
        {
            var sessionId = this.GetSessionId();

            var sessDict = this.GetOrCreateSessDict();

            lock (sessDict)
            {
                PackageHolder holder;
                if (!sessDict.TryGetValue(packageId, out holder))
                {
                    throw new InvalidOperationException($"Запрошенный пакет отсутствует. SessionId = {sessionId}, PackageId = {packageId}");
                }

                return holder;
            }
        }

        private class PackageHolder
        {
            public TPackageInfo Package { get; set; }

            public string Data { get; set; }

            public string SignedData { get; set; }

            public Dictionary<Type, Dictionary<string, long>> TransportGuidDictionary { get; set; }
        }
    }
}