namespace Bars.GkhCr.Regions.Tatarstan.ViewModel.ObjectOutdoorCr
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class TypeWorkRealityObjectOutdoorHistoryViewModel : BaseViewModel<TypeWorkRealityObjectOutdoorHistory>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TypeWorkRealityObjectOutdoorHistory> domainService, BaseParams baseParams)
        {
            var objectOutdoorCrId = baseParams.Params.GetAs<long>("objectOutdoorCrId");

            return domainService.GetAll()
                .Where(x => x.TypeWorkRealityObjectOutdoor.ObjectOutdoorCr.Id == objectOutdoorCrId)
                .Select(x => new
                {
                    x.Id,
                    TypeWorkRealityObjectOutdoor = x.TypeWorkRealityObjectOutdoor.WorkRealityObjectOutdoor.Name,
                    x.ObjectCreateDate,
                    x.TypeAction,
                    x.Volume,
                    x.Sum,
                    UnitMeasure = x.TypeWorkRealityObjectOutdoor.WorkRealityObjectOutdoor.UnitMeasure.Name,
                    x.UserName
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}
