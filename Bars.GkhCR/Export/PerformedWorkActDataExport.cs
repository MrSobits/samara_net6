namespace Bars.GkhCr.Export
{
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Utils;

    using Bars.GkhCr.Enums;

    using DomainService;

    public class PerformedWorkActDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var realityObjId = baseParams.Params.GetAs<long>("realityObjId", 0);
            var programFilterId = baseParams.Params.GetAs<long>("programFilterId", 0);
            var municipalities = baseParams.Params.GetAs("municipalities", string.Empty);

            var municipalityIds = !string.IsNullOrEmpty(municipalities)
                ? municipalities.Split(';').Select(x => x.ToLong()).ToArray()
                : new long[0];
            
            return Container.Resolve<IPerformedWorkActService>().GetFilteredByOperator()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(realityObjId > 0, x => x.ObjectCr.RealityObject.Id == realityObjId)
                .WhereIf(programFilterId > 0, x => x.ObjectCr.ProgramCr.Id == programFilterId)
                .Where(x => x.ObjectCr.ProgramCr.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full)
                .Select(x => new
                    {
                        x.Id,
                        ObjectCrId = x.ObjectCr.Id,
                        x.ObjectCr.RealityObject.Address,
                        x.Sum,
                        x.State,
                        WorkName = x.TypeWorkCr.Work.Name,
                        Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                        WorkFinanceSource = x.TypeWorkCr.FinanceSource.Name,
                        x.DocumentNum,
                        x.DateFrom,
                        x.Volume
                    })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Address)
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }
    }
}