namespace Bars.GkhDi.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;

    public class WorkPprDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var groupWorkPprId = baseParams.Params.GetAs<long>("groupWorkPprId");

            return Container.Resolve<IDomainService<WorkPpr>>().GetAll()
                .WhereIf(groupWorkPprId > 0, x => x.GroupWorkPpr.Id == groupWorkPprId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    GroupWorkPprName = x.GroupWorkPpr.Name
                })
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
        }
    }
}