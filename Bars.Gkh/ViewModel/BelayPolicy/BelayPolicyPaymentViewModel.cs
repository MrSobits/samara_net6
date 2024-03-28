namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;

    public class BelayPolicyPaymentViewModel : BaseViewModel<BelayPolicyPayment>
    {
        public override IDataResult List(IDomainService<BelayPolicyPayment> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var belayPolicyId = baseParams.Params.GetAs<long>("belayPolicyId");

            var data = domain.GetAll()
                .Where(x => x.BelayPolicy.Id == belayPolicyId)
                .Select(x => new
                    {
                        x.Id,
                        x.DocumentNumber,
                        x.PaymentDate,
                        x.Sum,
                        x.FileInfo
                    })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}