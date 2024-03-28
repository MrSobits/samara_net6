namespace Bars.Gkh.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Entities;

    public class ConstructiveElementGroupDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            return Container.Resolve<IDomainService<ConstructiveElementGroup>>().GetAll()
                .Filter(loadParam, this.Container)
                .Order(loadParam)
                .ToList();
        }
    }
}