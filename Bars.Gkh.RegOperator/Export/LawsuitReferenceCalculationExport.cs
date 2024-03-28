namespace Bars.Gkh.RegOperator.Export
{
    using System.Collections;
    using System.Linq;

    using B4.DataAccess;
    using B4;
    using B4.Modules.DataExport.Domain;
    using Entities;

    public class LawsuitReferenceCalculationExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var docId = baseParams.Params.GetAs<long>("Lawsuit");

            var lawRefCalcDomain = this.Container.ResolveDomain<LawsuitReferenceCalculation>();

            try
            {
                return lawRefCalcDomain.GetAll()
                .Where(x => x.Lawsuit.Id == docId)
                .Select(x => new 
                {
                        x.Id,
                        x.AccountNumber,
                        Name = "_" + x.PeriodId.Name,
                        x.AreaShare,
                        x.BaseTariff,
                        x.RoomArea,
                        x.PaymentDate,
                        x.TarifDebt,
                        x.TarifDebtPay,
                        x.TariffCharged,
                        x.TarifPayment,
                        x.Description,
                        x.Penalties,
                        x.AccrualPenalties,
                        x.PenaltyPayment,
                        x.PenaltyPaymentDate
                })
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
            }
            finally
            {
                this.Container.Release(lawRefCalcDomain);
            }
        }
    }
}