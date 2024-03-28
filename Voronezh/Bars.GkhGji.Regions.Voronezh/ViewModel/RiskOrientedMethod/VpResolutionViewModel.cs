namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class VpResolutionViewModel : BaseViewModel<VpResolution>
    {
        public override IDataResult List(IDomainService<VpResolution> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var parentId = loadParams.Filter.GetAs("parentId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.ROMCategory.Id == parentId)
                .Select(x => new
                {
                    x.Id,
                    Resolution = x.Resolution.DocumentNumber,
                    ResolutionDate = x.Resolution.DocumentDate,
                    x.ArtLaws
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}