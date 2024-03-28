namespace Bars.GkhGji.DomainService
{
    using Bars.Gkh.Entities;
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Подготовка к отопительному сезону
    /// </summary>
    public class HeatSeasonService : IHeatSeasonService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// получение значений "Документы по подготовке к отопительному сезону"
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="paging"></param>
        /// <param name="totalCount"></param>
        /// <returns>результат запроса</returns>
        public virtual IList GetListForViewList(BaseParams baseParams, bool paging, ref int totalCount)
        {
            var periodService = this.Container.Resolve<IDomainService<HeatSeasonPeriodGji>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var periodId = baseParams.Params.GetAs<long>("periodId");

                var periodDateStart = periodService.GetAll()
                             .Where(x => x.Id == periodId)
                             .Select(x => x.DateStart)
                             .FirstOrDefault();

                // если параметра нет, то ставим true, т.к. запрос списка может быть не только из реестра
                // Если true показываем дома с индивидуальным отоплением
                var showIndividual = baseParams.Params.GetAs("showIndividual", false);

                // Если true показываем дома блокированной застройки
                var showBlocked = baseParams.Params.GetAs("showBlocked", false);

                // Если true показываем индивидуальные дома
                var showIndividualRealObj = baseParams.Params.GetAs("showIndividualRealObj", false);

                var data = this.GetViewList()
                    .Where(x => x.HeatSeasonPeriodId == periodId && x.ConditionHouse != ConditionHouse.Razed && !(x.ConditionHouse == ConditionHouse.Emergency && x.ResidentsEvicted))
                    .WhereIf(periodDateStart.HasValue, x => !x.DateCommissioning.HasValue || x.DateCommissioning < new DateTime(periodDateStart.Value.Year, 09, 15))
                    .WhereIf(!showBlocked, x => x.TypeHouse != TypeHouse.BlockedBuilding)
                    .WhereIf(!showIndividual, x => x.HeatingSystem != HeatingSystem.Individual)
                    .WhereIf(!showIndividualRealObj, x => x.TypeHouse != TypeHouse.Individual)
                    .Where(x => x.ConditionHouse != ConditionHouse.Resettlement)
                    .Select(x => new
                    {
                        x.Id,
                        x.HeatingSeasonId,
                        x.Period,
                        MU = x.Municipality,
                        x.DateHeat,
                        HeatSys = x.HeatingSystem,
                        NumEntr = x.NumberEntrances,
                        Morg = x.ManOrgName,
                        ActF = x.ActFlushing,
                        ActP = x.ActPressing,
                        ActV = x.ActVentilation,
                        ActC = x.ActChimney,
                        x.Passport,
                        x.Address,
                        Type = x.TypeHouse,
                        MinFl = x.Floors,
                        MaxFl = x.MaximumFloors,
                        x.AreaMkd
                    })
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.MU)
                    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Address)
                    .Filter(loadParam, this.Container);

                totalCount = data.Count();

                if (loadParam.Order.Length != 0)
                {
                    data = data.Order(loadParam);
                }

                if (paging)
                {
                    data = data.Paging(loadParam);
                }

                return data.ToList();
            }
            finally 
            {
                this.Container.Release(periodService);
            }
        }

        /// <summary>
        /// получение значений "Документы по подготовке к отопительному сезону"
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult ListView(BaseParams baseParams)
        {

            var totalCount = 0;
            var list = this.GetListForViewList(baseParams, true, ref totalCount);

            return new ListDataResult(list, totalCount);
        }

        /// <summary>
        /// получение значений "Документы по подготовке к отопительному сезону"
        /// </summary>
        /// <returns></returns>
        public IQueryable<ViewHeatingSeason> GetViewList()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceManOrgContractRobject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var heatService = this.Container.Resolve<IDomainService<ViewHeatingSeason>>();

            try
            {
                var contragentList = userManager.GetContragentIds();
                var municipalityList = userManager.GetMunicipalityIds();

                return heatService.GetAll()
                        .WhereIf(contragentList.Count > 0, y => serviceManOrgContractRobject.GetAll()
                            .Any(x => x.RealityObject.Id == y.Id
                                && contragentList.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id)
                                && x.ManOrgContract.StartDate <= DateTime.Now.Date
                                && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Now.Date)))
                        .WhereIf(municipalityList.Count > 0, x => x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value));
            }
            finally 
            {
                this.Container.Release(heatService);
                this.Container.Release(serviceManOrgContractRobject);
                this.Container.Release(userManager);
            }
        }
    }
}