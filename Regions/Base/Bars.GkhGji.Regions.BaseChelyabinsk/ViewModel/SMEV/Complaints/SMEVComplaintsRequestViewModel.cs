namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SMEVComplaintsRequestViewModel : BaseViewModel<SMEVComplaintsRequest>
    {
        public override IDataResult List(IDomainService<SMEVComplaintsRequest> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var complaintId = baseParams.Params.GetAs<long>("complaintId");
            var data = domainService.GetAll()
                .WhereIf(complaintId>0, x=> x.ComplaintId.HasValue && x.ComplaintId.Value == complaintId)
                .Select(x=> new
                {
                    x.Id,
                    ReqId = x.Id,
                    x.MessageId,
                    x.TypeComplainsRequest,
                    RequestDate = x.ObjectCreateDate,
                    Inspector = x.Inspector.Fio,
                    x.RequestState,
                    x.Answer
                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


       
    }
}
