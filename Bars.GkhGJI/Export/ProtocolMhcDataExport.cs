namespace Bars.GkhGji.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;

    public class ProtocolMhcDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<IProtocolMhcService>();

            try
            {
                var totalCount = 0;
                return service.GetViewModelList(baseParams, false, ref totalCount);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}