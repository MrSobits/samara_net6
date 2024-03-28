namespace Bars.GisIntegration.UI.Service.Impl
{
    using System;

    using Bars.B4;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Package;
    using Bars.GisIntegration.Base.Package.Impl;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с пакетами
    /// </summary>
    public class PackageService: IPackageService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить xml данные пакета форматированные для просмотра
        /// либо неформатированные для подписи
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие 
        /// идентификатор пакета, 
        /// тип пакета,
        /// признак: для предпросмотра
        /// признак: подписанные/неподписанные данные</param>
        /// <returns>xml данные пакета</returns>
        public IDataResult GetPackageXmlData(BaseParams baseParams)
        {
            var packageId = baseParams.Params.GetAs("packageId", string.Empty);
            var packageType = baseParams.Params.GetAs("packageType", string.Empty);
            var forPreview = baseParams.Params.GetAs<bool>("forPreview");
            var signed = baseParams.Params.GetAs<bool>("signed");

            if (string.IsNullOrEmpty(packageId))
            {
                return new BaseDataResult(false, "Не удалось получить идентификатор пакета");
            }

            if (string.IsNullOrEmpty(packageType))
            {
                return new BaseDataResult(false, "Не удалось получить тип пакета");
            }

            var packageManager = this.GetPackageManager(packageType);
            try
            {
                return new BaseDataResult(packageManager.GetData(packageId, signed, forPreview));
            }
            finally
            {
                this.Container.Release(packageManager);
            }
        }

        /// <summary>
        /// Сохранить подписанную xml
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// идентификатор пакета, 
        /// тип пакета,
        /// подписанные данные
        /// </param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult SaveSignedData(BaseParams baseParams)
        {
            var packageId = baseParams.Params.GetAs("packageId", string.Empty);
            var packageType = baseParams.Params.GetAs("packageType", string.Empty);
            var signedData = baseParams.Params.GetAs("signedData", string.Empty);

            if (string.IsNullOrEmpty(packageId))
            {
                return new BaseDataResult(false, "Не удалось получить идентификатор пакета");
            }

            if (string.IsNullOrEmpty(packageType))
            {
                return new BaseDataResult(false, "Не удалось получить тип пакета");
            }

            if (string.IsNullOrEmpty(signedData))
            {
                return new BaseDataResult(false, "Пустые подписанные данные");
            }

            var packageManager = this.GetPackageManager(packageType);
            try
            {
                packageManager.SaveSignedData(packageId, Uri.UnescapeDataString(signedData));
                return new BaseDataResult(new { Signed = true });
            }
            finally
            {
                this.Container.Release(packageManager);
            }
        }

        private IPackageManager GetPackageManager(string packageType)
        {
            switch (packageType.ToLower())
            {
                case "storable":
                    return this.Container.Resolve<IPackageManager<RisPackage, long>>();
                case "temp":
                    return this.Container.Resolve<IPackageManager<TempPackageInfo, Guid>>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(packageType));
            }
        }
    }
}
