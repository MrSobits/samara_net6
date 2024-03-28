namespace Bars.Gkh.Regions.Tatarstan.ViewModel.RealityObjectOutdoor
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities.RealityObjectOutdoor;
    using Bars.Gkh.Utils;

    public class RealityObjectOutdoorElementOutdoorViewModel : BaseViewModel<RealityObjectOutdoorElementOutdoor>
    {
        public override IDataResult Get(IDomainService<RealityObjectOutdoorElementOutdoor> domainService, BaseParams baseParams)
        {
            var entity = domainService.Get(baseParams.Params.GetAsId());

            if (entity == null)
            {
                return base.Get(domainService, baseParams);
            }

            return new BaseDataResult(new
            {
                entity.Id,
                Element = new
                {
                    entity.Element?.Id,
                    entity.Element?.Name,
                    UnitMeasure = entity.Element?.UnitMeasure?.Name
                },
                entity.Volume,
                entity.Condition
            });
        }

        public override IDataResult List(IDomainService<RealityObjectOutdoorElementOutdoor> domainService, BaseParams baseParams)
        {
            var outdoorId = baseParams.Params.GetAsId("outdoorId");

            return domainService.GetAll()
                .Where(x => x.Outdoor.Id == outdoorId)
                .Select(x => new
                {
                    x.Id,
                    Group = x.Element.ElementGroup,
                    Element = x.Element.Name,
                    UnitMeasure = x.Element.UnitMeasure.Name,
                    x.Volume,
                    x.Condition
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}