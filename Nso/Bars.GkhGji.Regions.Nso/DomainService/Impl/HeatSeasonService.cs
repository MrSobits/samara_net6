namespace Bars.GkhGji.Regions.Nso.DomainService
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class HeatSeasonService : GkhGji.DomainService.HeatSeasonService
    {
        public override IList GetListForViewList(BaseParams baseParams, bool paging, ref int totalCount)
        {
            var periodService = Container.Resolve<IDomainService<HeatSeasonPeriodGji>>();
            var heatDocService = Container.Resolve<IDomainService<HeatSeasonDoc>>();

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

                var data = GetViewList()
                    .Where(x => x.HeatSeasonPeriodId == periodId && x.ConditionHouse != ConditionHouse.Razed && !(x.ConditionHouse == ConditionHouse.Emergency && x.ResidentsEvicted))
                    .WhereIf(periodDateStart.HasValue, x => !x.DateCommissioning.HasValue || x.DateCommissioning < new DateTime(periodDateStart.Value.Year, 09, 15))
                    .WhereIf(!showBlocked, x => x.TypeHouse != TypeHouse.BlockedBuilding)
                    .WhereIf(!showIndividual, x => x.HeatingSystem != HeatingSystem.Individual)
                    .WhereIf(!showIndividualRealObj, x => x.TypeHouse != TypeHouse.Individual)
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
                        ActR = x.HeatingSeasonId.HasValue && heatDocService.GetAll().Any(y => y.HeatingSeason.Id == x.HeatingSeasonId.Value && y.TypeDocument == HeatSeasonDocType.ActReadyHeatingDevices),
                        x.Passport,
                        x.Address,
                        Type = x.TypeHouse,
                        MinFl = x.Floors,
                        MaxFl = x.MaximumFloors,
                        x.AreaMkd
                    })
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.MU)
                    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Address)
                    .Filter(loadParam, Container);

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
                Container.Release(periodService);
                Container.Release(heatDocService);
            }
        }
        
    }
}