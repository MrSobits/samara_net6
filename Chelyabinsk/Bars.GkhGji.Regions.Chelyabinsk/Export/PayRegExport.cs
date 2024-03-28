namespace Bars.GkhGji.Regions.Chelyabinsk.Export
{
    using System.Collections;
    using System.Linq;
    using B4.DataAccess;
    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Utils;

    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    using Entities;
    using Bars.Gkh.Enums;

    public class PayRegExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var paymentsDomain = this.Container.ResolveDomain<PayReg>();

            var data = paymentsDomain.GetAll()
           .Select(x => new
           {
               x.Id,
               x.Amount,
               Kbk = "_" + x.Kbk,
               x.OKTMO,
               x.GisGmp,
               x.PaymentDate,
               PaymentId = "_" + x.PaymentId,
               x.Purpose,
               SupplierBillID = "_" + x.SupplierBillID,
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
           .AsQueryable()
           .Select(x => new
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
               PayerId = "_" + x.PayerId,
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
               GisGmpUIN = "_" + x.GisGmpUIN,
               x.IsGisGmpConnected
           })
           .Filter(loadParams, Container)
           .Order(loadParams)
           .ToList();

            return data;
        }
    }
}
