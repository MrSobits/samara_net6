namespace Bars.GkhGji.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;

    public class ProtocolRSODataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<IProtocolRSOService>();

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