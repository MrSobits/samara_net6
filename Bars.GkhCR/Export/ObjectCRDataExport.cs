using Bars.GkhCr.DomainService;

namespace Bars.GkhCr.Export
{
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Utils;
    using Entities;
    using Enums;

    public class ObjectCrDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var programId = baseParams.Params.GetAs("programId", string.Empty);
            var municipalityId = baseParams.Params.GetAs("municipalityId", string.Empty);

            var programIds = !string.IsNullOrEmpty(programId) ? programId.Split(',').Select(x => x.ToInt()).ToArray() : new int[0];
            var municipalityIds = !string.IsNullOrEmpty(municipalityId) ? municipalityId.Split(',').Select(x => x.ToInt()).ToArray() : new int[0];

            var serviceProgramCr = Container.Resolve<IDomainService<ProgramCr>>();

            return Container.Resolve<IObjectCrService>().GetFilteredByOperator()
                .Where(x => x.ProgramCrId.HasValue)
                .WhereIf(programIds.Length > 0, x => programIds.Contains(x.ProgramCrId.Value))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.MunicipalityId))
                .Where(y => serviceProgramCr.GetAll()
                    .Any(x =>
                        x.Id == y.ProgramCrId
                        && x.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Hidden
                        && x.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Print))
                .Select(x => new
                    {
                        x.Id,
                        x.ProgramNum,
                        x.DateAcceptCrGji,
                        RealityObjName = x.Address,
                        x.Municipality,
                        x.State,
                        x.ProgramCrName,
                        AllowReneg = x.DateAcceptCrGji,
                        MonitoringSmrState = x.SmrState
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObjName)
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
        }
    }
}