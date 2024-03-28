namespace Bars.GkhDi.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhDi.Entities;

    public class WorkToDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            return Container.Resolve<IDomainService<WorkTo>>().GetAll()
                .Where(x => !x.IsNotActual)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    GroupWorkToName = x.GroupWorkTo.Name
                })
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
        }
    }
}