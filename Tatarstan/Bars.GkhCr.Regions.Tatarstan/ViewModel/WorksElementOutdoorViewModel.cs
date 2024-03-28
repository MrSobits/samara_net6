namespace Bars.GkhCr.Regions.Tatarstan.ViewModel
{
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict;
    using System.Linq;

    public class WorksElementOutdoorViewModel : BaseViewModel<WorksElementOutdoor>
    {
        public override IDataResult List(IDomainService<WorksElementOutdoor> domainService, BaseParams baseParams)
        {
            var elementOutdoorId = baseParams.Params.GetAsId("elementOutdoorId");

            return domainService
                .GetAll()
                .Where(x => x.ElementOutdoor.Id == elementOutdoorId)
                .Select(x => new
                {
                    x.Id,
                    x.Work.Name,
                    x.Work.TypeWork,
                    UnitMeasure = x.Work.UnitMeasure.Name
                }).ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}