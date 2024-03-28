namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;

    public class PaymentSizeMuRecordViewModel : BaseViewModel<PaymentSizeMuRecord>
    {
        public override IDataResult List(IDomainService<PaymentSizeMuRecord> domainService, BaseParams baseParams)
        {
            var paymentSizeCrId = baseParams.Params.GetAs<long>("paymentSizeCrId");
            var loadParams = GetLoadParam(baseParams);

            var data =
                domainService.GetAll()
                    .Where(x => x.PaymentSizeCr.Id == paymentSizeCrId)
                    .Select(x => new
                    {
                        x.Id, 
                        Municipality = x.Municipality.Name
                    })
                    .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
