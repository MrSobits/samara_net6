namespace Bars.GkhGji.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;

    public class HeatSeasonDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<IHeatSeasonService>();

            try
            {
                var totalCount = 0;
                return service.GetListForViewList(baseParams, false, ref totalCount);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}