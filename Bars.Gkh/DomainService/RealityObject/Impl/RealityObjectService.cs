namespace Bars.Gkh.DomainService
{
    using System;
    using System.IO;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using B4.Utils;
    using Authentification;
    using B4.Modules.Reports;
    using Domain;
    using Entities.Hcs;
    using Entities;
    using Enums;
    using Castle.Windsor;
    using Utils;
    using B4.Modules.NHibernateChangeLog;
    using B4.IoC;
    using System.Collections.Generic;

    /// <summary>
    /// Сервис для работы с МКД
    /// </summary>
    public class RealityObjectService : IRealityObjectService
    {
        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }
        
        /// <summary>
        /// Список лифтов
        /// </summary>
        /// <param name="baseParams">
        /// </param>
        /// <returns>Список лифтов</returns>
        public IDataResult ListLiftsRegistry(BaseParams baseParams)
        {
            try
            {
                var loadParams = baseParams.GetLoadParam();

                var data = Container.Resolve<IDomainService<RealityObjectLift>>().GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.PorchNum,
                            x.LiftNum,
                            x.RegNum,
                            x.Capacity,
                            x.StopCount,
                            x.Cost,
                            x.CostEstimate,
                            x.YearEstimate,
                            x.ComissioningDate,
                            x.DecommissioningDate,
                            x.PlanDecommissioningDate,
                            x.ReplacementPeriod,
                            x.YearInstallation,
                            x.YearPlannedReplacement,
                            TypeLift = x.TypeLift != null ? x.TypeLift.Name : string.Empty,
                            ModelLift = x.ModelLift != null ? x.ModelLift.Name : string.Empty,
                            TypeLiftShaft = x.TypeLiftShaft != null ? x.TypeLiftShaft.Name : string.Empty,
                            TypeLiftDriveDoors = x.TypeLiftDriveDoors != null ? x.TypeLiftDriveDoors.Name : string.Empty,
                            TypeLiftMashineRoom = x.TypeLiftMashineRoom != null ? x.TypeLiftMashineRoom.Name : string.Empty,
                            Contragent = x.Contragent != null ? x.Contragent.Name : string.Empty,
                            x.AvailabilityDevices,
                            x.SpeedRise,
                            x.LifeTime,
                            x.YearExploitation,
                            x.NumberOfStoreys,
                            x.DepthLiftShaft,
                            x.WidthLiftShaft,
                            x.HeightLiftShaft,
                            x.OwnerLift,
                            CabinLift = x.CabinLift != null ? x.CabinLift.Name : string.Empty,
                            Info = "Лифт №" + x.RegNum + ", подъезд №" + x.PorchNum,
                            Municipality = x.RealityObject.Municipality != null ? x.RealityObject.Municipality.Name : string.Empty,
                            Settlement = x.RealityObject.MoSettlement != null ? x.RealityObject.MoSettlement.Name : string.Empty,
                            Address = x.RealityObject.Address ?? string.Empty,
                            x.RealityObject.CodeErc,
                            x.RealityObject.State
                        })
                    .Filter(loadParams, this.Container);

                var count = data.Count();
                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), count);
            }
            finally
            {
                
            }
        }

        /// <summary>
        /// Возвращает то же самое что и метод List, но данные берет из ViewRealityObject. 
        /// Т.к. нужны наименования управляющих организаций и договоров с жилыми домами
        /// </summary>
        public virtual IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var query = this.GetRealityObjectsQuery(baseParams);
            return new ListDataResult(query.Paging(loadParam).ToList(), query.Count());
        }
        
        public virtual List<long> GetAvaliableRoIds(List<long> contragentList)
        {
            var list = new List<long>();
            return list;
        }
        
        /// <summary>
        /// Получить список из представлений МКД
        /// </summary>
        public IQueryable<ViewRealityObject> GetViewList()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceManOrgContractRobject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var viewRoDomain = this.Container.ResolveDomain<ViewRealityObject>();

            try
            {
                var contragentIds = userManager.GetContragentIds();
                var municipalityIds = userManager.GetMunicipalityIds();

                return viewRoDomain.GetAll()
                    .WhereIf(municipalityIds.Count > 0,
                        x => municipalityIds.Contains(x.MunicipalityId)
                            || (x.SettlementId != null && municipalityIds.Contains(x.SettlementId.GetValueOrDefault())))
                    .WhereIf(contragentIds.Count > 0,
                        y => serviceManOrgContractRobject.GetAll()
                            .Where(x => contragentIds.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id))
                            .Where(x => x.ManOrgContract.StartDate <= DateTime.Today)
                            .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Today)
                            .Any(x => x.RealityObject.Id == y.Id));
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(serviceManOrgContractRobject);
                this.Container.Release(viewRoDomain);
            }
        }

        /// <summary>
        /// Жилые дома (RealityObject) с фильтрацией по оператору
        /// </summary>
        /// <returns>RealityObject</returns>
        public IQueryable<RealityObject> GetFilteredByOperator()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var roDomain = this.Container.ResolveDomain<RealityObject>();
            var serviceManOrgContractRobject = this.Container.ResolveDomain<ManOrgContractRealityObject>();

            try
            {
                var municipalityIds = userManager.GetMunicipalityIds();
                var contragentIds = userManager.GetContragentIds();

                return roDomain.GetAll()
                    .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Municipality.Id))
                    .WhereIf(
                        contragentIds.Count > 0,
                        y => serviceManOrgContractRobject.GetAll()
                            .Where(x => contragentIds.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id))
                            .Where(x => x.ManOrgContract.StartDate <= DateTime.Today)
                            .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Today)
                            .Any(x => x.RealityObject.Id == y.Id));
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(roDomain);
                this.Container.Release(serviceManOrgContractRobject);
            }
        }

        /// <summary>
        /// Список жилых домов, у которых нет текущих договоров непосредственного управления
        /// с фильтрацией по муниципальному образованию оператора
        /// </summary>
        /// <param name="baseParams">
        /// </param>
        /// <returns>Список жилых домов</returns>
        public IDataResult ListExceptDirectManag(BaseParams baseParams)
        {
            var userService = this.Container.Resolve<IGkhUserManager>();
            var serviceManOrgContractRobject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var roDomain = this.Container.ResolveRepository<RealityObject>();
            var managingOrgRealityObjectService = this.Container.Resolve<IManagingOrgRealityObjectService>();
            var manOrgLicenseDomain = this.Container.ResolveDomain<ManOrgLicense>();

            var activeContractsQuery = managingOrgRealityObjectService.GetAllActive()
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.ManagingOrgJskTsj);

            var manorgLicenceQuery = manOrgLicenseDomain.GetAll();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var municipalityIds = userService.GetMunicipalityIds();

                #warning убрать IRepository после исправления GetAll в DomainService жилых домов
                var data = roDomain.GetAll()
                    .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Municipality.Id))
                    .Where(
                        y => !serviceManOrgContractRobject.GetAll()
                            .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
                            .Where(x => !x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate <= DateTime.Today)
                            .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Today)
                            .Any(x => x.RealityObject.Id == y.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Address,
                        Municipality = x.Municipality.Name,

                        ActiveManagingOrganization = activeContractsQuery
                            .Where(y => y.RealityObject == x)
                            .Select(y => y.ManOrgContract.ManagingOrganization.Contragent.ShortName)
                            .FirstOrDefault(),

                        ActiveInn = activeContractsQuery
                            .Where(y => y.RealityObject == x)
                            .Select(y => y.ManOrgContract.ManagingOrganization.Contragent.Inn)
                            .FirstOrDefault(),

                        ActiveDateStart = activeContractsQuery
                            .Where(y => y.RealityObject == x)
                            .Select(y => y.ManOrgContract.StartDate)
                            .FirstOrDefault(),

                        ActiveLicenseDate = manorgLicenceQuery
                            .Where(z => activeContractsQuery
                                .Where(y => y.RealityObject == x).Any(y => y.ManOrgContract.ManagingOrganization.Contragent == z.Contragent))
                            .Select(y => y.DateRegister)
                            .FirstOrDefault()
                    })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(userService);
                this.Container.Release(serviceManOrgContractRobject);
                this.Container.Release(roDomain);
                this.Container.Release(managingOrgRealityObjectService);
            }
        }

        /// <summary>
        /// Список жилых домов, которые указаны во вкладке "жилые дома" в форме редактирования упр.орг
        /// </summary>
        /// <param name="baseParams">
        /// </param>
        /// <returns>Список жилых домов</returns>
        public IDataResult ListByManOrg(BaseParams baseParams)
        {
            var manOrgRoDomain = this.Container.Resolve<IDomainService<ManagingOrgRealityObject>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var manorgId = baseParams.Params.GetAs<long>("manorgId");

                var data = manOrgRoDomain.GetAll()
                    .Where(x => x.ManagingOrganization.Id == manorgId)
                    .Select(
                        x => new
                        {
                            x.RealityObject.Id,
                            x.RealityObject.Address,
                            Municipality = x.RealityObject.Municipality.Name
                        })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(manOrgRoDomain);
            }
        }

        /// <summary>
        /// Список жилых домов, которые указаны во вкладке "жилые дома" в форме редактирования упр.орг
        /// </summary>
        /// <param name="baseParams">
        /// </param>
        /// <returns>Список жилых домов</returns>
        public IDataResult ListByManOrgTumen(BaseParams baseParams)
        {
            var manOrgRoDomain = this.Container.Resolve<IDomainService<ManagingOrgRealityObject>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var manorgId = baseParams.Params.GetAs<long>("manorgId");

                var data = manOrgRoDomain.GetAll()
                    .Where(x => x.ManagingOrganization.Id == manorgId)
                    .Where(x=> x.RealityObject.ConditionHouse != ConditionHouse.Razed)
                    .Select(
                        x => new
                        {
                            x.RealityObject.Id,
                            x.RealityObject.Address,
                            Municipality = x.RealityObject.Municipality.Name
                        })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(manOrgRoDomain);
            }
        }

        /// <summary>
        /// Список МО по контрагенту
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Список МО </returns>
        public IDataResult ListMoByContragentId(BaseParams baseParams)
        {
            var ctrgMunDomain = this.Container.ResolveDomain<ContragentMunicipality>();
            var roRepo = this.Container.ResolveRepository<RealityObject>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var contrId = baseParams.Params.GetAs<long>("contragentId");

                var data = roRepo.GetAll()
                    .Where(x => ctrgMunDomain.GetAll().Any(y => y.Contragent.Id == contrId && y.Municipality.Id == x.Municipality.Id))
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Address,
                            Municipality = x.Municipality.Name
                        })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(ctrgMunDomain);
                this.Container.Release(roRepo);
            }
        }

        /// <summary>
        /// Список жилых домов по постащику
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Список жилых домов</returns>
        public IDataResult ListRoBySupplyResorg(BaseParams baseParams)
        {
            var suplyResorgDomain = this.Container.Resolve<IDomainService<SupplyResourceOrgMunicipality>>();
            var roDomain = this.Container.Resolve<IDomainService<RealityObject>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var supplyResOrgId = baseParams.Params.GetAs<long>("supplyResOrgId");

                var data = roDomain.GetAll()
                    .Where(x => suplyResorgDomain.GetAll().Any(y => y.SupplyResourceOrg.Id == supplyResOrgId && y.Municipality.Id == x.Municipality.Id))
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Address,
                            Municipality = x.Municipality.Name
                        })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(suplyResorgDomain);
                this.Container.Release(roDomain);
            }
        }

        /// <summary>
        /// Список жилых домов по постащику жилищных услуг 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Список жилых домов</returns>
        public IDataResult ListRoByServOrg(BaseParams baseParams)
        {
            var servorgDomain = this.Container.Resolve<IDomainService<ServiceOrgMunicipality>>();
            var roDomain = this.Container.Resolve<IDomainService<RealityObject>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var servOrgId = baseParams.Params.GetAs<long>("contragentId");

                var data = roDomain.GetAll()
                    .Where(x => servorgDomain.GetAll().Any(y => y.ServOrg.Id == servOrgId && y.Municipality.Id == x.Municipality.Id))
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Address,
                            Municipality = x.Municipality.Name
                        })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(servorgDomain);
                this.Container.Release(roDomain);
            }
        }

        /// <summary>
        /// Список Жилых домов по поставщику ком.услуг
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Список Жилых домов</returns>
        public IDataResult ListBySupplySerOrg(BaseParams baseParams)
        {
            var supplyOrgRoDomain = this.Container.Resolve<IDomainService<SupplyResourceOrgRealtyObject>>();
            var roDomain = this.Container.Resolve<IDomainService<RealityObject>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var supplyResOrgId = baseParams.Params.GetAs<long>("supplyResOrgId");

                var roId = supplyOrgRoDomain.GetAll()
                    .Where(x => x.SupplyResourceOrg.Id == supplyResOrgId)
                    .Select(x => x.RealityObject.Id);

                var data = roDomain.GetAll()
                    .Where(x => roId.Contains(x.Id))
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Address,
                            Municipality = x.Municipality.Name
                        })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(supplyOrgRoDomain);
                this.Container.Release(roDomain);
            }
        }

        /// <summary>
        /// Список Жилых домов по guid населенного пункта
        /// </summary>
        /// <param name="baseParams">
        /// </param>
        /// <returns>Список Жилых домов</returns>
        public virtual IDataResult ListByLocalityGuid(BaseParams baseParams)
        {
            var roDomain = this.Container.Resolve<IDomainService<RealityObject>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var localityGuid = baseParams.Params.GetAs<string>("localityGuid");
                var muId = baseParams.Params.GetAs<long>("muId");
                var settlementId = baseParams.Params.GetAs<long>("settlementId");

                var data = roDomain.GetAll()
                    .WhereIf(!string.IsNullOrEmpty(localityGuid), x => x.FiasAddress.PlaceGuidId == localityGuid)
                    .WhereIf(muId > 0, x => x.Municipality.Id == muId)
                    .WhereIf(settlementId > 0, x => x.MoSettlement.Id == settlementId)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Address,
                            Municipality = x.Municipality.Name
                        })
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(roDomain);
            }
        }

        /// <summary>
        /// Список жилых домов, которые указаны во вкладке "жилые дома" в форме редактирования орг. жил. услуг
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Список жилых домов</returns>
        public IDataResult ListByServOrg(BaseParams baseParams)
        {
            var servOrgRoDomain = this.Container.Resolve<IDomainService<ServiceOrgRealityObject>>();
            try
            {
                var loadParams = baseParams.GetLoadParam();

                var servorgId = baseParams.Params.GetAs<long>("servorgId");

                var data = servOrgRoDomain.GetAll()
                    .Where(x => x.ServiceOrg.Id == servorgId)
                    .Select(
                        x => new
                        {
                            x.RealityObject.Id,
                            x.RealityObject.Address,
                            Municipality = x.RealityObject.Municipality.Name
                        })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(servOrgRoDomain);
            }
        }

        /// <summary>
        /// Возвращает список жилых домов по типу юрлица,
        /// Если уо - список домов в управлении на переданную дату
        /// иначе - весь список домов
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Список жилых домов</returns>
        public IDataResult ListByTypeOrg(BaseParams baseParams)
        {
            var roDomain = this.Container.Resolve<IDomainService<RealityObject>>();
            var manOrgRoDomain = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var contragentId = baseParams.Params.GetAs<long>("contragentId");
                var date = baseParams.Params.GetAs("date", DateTime.MinValue);
                var typeJurPerson = baseParams.Params.GetAs("typeJurPerson", 0);
                var isPhysicalPerson = baseParams.Params.GetAs("isPhysicalPerson", false);

                var query = roDomain.GetAll();

                if (!isPhysicalPerson && typeJurPerson > 0)
                {
                    switch ((TypeJurPerson) typeJurPerson)
                    {
                        case TypeJurPerson.ManagingOrganization:

                            query = manOrgRoDomain.GetAll()
                                .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Id == contragentId)
                                .WhereIf(
                                    date > DateTime.MinValue,
                                    x => x.ManOrgContract.StartDate <= date
                                        && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= date))
                                .Select(x => x.RealityObject);
                            break;
                    }
                }

                var data = query
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Address,
                            Municipality = x.Municipality.Name
                        })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(roDomain);
                this.Container.Release(manOrgRoDomain);
            }
        }

        /// <summary>
        /// Список жилых домов, для сведений о ЖКУ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Список жилых домов</returns>
        public IDataResult ListGkuInfo(BaseParams baseParams)
        {
            var realObjService = this.Container.Resolve<IDomainService<RealityObject>>();
            var houseOverallBalanceService = this.Container.Resolve<IDomainService<HouseOverallBalance>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var showIndividual = baseParams.Params.GetAs<bool>("showIndividual");
                var showBlocked = baseParams.Params.GetAs<bool>("showBlocked");

                var querySum = houseOverallBalanceService.GetAll()
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.MonthCharge,
                        x.Paid,
                        Debt = x.MonthCharge - x.Paid
                    });

                var data = realObjService.GetAll()
                    .Where(x => x.ConditionHouse != ConditionHouse.Razed)
                    .WhereIf(!showIndividual, x => x.TypeHouse != TypeHouse.Individual)
                    .WhereIf(!showBlocked, x => x.TypeHouse != TypeHouse.BlockedBuilding)
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.Municipality.Name,
                        x.Address,
                        MonthCharge = (decimal?) querySum.Where(y => y.RoId == x.Id).Sum(z => z.MonthCharge),
                        Paid = (decimal?) querySum.Where(y => y.RoId == x.Id).Sum(z => z.Paid),
                        Debt = (decimal?) querySum.Where(y => y.RoId == x.Id).Sum(z => z.Debt)
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.Municipality,
                        x.Address,
                        x.MonthCharge,
                        x.Paid,
                        x.Debt,
                        Collection = x.MonthCharge.HasValue && x.Paid.HasValue && x.MonthCharge.Value != 0
                            ? (x.Paid.Value / x.MonthCharge.Value).RoundDecimal(2)
                            : (decimal?) null
                    })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(realObjService);
                this.Container.Release(houseOverallBalanceService);
            }
        }

        /// <summary>
        /// Сведения о ЖКУ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Сведения о ЖКУ</returns>
        public IDataResult GetGkuInfo(BaseParams baseParams)
        {
            var realObjService = this.Container.Resolve<IDomainService<RealityObject>>();
            var houseInfoService = this.Container.Resolve<IDomainService<HouseInfoOverview>>();

            try
            {
                var roId = baseParams.Params.GetAs<long>("id");
                var ro = realObjService.GetAll().FirstOrDefault(x => x.Id == roId);
                var hi = houseInfoService.GetAll().FirstOrDefault(x => x.RealityObject.Id == roId) ?? new HouseInfoOverview();

                return new BaseDataResult(
                    new
                    {
                        ro.Id,
                        ro.Municipality,
                        ro.FiasAddress,
                        ro.Address,
                        ro.AreaLiving,
                        ro.AreaLivingOwned,
                        ro.AreaOwned,
                        ro.AreaMunicipalOwned,
                        ro.AreaGovernmentOwned,
                        ro.AreaNotLivingFunctional,
                        ro.AreaMkd,
                        ro.AreaBasement,
                        ro.DateLastOverhaul,
                        ro.DateCommissioning,
                        ro.CapitalGroup,
                        ro.CodeErc,
                        ro.DateDemolition,
                        ro.MaximumFloors,
                        ro.RoofingMaterial,
                        ro.WallMaterial,
                        ro.HavingBasement,
                        ro.IsInsuredObject,
                        ro.Notation,
                        ro.SeriesHome,
                        ro.TypeProject,
                        ro.HeatingSystem,
                        ro.ConditionHouse,
                        ro.TypeHouse,
                        ro.TypeRoof,
                        ro.FederalNum,
                        ro.PhysicalWear,
                        ro.TypeOwnership,
                        ro.Floors,
                        ro.NumberApartments,
                        ro.NumberEntrances,
                        ro.NumberLifts,
                        ro.NumberLiving,
                        ro.Description,
                        ro.GkhCode,
                        ro.WebCameraUrl,
                        ro.DateTechInspection,
                        ro.ResidentsEvicted,
                        ro.DeleteAddressId,
                        ro.IsBuildSocialMortgage,
                        ro.TotalBuildingVolume,
                        ro.CadastreNumber,
                        ro.CadastralHouseNumber,
                        ro.NecessaryConductCr,
                        ro.FloorHeight,
                        ro.PercentDebt,
                        ro.PrivatizationDateFirstApartment,
                        ro.HasPrivatizedFlats,
                        ro.BuildYear,
                        ro.State,
                        ro.MethodFormFundCr,
                        ro.HasJudgmentCommonProp,
                        ro.IsRepairInadvisable,
                        hi.IndividualAccountsCount,
                        hi.IndividualOwnerAccountsCount,
                        hi.IndividualTenantAccountsCount,
                        hi.LegalAccountsCount,
                        hi.LegalOwnerAccountsCount,
                        hi.LegalTenantAccountsCount
                    });
            }
            finally
            {
                this.Container.Release(realObjService);
                this.Container.Release(houseInfoService);
            }
        }

        public IDataResult GetPassportReport(BaseParams baseParams)
        {
            var printDomain = this.Container.ResolveAll<IPrintForm>();

            try
            {
                var printForm = printDomain.FirstOrDefault(x => x.Name == "RealtyObjectPassport");

                if (printForm == null)
                {
                    return new BaseDataResult(false);
                }

                var rp = new ReportParams();

                printForm.SetUserParams(baseParams);
                printForm.PrepareReport(rp);
                var template = printForm.GetTemplate();

                IReportGenerator generator;
                if (printForm is IGeneratedPrintForm)
                {
                    generator = printForm as IGeneratedPrintForm;
                }
                else
                {
                    generator = this.Container.Resolve<IReportGenerator>("XlsIoGenerator");
                }

                var result = new MemoryStream();

                generator.Open(template);
                generator.Generate(result, rp);
                result.Seek(0, SeekOrigin.Begin);

                return new BaseDataResult(result);
            }
            finally
            {
                this.Container.Release(printDomain);
            }
        }

        public IDataResult ListWoPagingCapitalGroup(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var domaneService = this.Container.Resolve<IDomainService<CapitalGroup>>();

            var data = domaneService.GetAll().Select(x => new {x.Id, x.Name}).Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name)
                .Order(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        /// <summary>
        /// Список жилых домов по муниципальному образованию образованием(поселению)
        /// </summary>
        /// <param name="baseParams">
        /// </param>
        /// <returns>Список жилых домов</returns>
        public virtual IDataResult ListByMoSettlement(BaseParams baseParams)
        {
            var roDomain = this.Container.Resolve<IDomainService<RealityObject>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var roIds = baseParams.Params.GetAs("Id", string.Empty).ToLongArray();

                var settlementIds = baseParams.Params.GetAs("settlementId", string.Empty).ToLongArray();

                var data = roDomain.GetAll()
                    .WhereIf(roIds.Any() && roIds.First() != 0, x => roIds.Contains(x.Id))
                    .WhereIf(
                        settlementIds.Any() && settlementIds.First() != 0,
                        x => settlementIds.Contains(x.MoSettlement.Id)
                            || (x.MoSettlement.ParentMo != null && settlementIds.Contains(x.MoSettlement.ParentMo.Id)))
                    .Select(x => new {x.Id, Name = x.Address})
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(roDomain);
            }
        }

        #region IRealityObjectService Members

        /// <summary>
        /// Тарифы ЖКУ
        /// </summary>
        /// <param name="baseParams">
        /// </param>
        /// <returns>Тарифы ЖКУ</returns>
        public IDataResult ListGkuInfoTarifs(BaseParams baseParams)
        {
            var houseChargeService = this.Container.Resolve<IDomainService<HouseAccountCharge>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var roId = baseParams.Params.GetAs<long>("id", 0, true);
                var monthField = baseParams.Params.GetAs<DateTime>("monthField");

                var data = houseChargeService.GetAll()
                    .Where(x => x.RealityObject.Id == roId)
                    .WhereIf(
                        monthField != DateTime.MinValue,
                        x => x.DateCharging != null
                            && x.DateCharging.Month == monthField.Month
                            && x.DateCharging.Year == monthField.Year)
                    .Select(
                        x => new
                        {
                            x.Supplier,
                            x.Service,
                            x.Tariff,
                            Month = x.DateCharging.ToString("MM.yyyy")
                        })
                    .ToList()
                    .GroupBy(x => new {x.Supplier, x.Tariff})
                    .Select(
                        x => new
                        {
                            x.Key.Supplier,
                            x.Key.Tariff,
                            Month = x.Select(y => y.Month).FirstOrDefault(),
                            Service = x.Select(y => y.Service).FirstOrDefault()
                        })
                    .AsQueryable();

                int totalCount = data.Count();
                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(houseChargeService);
            }
        }

        #endregion

        /// <summary>
        /// Возвращает информацию о доме, где адрес дома сформирован в необходимом для поиска виде
        /// </summary>
        /// <param name="baseParams">
        /// </param>
        /// <returns>Информация о доме</returns>
        public virtual IDataResult GetForMap(BaseParams baseParams)
        {
            var roDomain = this.Container.ResolveDomain<RealityObject>();
            var fias = this.Container.ResolveDomain<Fias>();

            try
            {
                var id = baseParams.Params.GetAs<long>("id");
                var dt = roDomain.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new {placeGuid = x.FiasAddress.PlaceGuidId}).First();

                string place = "";
                string placeParent = "";
                var placeFias = fias.GetAll().Where(x => x.AOGuid == dt.placeGuid).First();
                if (placeFias != null)
                {
                    place = placeFias.FormalName + " " + placeFias.ShortName;
                    var placeParentFias = fias.GetAll().Where(x => x.AOGuid == placeFias.ParentGuid).First();
                    if (placeParentFias != null)
                    {
                        placeParent = placeParentFias.FormalName + " " + placeParentFias.ShortName;
                    }
                }

                var data = roDomain.GetAll()
                    .Where(x => x.Id == id)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.ExternalId,
                            Municipality = x.Municipality.Name,
                            Settlement = x.MoSettlement.Name,

                            //x.SettlementId,
                            //x.Settlement,
                            x.Address,
                            FullAddress = x.FiasAddress.AddressName,
                            SearchAddress = placeParent + ", " + place + ", " + x.Address,
                            x.TypeHouse,
                            x.ConditionHouse,
                            x.DateDemolition,
                            x.Floors,
                            x.NumberEntrances,
                            x.NumberLiving,
                            x.NumberApartments,
                            x.AreaMkd,
                            x.AreaLiving,
                            x.PhysicalWear,
                            x.NumberLifts,
                            x.HeatingSystem,
                            WallMaterialId = (long?) x.WallMaterial.Id,
                            WallMaterialName = x.WallMaterial.Name,
                            RoofingMaterialId = (long?) x.RoofingMaterial.Id,
                            RoofingMaterialName = x.RoofingMaterial.Name,
                            x.TypeRoof,
                            x.DateLastOverhaul,
                            x.DateCommissioning,
                            CapitalGroup = x.CapitalGroup.Name,
                            ManOrgNames = x.ManOrgs,

                            //x.ManOrgNames,
                            TypeContracts = x.TypesContract,
                            x.CodeErc,
                            x.IsInsuredObject,
                            x.GkhCode,
                            x.IsBuildSocialMortgage,
                            x.State,
                            IsRepairInadvisable = (bool?) x.IsRepairInadvisable,
                            IsInvolvedCr = !x.IsNotInvolvedCr
                        })
                    .First();

                return new BaseDataResult(data);
            }
            finally
            {
                this.Container.Release(roDomain);
            }
        }

        /// <summary>
        /// Получение запроса жилых домов 
        /// </summary>
        /// <param name="baseParams">Базовые параметры </param>
        /// <returns>Запрос жилых домов</returns>
        public virtual IQueryable<RealityObjectProxy> GetRealityObjectsQuery(BaseParams baseParams)
        {
            var roDomain = this.Container.ResolveDomain<RealityObject>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                // Если true показываем снесенные дома
                var filterShowDemolished = baseParams.Params.GetAs<bool>("filterShowDemolished");

                // Если true показываем индивидуальные дома
                var filterShowIndividual = baseParams.Params.GetAs<bool>("filterShowIndividual");

                // Если true показываем дома блокированной застройки
                var filterShowBlockedBuilding = baseParams.Params.GetAs<bool>("filterShowBlockedBuilding");

                //Данный костыль добавляю потому как Пользователи при фильтрации иногда забывают (или не хотят) вбивать пробелы
                //Но при этом хотят чтобы их значения находились
                //соответственно я получаю Фильтр по Адресу и проверяю наличие данных как по Переданному фильтру, так и с удалением пробелов
                var complexFilter = loadParam.FindInComplexFilter("Address");

                string filterForAddress = null;

                if (complexFilter != null)
                {
                    filterForAddress = complexFilter.Value.ToStr();
                    loadParam.SetComplexFilterNull("Address");
                }

                var query = roDomain.GetAll()
                    .WhereIf(!filterShowBlockedBuilding, x => x.TypeHouse != TypeHouse.BlockedBuilding)
                    .WhereIf(!filterShowIndividual, x => x.TypeHouse != TypeHouse.Individual)
                    .WhereIf(!filterShowDemolished, x => x.ConditionHouse != ConditionHouse.Razed)
                    .WhereIf(
                        !filterForAddress.IsEmpty(),
                        x => x.Address != null
                            && (x.Address.ToUpper().Contains(filterForAddress.ToUpper())
                                || x.Address.Replace(" ", "")
                                    .ToUpper()
                                    .Contains(filterForAddress.Replace(" ", "").ToUpper())))
                    .Select(
                        x => new RealityObjectProxy
                        {
                            Id = x.Id,
                            ExternalId = x.ExternalId,
                            Municipality = x.Municipality.Name,
                            Settlement = x.MoSettlement.Name,
                            Address = x.Address,
                            FullAddress = x.FiasAddress.AddressName,
                            TypeHouse = x.TypeHouse,
                            ConditionHouse = x.ConditionHouse,
                            DateDemolition = x.DateDemolition,
                            Floors = x.MaximumFloors,
                            NumberEntrances = x.NumberEntrances,
                            NumberLiving = x.NumberLiving,
                            NumberApartments = x.NumberApartments,
                            AreaMkd = x.AreaMkd,
                            AreaLiving = x.AreaLiving,
                            PhysicalWear = x.PhysicalWear,
                            NumberLifts = x.NumberLifts,
                            HeatingSystem = x.HeatingSystem,
                            WallMaterialId = (long?) x.WallMaterial.Id,
                            WallMaterialName = x.WallMaterial.Name,
                            RoofingMaterialId = (long?) x.RoofingMaterial.Id,
                            RoofingMaterialName = x.RoofingMaterial.Name,
                            TypeRoof = x.TypeRoof,
                            DateLastOverhaul = x.DateLastOverhaul,
                            DateCommissioning = x.DateCommissioning,
                            CapitalGroup = x.CapitalGroup.Name,
                            ManOrgNames = x.ManOrgs,
                            TypeContracts = x.TypesContract,
                            CodeErc = x.CodeErc,
                            IsInsuredObject = x.IsInsuredObject,
                            GkhCode = x.GkhCode,
                            IsBuildSocialMortgage = x.IsBuildSocialMortgage,
                            State = x.State,
                            IsRepairInadvisable = (bool?) x.IsRepairInadvisable,
                            IsNotInvolvedCr = x.IsNotInvolvedCr,
                            District = x.District
                        })
                    .Filter(loadParam, this.Container)
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Address)
                    .Order(loadParam);

                return query;
            }
            finally
            {
                this.Container.Release(roDomain);
            }
        }

        /// <summary>
        /// Получение изменений по текущему жилому дому 
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Список изменений</returns>
        public IDataResult GetHistory(BaseParams baseParams)
        {
            var roSeIdHistoryDomain = this.Container.ResolveDomain<RealityObject>();
            var logEntityDomain = this.Container.ResolveDomain<LogEntity>();

            using (this.Container.Using(logEntityDomain, roSeIdHistoryDomain))
            {
                var loadParam = baseParams.GetLoadParam();
                var realityObjectId = baseParams.Params.GetAs<long>("objectId");

                var data = logEntityDomain.GetAll()
                    .Where(x => x.ActionKind == ActionKind.Update || x.ActionKind == ActionKind.Insert)
                    .Where(x => x.EntityType == typeof(RealityObject).FullName
                        && x.EntityId == realityObjectId)
                    .Select(x => new RealityObjectLogEntity
                    {
                        Id = x.Id,
                        EntityDateChange = x.ChangeDate,
                        Ip = x.UserIpAddress,
                        UserLogin = x.UserLogin,
                        EntityTypeChange = x.ActionKind,
                    })
                    .Filter(loadParam, this.Container);

                var excludedLogEntities = this.GetExcludeLogEntity(data);

                data = data.Where(x => !excludedLogEntities.Contains(x.Id));

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
        }

        private List<long> GetExcludeLogEntity(IQueryable<RealityObjectLogEntity> data)
        {
            var logEntityPropertyDomain = this.Container.ResolveDomain<LogEntityProperty>();

            var insertedLogEntityIds = data.Where(x => x.EntityTypeChange == ActionKind.Insert).Select(x => x.Id).ToList();
            var filterLogEnityId = new List<long>();

            using (this.Container.Using(logEntityPropertyDomain))
            {
                var logEntityProperties = logEntityPropertyDomain.GetAll()
                    .Where(x => insertedLogEntityIds.Contains(x.LogEntity.Id))
                    .Select(x => new { x.Id, x.NewValue, x.LogEntity })
                    .AsEnumerable()
                    .GroupBy(x => x.LogEntity.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());

                foreach (var logEntityProperty in logEntityProperties)
                {
                    if (logEntityProperty.Value.All(x => x.NewValue.IsEmpty()))
                    {
                        filterLogEnityId.Add(logEntityProperty.Key);
                    }
                }
            }

            return filterLogEnityId;
        }

        private class RealityObjectLogEntity
        {
            public long Id { get; set; }

            public string EntityDescription { get; set; }

            public DateTime EntityDateChange { get; set; }

            public string Ip { get; set; }

            public string UserLogin { get; set; }

            public ActionKind EntityTypeChange { get; set; }
        }

        /// <summary>
        /// Детализации по изменениям
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Список изменений</returns>
        public IDataResult GetHistoryDetail(BaseParams baseParams)
        {
            var logEntityDomain = this.Container.ResolveDomain<LogEntity>();
            var logEntityPropertyDomain = this.Container.ResolveDomain<LogEntityProperty>();

            using (this.Container.Using(logEntityDomain, logEntityPropertyDomain))
            {
                var loadParams = baseParams.GetLoadParam();
                var logEntityId = loadParams.Filter.GetAs<long>("logEntityId");
                const string PropertyName = "Неизвестный атрибут";

                var logEntity = logEntityDomain.GetAll().First(x => x.Id == logEntityId);

                var dictProperties = this.Container.Resolve<IChangeLogInfoProvider>()
                    .GetLoggedEntities()
                    .GroupBy(y => y.EntityType)
                    .Select(y => new { y.Key, Properties = y.SelectMany(z => z.Properties).ToList() })
                    .ToDictionary(y => y.Key, z => z.Properties);

                var properties = dictProperties.ContainsKey(logEntity.EntityType)
                    ? dictProperties[logEntity.EntityType].ToDictionary(x => x.PropertyCode, y => y)
                    : null;

                var data = logEntityPropertyDomain.GetAll()
                    .Where(x => x.LogEntity == logEntity)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        PropertyName = properties.ContainsKey(x.PropertyCode)
                            ? properties[x.PropertyCode].DisplayName
                            : PropertyName,
                        x.NewValue,
                        x.OldValue,
                        Type = properties.ContainsKey(x.PropertyCode)
                            ? this.GetNativeType(properties[x.PropertyCode].Type).Name
                            : null,
                    })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
        private Type GetNativeType(Type type)
        {
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return type.GetGenericArguments()[0];
                }
            }

            return type;
        }
    }
}