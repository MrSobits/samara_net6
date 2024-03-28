namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Gkh.DataResult;
    using Entities;

    public class BasePaymentOrderViewModel : BaseViewModel<BasePaymentOrder>
    {
        public override IDataResult List(IDomainService<BasePaymentOrder> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var periodId = baseParams.Params.GetAs<long>("periodId", 0);

            var data = Container.Resolve<IBasePaymentOrderService>().GetFilteredByOperator()
                .WhereIf(periodId > 0, x => x.BankStatement.Period.Id == periodId)
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
                        x.RedirectFunds,
                        MunicipalityName = x.BankStatement.ObjectCr.RealityObject.Municipality.Name,
                        RealObjName = x.BankStatement.ObjectCr.RealityObject.Address,
                        x.RepeatSend
                    })
                .Filter(loadParams, Container);

            var summary = data.Sum(x => x.Sum);
            var totalCount = data.Count();

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.MunicipalityName)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealObjName);

            var result = loadParams.Order.Length == 0 ? data.Paging(loadParams) : data.Order(loadParams).Paging(loadParams);

            return new ListSummaryResult(result.ToList(), totalCount, new { Sum = summary });
        }       
    }
}