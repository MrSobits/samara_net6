namespace Bars.GisIntegration.Base.Package.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.GisIntegration.Base.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Менеджер хранимых пакетов
    /// </summary>
    public class StorablePackageManager<TPackageInfo> : PackageManagerBase<TPackageInfo, long>
        where TPackageInfo : IStorablePackageInfo, new()
    {
        private readonly IWindsorContainer container;

        public StorablePackageManager(IWindsorContainer container)
        {
            this.container = container;
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
            var identity = this.container.Resolve<IUserIdentity>();
            var fileManager = this.container.Resolve<IFileManager>();

            var packageDomain = this.container.ResolveDomain<TPackageInfo>();
            var dataDomain = this.container.ResolveDomain<RisPackageData>();

            using (this.container.Using(identity, fileManager, packageDomain, dataDomain))
            {
                var package = default(TPackageInfo);

                this.container.InTransaction(
                    () =>
                        {
                            package = new TPackageInfo { Name = packageName, UserName = identity.Name };

                            var data = new RisPackageData
                                           {
                                               Data =
                                                   fileManager.SaveFile(
                                                       packageName,
                                                       "xml",
                                                       this.XmlToBytes(this.SerializeRequest(notSignedData))),
                                               TransportGuidDictionary = this.SerializeTransportGuidDictionary(transportGuidDictionary)
                                           };

                            dataDomain.Save(data);
                            package.PackageDataId = data.Id;

                            packageDomain.Save(package);
                        });

                return package;
            }
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
            var fileManager = this.container.Resolve<IFileManager>();
            var dataDomain = this.container.ResolveDomain<RisPackageData>();

            using (this.container.Using(fileManager, dataDomain))
            {
                this.container.InTransaction(
                    () =>
                        {
                            var data = dataDomain.Get(packageInfo.PackageDataId);
                            if (data == null)
                            {
                                throw new InvalidOperationException(
                                    $"PackageDataId указывает на несуществующий объект. Id = {packageInfo.Id}, PackageDataId = {packageInfo.PackageDataId}");
                            }

                            fileManager.Delete(data.Data);
                            data.Data = fileManager.SaveFile(packageInfo.Name, "xml", this.XmlToBytes(this.SerializeRequest(notSignedData)));
                            data.TransportGuidDictionary = this.SerializeTransportGuidDictionary(transportGuidDictionary);

                            dataDomain.Update(data);
                        });
            }
        }

        /// <summary>
        /// Сохранить подписанные данные
        /// </summary>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signedData">Подписанные данные</param>
        public override void SaveSignedData(long packageId, string signedData)
        {
            var packageDomain = this.container.ResolveDomain<TPackageInfo>();
            using (this.container.Using(packageDomain))
            {
                this.container.InTransaction(
                    () =>
                        {
                            var package = packageDomain.Get(packageId);
                            if (package == null)
                            {
                                throw new InvalidOperationException($"Пакет с указанным идентитфикаторм не найден. Id = {packageId}");
                            }

                            this.SaveSignedDataInternal(package, signedData);

                            package.Signed = true;
                            packageDomain.Update(package);
                        });
            }
        }

        /// <summary>
        /// Сохранить подписанные данные
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="signedData">Подписанные данные</param>
        public override void SaveSignedData(TPackageInfo packageInfo, string signedData)
        {
            var packageDomain = this.container.ResolveDomain<TPackageInfo>();
            using (this.container.Using(packageDomain))
            {
                this.container.InTransaction(
                    () =>
                        {
                            this.SaveSignedDataInternal(packageInfo, signedData);

                            packageInfo.Signed = true;
                            packageDomain.Update(packageInfo);
                        });
            }
        }

        private void SaveSignedDataInternal(TPackageInfo packageInfo, string signedData)
        {
            var dataDomain = this.container.ResolveDomain<RisPackageData>();
            var fileManager = this.container.Resolve<IFileManager>();

            using (this.container.Using(dataDomain, fileManager))
            {
                this.container.InTransaction(
                    () =>
                        {
                            var data = dataDomain.Get(packageInfo.PackageDataId);
                            if (data == null)
                            {
                                throw new InvalidOperationException(
                                    $"PackageDataId указывает на несуществующий объект. Id = {packageInfo.Id}, PackageDataId = {packageInfo.PackageDataId}");
                            }

                            data.SignedData = fileManager.SaveFile($"{packageInfo.Name}_signed", "xml", Encoding.UTF8.GetBytes(signedData));
                            dataDomain.Update(data);
                        });
            }
        }

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <param name="formatted">Признак - вернуть форматированную/неформатированную xml строку</param>
        /// <returns>Данные xml строкой</returns>
        public override string GetData(long packageId, bool signed, bool formatted)
        {
            var packageDomain = this.container.ResolveDomain<TPackageInfo>();
            using (this.container.Using(packageDomain))
            {
                var package = packageDomain.Get(packageId);
                if (package == null)
                {
                    throw new InvalidOperationException($"Пакет с указанным идентитфикаторм не найден. Id = {packageId}");
                }

                return this.GetDataInternal(package, signed, formatted);
            }
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
            return this.GetDataInternal(packageInfo, signed, formatted);
        }

        private string GetDataInternal(TPackageInfo package, bool signed, bool formatted)
        {
            var dataDomain = this.container.ResolveDomain<RisPackageData>();
            var fileManager = this.container.Resolve<IFileManager>();

            using (this.container.Using(dataDomain, fileManager))
            {
                var data = dataDomain.Get(package.PackageDataId);
                if (data == null)
                {
                    throw new InvalidOperationException(
                        $"PackageDataId указывает на несуществующий объект. Id = {package.Id}, PackageDataId = {package.PackageDataId}");
                }

                var fileStream = fileManager.GetFile(signed ? data.SignedData : data.Data);
                var fileData = this.StreamToByteArray(fileStream);
                return formatted ? this.GetFormattedXmlDataString(fileData) : this.GetXmlDataString(fileData);
            }
        }

        /// <summary>
        /// Получить словать транспортных идентификаторов
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <returns>Словарь транспортных идентификаторов</returns>
        public override Dictionary<Type, Dictionary<string, long>> GetTransportGuidDictionary(TPackageInfo packageInfo)
        {
            var dataDomain = this.container.ResolveDomain<RisPackageData>();

            using (this.container.Using(dataDomain))
            {
                var data = dataDomain.Get(packageInfo.PackageDataId);
                if (data == null)
                {
                    throw new InvalidOperationException(
                        $"PackageDataId указывает на несуществующий объект. Id = {packageInfo.Id}, PackageDataId = {packageInfo.PackageDataId}");
                }

                return this.DeserializeTransportGuidDictionary(data.TransportGuidDictionary);
            }
        }
    }
}