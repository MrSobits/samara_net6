namespace Bars.Gkh.Overhaul.Hmao.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;

    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class HmaoWorkPriceDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<IWorkPriceService<HmaoWorkPrice>>();

            try
            {
                int totalCount = 0;
                return service.GetListView(baseParams, false, ref totalCount);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}