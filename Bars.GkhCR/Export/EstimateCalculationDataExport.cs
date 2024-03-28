namespace Bars.GkhCr.Export
{
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Utils;
    using DomainService;

    public class EstimateCalculationDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var programmCrId = baseParams.Params.GetAs<long>("programmCrId", 0);
            var loadParam = baseParams.GetLoadParam();

            return Container.Resolve<IEstimateCalculationService>().GetFilteredByOperator()
                .WhereIf(programmCrId > 0, x => x.ProgramCrId == programmCrId)
                .Select(x => new
                    {
                        x.Id,
                        x.ObjectCrId,
                        x.ProgramCrName,
                        x.Municipality,
                        x.RealityObjName,
                        x.TypeWorkCrCount,
                        x.EstimateCalculationsCount
                    })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObjName)
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }
    }
}