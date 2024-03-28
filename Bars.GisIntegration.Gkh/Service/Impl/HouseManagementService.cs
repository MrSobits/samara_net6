namespace Bars.GisIntegration.Gkh.Service.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.External.Housing.Notif;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Service;
    using Bars.GisIntegration.UI.Service;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    using Castle.Windsor;
    using GkhDi.Entities;

    /// <summary>
    /// Сервис для получения объектов при выполнении импорта/экспорта данных через сервис HouseManagement
    /// </summary>
    public class HouseManagementService : IHouseManagementService
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить список домов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetHouseList(BaseParams baseParams)
        {
            var selector = this.Container.Resolve<IDataSelector<RealityObject>>("HouseDataSelector");
            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();

            try
            {
                var currentContragent = dataSupplierProvider.GetCurrentDataSupplier();

                baseParams.Params.Add("selectedHouses", "all");

                if (baseParams.Params.GetAs("forUO", false))
                {
                    baseParams.Params.Add("uoId", currentContragent.GkhId);
                }

                if (baseParams.Params.GetAs("forOMS", false))
                {
                    baseParams.Params.Add("omsId", currentContragent.GkhId);
                }

                if (baseParams.Params.GetAs("forRSO", false))
                {
                    baseParams.Params.Add("rsoId", currentContragent.GkhId);
                }

                var houseList = selector.GetExternalEntities(baseParams.Params);

                var loadParams = baseParams.GetLoadParam();

                var data = houseList.Select(x =>
                new
                {
                    x.Id,
                    x.Address,
                    HouseType = this.ConvertHouseType(x.TypeHouse).GetDisplayName()
                })
                .AsQueryable()
                .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(selector);
                this.Container.Release(dataSupplierProvider);
            }
        }

        /// <summary>
        /// Конвертировать тип дома
        /// </summary>
        /// <param name="houseType">Тип дома в сторонней системе</param>
        /// <returns>Тип дома Ris</returns>
        private HouseType ConvertHouseType(TypeHouse houseType)
        {
            switch (houseType)
            {
                case TypeHouse.BlockedBuilding:
                    return HouseType.Blocks;
                case TypeHouse.ManyApartments:
                    return HouseType.Apartment;
                default:
                    return HouseType.Living;
            }
        }

        /// <summary>
        /// Получить список договоров
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetContractList(BaseParams baseParams)
        {
            var extractor = this.Container.Resolve<IDataSelector<ManOrgBaseContract>>("ContractDataSelector");
            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var currentContragent = dataSupplierProvider.GetCurrentDataSupplier();

                baseParams.Params.Add("selectedList", "all");

                var contracts = extractor.GetExternalEntities(baseParams.Params)
                    .Select(x =>
                    new
                    {
                        x.Id,
                        DocNum = x.DocumentNumber,
                        SigningDate = x.DocumentDate
                    })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListDataResult(contracts.Order(loadParams).Paging(loadParams).ToList(), contracts.Count());
            }
            finally
            {
                this.Container.Release(extractor);
                this.Container.Release(dataSupplierProvider);
            }
        }

        /// <summary>
        /// Получить список уставов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetCharterList(BaseParams baseParams)
        {
            var extractor = this.Container.Resolve<IDataSelector<ManOrgJskTsjContract>>("CharterSelector");
            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var currentContragent = dataSupplierProvider.GetCurrentDataSupplier();

                baseParams.Params.Add("selectedList", "all");

                var charters =
                    extractor.GetExternalEntities(baseParams.Params)
                        .Select(x => new { x.Id, DocNum = x.DocumentNumber, SigningDate = x.DocumentDate })
                        .AsQueryable()
                        .Filter(loadParams, this.Container);

                return new ListDataResult(charters.Order(loadParams).Paging(loadParams).ToList(), charters.Count());
            }
            finally
            {
                this.Container.Release(extractor);
                this.Container.Release(dataSupplierProvider);
            }
        }

        /// <summary>
        /// Получить список ДОИ
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetPublicPropertyContractList(BaseParams baseParams)
        {
            var extractor = this.Container.Resolve<IDataSelector<InfoAboutUseCommonFacilities>>("PublicPropertyContractSelector");
            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var currentContragent = dataSupplierProvider.GetCurrentDataSupplier();

                baseParams.Params.Add("selectedList", "all");

                var charters =
                    extractor.GetExternalEntities(baseParams.Params)
                        .Select(x => new { x.Id, x.DisclosureInfoRealityObj.RealityObject.Address, x.ContractNumber, x.DateStart, x.DateEnd })
                        .AsQueryable()
                        .Filter(loadParams, this.Container);

                return new ListDataResult(charters.Order(loadParams).Paging(loadParams).ToList(), charters.Count());
            }
            finally
            {
                this.Container.Release(extractor);
                this.Container.Release(dataSupplierProvider);
            }
        }

        /// <summary>
        /// Получить список новостей
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetNotificationList(BaseParams baseParams)
        {
            var extractor = this.Container.Resolve<IDataSelector<NotifAddress>>("NotificationAddresseeSelector");
            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();

            try
            {
                var currentContragent = dataSupplierProvider.GetCurrentDataSupplier();
                baseParams.Params.Add("Contragent", currentContragent);

                var notificationList = extractor.GetExternalEntities(baseParams.Params);

                var loadParams = baseParams.GetLoadParam();

                var data = notificationList
                    .GroupBy(x => x.Notif)
                    .Select(x => new
                    {
                        x.Key.Id,
                        x.Key.NotifTopic,
                        x.Key.NotifContent,
                        x.Key.IsImportant,
                        x.Key.NotifFrom,
                        x.Key.NotifTo,
                        Address = x.Select(y => y.House.FiasAddress.AddressName)
                    })
                .AsQueryable()
                .Order(loadParams)
                .Filter(loadParams, this.Container);

                return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(extractor);
                this.Container.Release(dataSupplierProvider);
            }
        }

        /// <summary>
        /// Получить список договоров ресурсоснабжения
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetSupplyResourceContractList(BaseParams baseParams)
        {
            var extractor = this.Container.Resolve<IDataSelector<PublicServiceOrgContract>>("SupplyResourceContractSelector");

            try
            {
                var loadParams = baseParams.GetLoadParam();
                baseParams.Params.Add("selectedList", "all");

                var contracts = extractor.GetExternalEntities(baseParams.Params)
                    .Select(x =>
                    new
                    {
                        x.Id,
                        x.RealityObject.Address,
                        x.ContractNumber,
                        x.DateStart,
                        x.DateEnd
                    })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListDataResult(contracts.Order(loadParams).Paging(loadParams).ToList(), contracts.Count());
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }
    }
}
