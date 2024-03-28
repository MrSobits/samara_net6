namespace Bars.Gkh.Regions.Tatarstan.ViewModel.RealityObjectOutdoor
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities.RealityObjectOutdoor;
    using Bars.Gkh.Utils;

    public class OutdoorImageViewModel : BaseViewModel<OutdoorImage>
    {
        public override IDataResult List(IDomainService<OutdoorImage> domainService, BaseParams baseParams)
        {
            var outdoorId = baseParams.Params.GetAsId("outdoorId");

            return domainService.GetAll()
                .Where(x => x.Outdoor.Id == outdoorId)
                .Select(x => new
                {
                    x.Id,
                    x.DateImage,
                    Period = x.Period.Name,
                    x.Name,
                    x.ImagesGroup,
                    WorkCr = x.WorkCr.Name,
                    x.Description,
                    x.File
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}