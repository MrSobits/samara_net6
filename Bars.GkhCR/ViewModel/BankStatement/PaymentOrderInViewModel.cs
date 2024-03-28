namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using Entities;

    public class PaymentOrderInViewModel : BaseViewModel<PaymentOrderIn>
    {
        public override IDataResult List(IDomainService<PaymentOrderIn> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var bankStatementId = baseParams.Params.GetAs<long>("bankStatementId", 0);

            var data = domainService.GetAll()
                .Where(x => x.BankStatement.Id == bankStatementId)
                .Select(x => new
                {
                    x.Id,
                    BankStatementName = x.BankStatement.DocumentNum,
                    x.TypePaymentOrder,
                    x.TypeFinanceSource,
                    PayerContragentName = x.PayerContragent.Name,
                    ReceiverContragentName = x.ReceiverContragent.Name,
                    x.PayPurpose,
                    x.BidNum,
                    x.DocumentNum,
                    x.BidDate,
                    x.DocumentDate,
                    x.Sum,
                    x.RedirectFunds
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}