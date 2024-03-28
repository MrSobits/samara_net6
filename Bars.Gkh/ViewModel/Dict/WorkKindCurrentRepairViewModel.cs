namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class WorkKindCurrentRepairViewModel : BaseViewModel<WorkKindCurrentRepair>
    {
        public override IDataResult List(IDomainService<WorkKindCurrentRepair> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    UnitMeasure = x.UnitMeasure.Name,
                    TypeWork = x.TypeWork.GetEnumMeta().Display
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}