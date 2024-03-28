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

    public class GisGmpExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var gisGmpDomain = this.Container.ResolveDomain<GisGmp>();

            var paymentsContainer = this.Container.ResolveDomain<PayReg>();

            var payments = paymentsContainer.GetAll()
                .Where(x => x.GisGmp != null)
                .GroupBy(x => x.GisGmp.Id)
                .Select(x => new
                {
                    x.Key,
                    Sum = (decimal)x.Sum(y => y.Amount)
                }).ToDictionary(x => x.Key, x => x.Sum);

            var data = gisGmpDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    RequestDate = x.ObjectCreateDate,
                    x.GisGmpPaymentsType,
                    Inspector = x.Inspector.Fio,
                    x.RequestState,
                    AltPayerIdentifier = "_" + x.AltPayerIdentifier,
                    UIN = "_" + x.UIN,
                    x.BillFor,
                    x.TotalAmount,
                    PaymentsAmount = payments.ContainsKey(x.Id) ? payments[x.Id] : 0,
                    x.GisGmpChargeType,
                    x.MessageId,
                    x.CalcDate,
                })
                .AsQueryable()
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();

            return data;
        }
    }
}
