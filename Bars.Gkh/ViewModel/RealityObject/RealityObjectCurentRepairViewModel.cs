using Bars.Gkh.Domain;

namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RealityObjectCurentRepairViewModel : BaseViewModel<RealityObjectCurentRepair>
    {
        public override IDataResult List(IDomainService<RealityObjectCurentRepair> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.UnitMeasure,
                    x.PlanDate,
                    x.PlanSum,
                    x.PlanWork,
                    x.FactDate,
                    x.FactSum,
                    x.FactWork,
                    WorkKindName = x.WorkKind.Name,
                    Builder = x.Builder.Contragent.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<RealityObjectCurentRepair> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("id");
            var record = domainService.Get(id);
            return record != null ? new BaseDataResult( new
                {
                    record.Id,
                    WorkKindName = record.WorkKind.Name,
                    Builder = record.Builder != null ? new { record.Builder.Id, ContragentName = record.Builder.Contragent.Name } : null,
                    record.UnitMeasure,
                    record.PlanDate,
                    record.PlanSum,
                    record.PlanWork,
                    record.FactDate,
                    record.FactSum,
                    record.FactWork,
                }) : new BaseDataResult(null);
        }
    }
}