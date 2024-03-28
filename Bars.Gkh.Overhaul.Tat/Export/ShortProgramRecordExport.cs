namespace Bars.Gkh.Overhaul.Tat.Export
{
    using System.Collections;
    using System.Collections.Generic;
    using B4;
    using B4.Modules.DataExport.Domain;
    using Entities;

    public class ShortProgramRecordExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            //ToDo переделать Экспорт в эксель
            /*
            var loadParams = GetLoadParam(baseParams);

            return Container.Resolve<IDomainService<ShortProgramRecord>>().GetAll()
                .Where(x => x.DpkrCorrection.Stage2.Stage3Version.ProgramVersion.IsMain)
                .Select(x => new
                {
                    Municipality = x.DpkrCorrection.RealityObject.Municipality.Name,
                    x.DpkrCorrection.RealityObject.Address,
                    CeoName = x.DpkrCorrection.Stage2.CommonEstateObject.Name,
                    x.DpkrCorrection.PlanYear,
                    x.DpkrCorrection.Stage2.Sum,
                    BudgetOwners = x.OwnerSumForCR,
                    x.BudgetRegion,
                    x.BudgetMunicipality,
                    x.BudgetFcr,
                    x.BudgetOtherSource
                })
                .OrderBy(x => x.PlanYear)
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
            */ 
             return new List<object>();
        }
    }
}
