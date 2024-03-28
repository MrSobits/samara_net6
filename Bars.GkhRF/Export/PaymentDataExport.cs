namespace Bars.GkhRf.Export
{
    using System.Collections;
    using System.Linq;
    using B4;
    using B4.Modules.DataExport.Domain;

    using Entities;

    public class PaymentDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            return Container.Resolve<IDomainService<ViewPayment>>().GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.Address,
                        Municipality = x.MunicipalityName,
                        x.ChargePopulationCr,
                        x.PaidPopulationCr,
                        x.ChargePopulationHireRf,
                        x.PaidPopulationHireRf,
                        x.ChargePopulationCr185,
                        x.PaidPopulationCr185,
                        x.ChargePopulationBldRepair,
                        x.PaidPopulationBldRepair
                    })
                .Filter(loadParams, this.Container)
                .Order(loadParams)
                .ToList();
        }
    }
}