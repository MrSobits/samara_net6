namespace Bars.GkhGji.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Entities;

    public class InspectedPartGjiDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            return Container.Resolve<IDomainService<InspectedPartGji>>()
                .GetAll()
                .Order(loadParam)
                .Filter(loadParam, Container)
                .ToList();
        }
    }
}