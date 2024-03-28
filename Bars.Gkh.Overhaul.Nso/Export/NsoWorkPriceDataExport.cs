namespace Bars.Gkh.Overhaul.Nso.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.Gkh.Overhaul.DomainService;

    using Entities;

    public class NsoWorkPriceDataExport : Bars.Gkh.Overhaul.Export.WorkPriceDataExport<NsoWorkPrice>
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<IWorkPriceService<NsoWorkPrice>>();

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