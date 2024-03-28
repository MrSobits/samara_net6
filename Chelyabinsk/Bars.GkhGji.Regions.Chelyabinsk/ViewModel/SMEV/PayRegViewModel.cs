namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;

    public class PayRegViewModel : BaseViewModel<PayReg>
    {
        public override IDataResult List(IDomainService<PayReg> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
           .Select(x => new
           {
               x.Id,
               x.Amount,
               x.Kbk,
               x.OKTMO,
               x.GisGmp,
               x.PaymentDate,
               x.PaymentId,
               x.Purpose,
               x.SupplierBillID,
               PaymentOrg = x.PaymentOrg != null ? x.PaymentOrg : "Неизвестно",
               PaymentOrgDescr = x.PaymentOrgDescr != null ? x.PaymentOrgDescr : "",
               PayerId = x.PayerId != null ? x.PayerId : "",
               PayerAccount = x.PayerAccount != null ? x.PayerAccount : "",
               PayerName = x.PayerName != null ? x.PayerName : "",
               BdiStatus = x.BdiStatus != null ? x.BdiStatus : "",
               BdiPaytReason = x.BdiPaytReason != null ? x.BdiPaytReason : "",
               BdiTaxPeriod = x.BdiTaxPeriod != null ? x.BdiTaxPeriod : "",
               BdiTaxDocNumber = x.BdiTaxDocNumber != null ? x.BdiTaxDocNumber : "",
               BdiTaxDocDate = x.BdiTaxDocDate != null ? x.BdiTaxDocDate : "",
               GisGmpUIN = x.GisGmp != null ? x.GisGmp.UIN : "",
               IsGisGmpConnected = x.GisGmp != null ? YesNoNotSet.Yes : YesNoNotSet.No,
               AccDocDate = x.AccDocDate != null ? x.AccDocDate : System.DateTime.MinValue,
               AccDocNo = x.AccDocNo != null ? x.AccDocNo : "",
               Status = x.Status != null ? x.Status : 0,
               x.Reconcile,

           })
           .AsQueryable();

            var data2 = data.Select(x=> new
            {
                x.Id,
                x.Amount,
                x.Kbk,
                x.OKTMO,
                x.PaymentDate,
                x.PaymentId,
                x.Purpose,
                x.SupplierBillID,
                x.PaymentOrg,
                x.PaymentOrgDescr,
                x.PayerId,
                x.PayerAccount,
                x.PayerName,
                x.BdiStatus,
                x.GisGmp,
                x.BdiPaytReason,
                x.BdiTaxPeriod,
                x.BdiTaxDocNumber,
                x.BdiTaxDocDate,
                x.AccDocDate,
                x.AccDocNo,
                x.Status,
                x.Reconcile,
                x.GisGmpUIN,
                x.IsGisGmpConnected
            })
            .Filter(loadParams, Container);

            return new ListDataResult(data2.Order(loadParams).Paging(loadParams).ToList(), data2.Count());



        }
    }
}