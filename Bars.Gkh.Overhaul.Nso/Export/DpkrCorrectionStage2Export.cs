namespace Bars.Gkh.Overhaul.Nso.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class DpkrCorrectionStage2Export : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);

            return Container.Resolve<IDomainService<DpkrCorrectionStage2>>().GetAll()
                                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                                    .Select(x => new
                                    {
                                        Municipality = x.RealityObject.Municipality.Name,
                                        RealityObject = x.RealityObject.Address,
                                        CorrectionYear = x.PlanYear,
                                        PlanYear = x.Stage2.Stage3Version.Year,
                                        x.Stage2.Sum,
                                        CommonEstateObjectName = x.Stage2.CommonEstateObject.Name,
                                        x.Stage2.Stage3Version.IndexNumber
                                    })
                                    .OrderBy(x => x.CorrectionYear)
                                    .Filter(loadParam, Container)
                                    .Order(loadParam)
                                    .ToList();
        }
    }
}