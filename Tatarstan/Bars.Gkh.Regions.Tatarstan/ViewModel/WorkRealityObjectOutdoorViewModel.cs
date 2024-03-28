namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    using System.Linq;

    public class WorkRealityObjectOutdoorViewModel : BaseViewModel<WorkRealityObjectOutdoor>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<WorkRealityObjectOutdoor> domainService, BaseParams baseParams)
        {
            var needMainWork = baseParams.Params.GetAs<bool?>("needMainWork");

            return domainService.GetAll()
                .WhereIf(needMainWork.HasValue && needMainWork.Value, x => x.TypeWork == KindWorkOutdoor.Main && x.IsActual)
                .WhereIf(needMainWork.HasValue && !needMainWork.Value, x => x.TypeWork == KindWorkOutdoor.Additional && x.IsActual)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    x.TypeWork,
                    UnitMeasure = x.UnitMeasure.Name,
                    x.IsActual,
                    x.Description
                }).ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}