namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using System.Linq;

    public class MKDLicRequestSourceViewModel : BaseViewModel<MKDLicRequestSource>
    {
        public override IDataResult List(IDomainService<MKDLicRequestSource> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var requestId = baseParams.Params.GetAs<long>("mkdlicrequestId");

            var data = domainService.GetAll()
                .Where(x => x.MKDLicRequest.Id == requestId)
                .Select(x => new
                {
                    x.Id,
                    x.MKDLicRequest,
                    x.RevenueDate,
                    x.SSTUDate,
                    x.RevenueSourceNumber,
                    RevenueSource = x.RevenueSource != null ? x.RevenueSource.Name : string.Empty,
                    RevenueForm = x.RevenueForm != null ? x.RevenueForm.Name : string.Empty
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}