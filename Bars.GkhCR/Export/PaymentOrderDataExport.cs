namespace Bars.GkhCr.Export
{
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Utils;
    using DomainService;

    public class PaymentOrderDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var periodId = baseParams.Params.GetAs<long>("periodId", 0);

            return Container.Resolve<IBasePaymentOrderService>().GetFilteredByOperator()
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
                .OrderIf(loadParam.Order.Length == 0, true, x => x.MunicipalityName)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealObjName)
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }
    }
}