namespace Bars.GkhCr.Regions.Tatarstan.ViewModel
{
    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;
    using System.Linq;

    public class ElementOutdoorViewModel : BaseViewModel<ElementOutdoor>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ElementOutdoor> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    x.ElementGroup,
                    UnitMeasure = x.UnitMeasure.Name
                }).ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}