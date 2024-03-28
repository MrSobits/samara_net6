namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Enums;

    using Entities;
    using Gkh.DataResult;

    public class FinActivityManagRealityObjViewModel : BaseViewModel<FinActivityManagRealityObj>
    {
        public override IDataResult List(IDomainService<FinActivityManagRealityObj> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var diInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            // Получаем все дома по данной упр орг в данный период 
            var diInfo = Container.Resolve<IDomainService<DisclosureInfo>>().Get(diInfoId);

            if (diInfo == null)
            {
                return new ListDataResult();
            }

            // Получаем дома, которые должны отображаться в реестре
            var robjects = 
                Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                    .Where(x => x.ManOrgContract.ManagingOrganization.Id == diInfo.ManagingOrganization.Id)
                    .Where(x => x.RealityObject.TypeHouse != TypeHouse.BlockedBuilding
                             && x.RealityObject.TypeHouse != TypeHouse.Individual)
                    .Where(x => x.RealityObject.ConditionHouse == ConditionHouse.Serviceable
                             || x.RealityObject.ConditionHouse == ConditionHouse.Emergency
                             || x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated
                             && !x.RealityObject.ResidentsEvicted)
                    .Select(x => new
                    {
                        x.RealityObject.Id,
                        x.RealityObject.FiasAddress.AddressName,
                        x.RealityObject.AreaMkd,
                        x.ManOrgContract.StartDate,
                        x.ManOrgContract.EndDate
                    })
                    .Where(x => x.StartDate <= diInfo.PeriodDi.DateEnd && (!x.EndDate.HasValue || x.EndDate >= diInfo.PeriodDi.DateStart))
                    .Select(x => new { x.Id, x.AddressName, x.AreaMkd })
                    .AsEnumerable();

            // Получаем уже имеющиеся в базе записи
            var finActRobjDict =
                Container.Resolve<IDomainService<FinActivityManagRealityObj>>().GetAll()
                    .Where(x => x.DisclosureInfo.Id == diInfoId)
                    .ToDictionary(x => x.RealityObject.Id);

            var dataList = new List<FinActRoProxy>();

            // Догоняем полученные дома уже имеющимися в базе данными
            foreach (var ro in robjects)
            {
                if (finActRobjDict.ContainsKey(ro.Id))
                {
                    var rec = finActRobjDict[ro.Id];

                    dataList.Add(new FinActRoProxy
                    {
                        RoId = ro.Id,
                        Address = ro.AddressName,
                        AreaMkd = ro.AreaMkd,
                        ObjId = rec.Id,
                        PresentedToRepay = rec.PresentedToRepay,
                        ReceivedProvidedService = rec.ReceivedProvidedService,
                        SumDebt = rec.SumDebt,
                        SumFactExpense = rec.SumFactExpense,
                        SumIncomeManage = rec.SumIncomeManage
                    });
                }
                else
                {
                    dataList.Add(new FinActRoProxy
                    {
                        RoId = ro.Id,
                        Address = ro.AddressName,
                        AreaMkd = ro.AreaMkd
                    });
                }
            }

            var data = dataList
                .Select(
                    x => new
                    {
                        Id = x.RoId, // Если Id == 0 то записи схлопываются в одну, для этого id модели берем за id дома
                        ObjectId = x.ObjId, // храним Id сущности для сохранения/обновления
                        AddressName = x.Address,
                        x.AreaMkd,
                        x.PresentedToRepay,
                        x.ReceivedProvidedService,
                        x.SumDebt,
                        x.SumFactExpense,
                        x.SumIncomeManage
                    })
                .DistinctBy(x => x.Id)
                .AsQueryable()
                .OrderIf(loadParams.Order.Length == 0, true, x => x.AddressName)
                .Filter(loadParams, Container);

            data = data.Order(loadParams);
            var totalCount = data.Count();
            var areaMkd = totalCount > 0 ? data.Sum(x => x.AreaMkd) : 0;
            var presentedToRepay = totalCount > 0 ? data.Sum(x => x.PresentedToRepay) : 0;
            var receivedProvidedService = totalCount > 0 ? data.Sum(x => x.ReceivedProvidedService) : 0;
            var sumDebt = totalCount > 0 ? data.Sum(x => x.SumDebt) : 0;
            var sumFactExpense = totalCount > 0 ? data.Sum(x => x.SumFactExpense) : 0;
            var sumIncomeManage = totalCount > 0 ? data.Sum(x => x.SumIncomeManage) : 0;

            return new ListSummaryResult(data.Paging(loadParams).ToList(), totalCount, 
                new { AreaMkd = areaMkd, PresentedToRepay = presentedToRepay, ReceivedProvidedService = receivedProvidedService,
                    SumDebt = sumDebt, SumFactExpense = sumFactExpense, SumIncomeManage = sumIncomeManage});
        }

        private class FinActRoProxy
        {
            public long RoId { get; set; }
            public long? ObjId { get; set; }
            public string Address { get; set; }
            public decimal? AreaMkd { get; set; }
            public decimal? PresentedToRepay { get; set; }
            public decimal? ReceivedProvidedService { get; set; }
            public decimal? SumDebt { get; set; }
            public decimal? SumFactExpense { get; set; }
            public decimal? SumIncomeManage { get; set; }
        }
    }
}
