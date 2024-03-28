namespace Bars.GkhCr.Regions.Tatarstan.ViewModel.ObjectOutdoorCr
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class TypeWorkRealityObjectOutdoorViewModel : BaseViewModel<TypeWorkRealityObjectOutdoor>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TypeWorkRealityObjectOutdoor> domainService, BaseParams baseParams)
        {
            var objectOutdoorCrId = baseParams.Params.GetAsId("objectOutdoorCrId");
            return domainService.GetAll()
                .Where(x => x.ObjectOutdoorCr.Id == objectOutdoorCrId && x.IsActive)
                .Select(x => new
                {
                    x.Id,
                    x.WorkRealityObjectOutdoor.TypeWork,
                    WorkRealityObjectOutdoorName = x.WorkRealityObjectOutdoor.Name,
                    UnitMeasure = x.WorkRealityObjectOutdoor.UnitMeasure.Name,
                    x.Volume,
                    x.Sum
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}
