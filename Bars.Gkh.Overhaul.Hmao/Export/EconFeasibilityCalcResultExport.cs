namespace Bars.Gkh.Overhaul.Hmao.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class EconFeasibilityCalcResultExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var econFesabilityDomain = Container.Resolve<IDomainService<EconFeasibilityCalcResult>>();

            var loadParams = GetLoadParam(baseParams);

            return econFesabilityDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Adress = x.RoId.Address,
                    Municipality = x.RoId.Municipality.Name,
                    MoSettlement = x.RoId.MoSettlement != null ? x.RoId.MoSettlement.Name : "",
                    x.YearStart,
                    x.YearEnd,
                    x.TotatRepairSumm,
                    SquareCost = x.SquareCost.Cost,
                    x.TotalSquareCost,
                    x.CostPercent,
                    x.Decision //= EnumToTextHelper.EconFeasibilityToStrinng(x.Decision)
                }).Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
        }
    }
}