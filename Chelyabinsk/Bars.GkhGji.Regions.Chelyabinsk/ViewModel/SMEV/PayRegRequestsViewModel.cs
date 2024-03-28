namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using B4;
    using Entities;
    using System.Linq;

    public class PayRegRequestsViewModel : BaseViewModel<PayRegRequests>
    {
        public override IDataResult List(IDomainService<PayRegRequests> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Filter(loadParams, Container)
                .Select(x=> new
                {
                    x.Id,
                    RequestDate = x.ObjectCreateDate,
                    x.PayRegPaymentsType,
                    Inspector = x.Inspector.Fio,
                    x.RequestState,
                    x.MessageId, 
                    x.CalcDate,
                    x.Answer,
                    x.GetPaymentsStartDate,
                    x.GetPaymentsEndDate
                });

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


     
    }
}
