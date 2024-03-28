namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;

    using Entities;

    public class ManOrgRequestSMEVViewModel : BaseViewModel<ManOrgRequestSMEV>
    {

        public override IDataResult List(IDomainService<ManOrgRequestSMEV> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var requestId = loadParams.Filter.GetAs("requestId", 0L);

           var data = domain.GetAll()
               .Where(x => x.LicRequest.Id == requestId)
               .Select(x => new
               {
                   x.Id,
                   x.LicRequest,
                   x.Date,
                   x.RequestSMEVType,
                   x.SMEVRequestState,
                   Inspector = x.Inspector.Fio,
                   x.RequestId
               })
               .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}