namespace Bars.Gkh.Overhaul.Nso.Export
{
    using System.Collections;
    using System.Linq;
    using B4;
    using B4.Modules.DataExport.Domain;
    using Entities;

    public class ShortProgramRecordExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            return Container.Resolve<IDomainService<ShortProgramRecord>>().GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                .Where(x => x.RealityObject != null)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.Address,
                    CeoName = x.Stage2.CommonEstateObject.Name,
                    PlanYear = x.Year,
                    x.Stage2.Sum,
                    BudgetOwners = x.OwnerSumForCr,
                    x.BudgetRegion,
                    x.BudgetMunicipality,
                    x.BudgetFcr,
                    x.BudgetOtherSource
                })
                .OrderBy(x => x.PlanYear)
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
        }
    }
}
