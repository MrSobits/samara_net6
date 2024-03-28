namespace Bars.Gkh.RegOperator.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.CashPaymentCenter;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;
    using Gkh.Enums;
    using ConfigSections.RegOperator;
    using Gkh.Utils; 
    
    /// <summary>
    /// Сервис расчётно-кассовых центров
    /// </summary>
    public class CashPaymentCenterService : ICashPaymentCenterService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Список расчетно-кассовых центров без пагинации
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var domaneService = Container.Resolve<IDomainService<Entities.CashPaymentCenter>>();

            var data = domaneService.GetAll().Select(x => new { x.Id, x.Contragent.Name }).Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name)
                .Order(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        /// <summary>
        /// Добавить связь расчетно-кассового центра с МО
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                var cashPaymentCenterDomain = Container.Resolve<IDomainService<Entities.CashPaymentCenter>>();
                var cashPaymentCenterMuDomain = Container.Resolve<IDomainService<CashPaymentCenterMunicipality>>();
                var municipalityDomain = Container.Resolve<IDomainService<Municipality>>();

                try
                {
                    var cashPaymentCenterId = baseParams.Params.GetAs<long>("cashPaymentCenterId");
                    var muIds = baseParams.Params.GetAs<long[]>("muIds");

                    var existRecs =
                        cashPaymentCenterMuDomain.GetAll()
                            .Where(x => x.CashPaymentCenter.Id == cashPaymentCenterId)
                            .Select(x => x.Municipality.Id)
                            .Distinct()
                            .AsEnumerable()
                            .ToDictionary(x => x);

                    var cashPaymentCenter = cashPaymentCenterDomain.Load(cashPaymentCenterId);

                    foreach (var id in muIds)
                    {
                        if (existRecs.ContainsKey(id))
                            continue;

                        var newObj = new CashPaymentCenterMunicipality
                        {
                            CashPaymentCenter = cashPaymentCenter,
                            Municipality = municipalityDomain.Load(id)
                        };

                        cashPaymentCenterMuDomain.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(cashPaymentCenterDomain);
                    Container.Release(cashPaymentCenterMuDomain);
                    Container.Release(municipalityDomain);
                }
            }
        }

        /// <summary>
        /// Прикрепить дома к расчётно-кассовому центру
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult AddObjects(BaseParams baseParams)
        {
            var cashPaymentCenterId = baseParams.Params.GetAs<long>("cashPaymentCenterId");
            var objectIds = baseParams.Params.GetAs("objectIds", new long[0]);
            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");

            var cachPaymentCenterConnectionType = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.CachPaymentCenterConnectionType;
            var serviceName = "CachPaymentCenterConnectionType.{0}".FormatUsing(cachPaymentCenterConnectionType.ToString("G"));
            var cashPaymentCenterAddObjectsService = this.Container.Resolve<ICashPaymentCenterObjectsService>(serviceName);

            using (this.Container.Using(cashPaymentCenterAddObjectsService))
            {
                return cashPaymentCenterAddObjectsService.AddObjects(cashPaymentCenterId, objectIds, dateStart, dateEnd);
            }
        }

        /// <summary>
        /// Установить расчётно-кассовый центр, из которого вызвана функция,
        /// всем Л/С без расчётно-кассового центра
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult SetCashPaymentCenters(BaseParams baseParams)
        {
            var cashPaymentCenterId = baseParams.Params.GetAs<long>("cashPaymentCenterId");

            var cashPaymentCenterConnectionType = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.CachPaymentCenterConnectionType;
            var serviceName = "CachPaymentCenterConnectionType.{0}".FormatUsing(cashPaymentCenterConnectionType.ToString("G"));
            var cashPaymentCenterAddObjectsService = this.Container.Resolve<ICashPaymentCenterObjectsService>(serviceName);

            using (this.Container.Using(cashPaymentCenterAddObjectsService))
            {
                return cashPaymentCenterAddObjectsService.SetCashPaymentCenters(cashPaymentCenterId);
            }
        }

        /// <summary>
        /// Открепить объекты от расчётно-кассового центру
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult DeleteObjects(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();

            var cachPaymentCenterConnectionType = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.CachPaymentCenterConnectionType;
            var serviceName = "CachPaymentCenterConnectionType.{0}".FormatUsing(cachPaymentCenterConnectionType.ToString("G"));
            var cashPaymentCenterAddObjectsService = this.Container.Resolve<ICashPaymentCenterObjectsService>(serviceName);

            using (this.Container.Using(cashPaymentCenterAddObjectsService))
            {
                return cashPaymentCenterAddObjectsService.DeleteObjects(ids);
            }
        }

        /// <summary>
        /// Список домов для прикрепления их к Расчетно-кассовому центру
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult ListObjForCashPaymentCenter(BaseParams baseParams)
        {
            var realObjDomain = Container.ResolveDomain<RealityObject>();
            var cashPaymentCenterMuDomain = Container.ResolveDomain<CashPaymentCenterMunicipality>();
            var persAccDomain = Container.ResolveDomain<BasePersonalAccount>();

            using (Container.Using(realObjDomain, cashPaymentCenterMuDomain, persAccDomain))
            {
                var loadParams = baseParams.GetLoadParam();
                var cashPaymentCenterId = baseParams.Params.GetAsId("cashPaymentCenterId");
                var isShowPersAcc = baseParams.Params.GetAs<bool>("isShowPersAcc");

                if (isShowPersAcc)
                {
                    var data = persAccDomain
                                .GetAll()
                                .Where(x => cashPaymentCenterMuDomain
                                    .GetAll()
                                    .Where(y => y.CashPaymentCenter.Id == cashPaymentCenterId)
                                    .Any(y => y.Municipality.Id == x.Room.RealityObject.Municipality.Id))
                                .Select(x => new
                                {
                                    x.Id,
                                    x.PersonalAccountNum,
                                    x.Room.RealityObject.Address,
                                    Municipality = x.Room.RealityObject.Municipality.Name
                                })
                                .Order(loadParams)
                                .Filter(loadParams, Container);

                    return new ListDataResult(data.Paging(loadParams), data.Count());
                }
                else
                {
                    var data = realObjDomain
                                 .GetAll()
                                 .Where(x => cashPaymentCenterMuDomain
                                     .GetAll()
                                     .Where(y => y.CashPaymentCenter.Id == cashPaymentCenterId)
                                     .Any(y => y.Municipality.Id == x.Municipality.Id))
                                 .Where(x => persAccDomain.GetAll().Any(y => y.Room.RealityObject.Id == x.Id))
                                 .Select(x => new
                                 {
                                     x.Id,
                                     x.Address,
                                     Municipality = x.Municipality.Name
                                 })
                                 .Order(loadParams)
                                 .Filter(loadParams, Container);

                    return new ListDataResult(data.Paging(loadParams), data.Count());
                }
            }
        }

        /// <summary>
        /// Список домов в Расчетно-кассовом центре с управляющими компаниями
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult ListRealObjForCashPaymentCenterManOrg(BaseParams baseParams)
        {
            var realObjRepo = Container.ResolveRepository<RealityObject>();
            var manOrgContractRoDomain = Container.ResolveRepository<ManOrgContractRealityObject>();
            var roDicrectManagContractDomain = Container.Resolve<IDomainService<RealityObjectDirectManagContract>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var manOrgByRoIdDict = manOrgContractRoDomain.GetAll()
                    .Select(x => new
                    {
                        x.RealityObject.Id,
                        ManagingOrganizationName = x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag ?
                            roDicrectManagContractDomain.GetAll().Any(y => y.Id == x.ManOrgContract.Id && y.IsServiceContract)
                                ? ManOrgBaseContract.DirectManagementWithContractText : ManOrgBaseContract.DirectManagementText
                            : x.ManOrgContract.ManagingOrganization.Contragent.Name,
                    })
                    .ToList()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.First().ManagingOrganizationName);

                var data = realObjRepo
                    .GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        RealityObject = x.Id,
                        Municipality = x.Municipality.Name,
                        x.Address,
                        ManOrg = manOrgByRoIdDict.ContainsKey(x.Id)
                            ? manOrgByRoIdDict[x.Id]
                            : string.Empty
                    })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                    .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
            }
            finally
            {
                Container.Release(realObjRepo);
                Container.Release(manOrgContractRoDomain);
                Container.Release(roDicrectManagContractDomain);
            }
        }
    }
}