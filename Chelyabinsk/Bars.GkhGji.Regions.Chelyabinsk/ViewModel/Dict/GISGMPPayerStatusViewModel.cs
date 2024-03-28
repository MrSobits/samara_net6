namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Entities;

    public class GISGMPPayerStatusViewModel : BaseViewModel<GISGMPPayerStatus>
    {
        public override IDataResult List(IDomainService<GISGMPPayerStatus> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
              .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
