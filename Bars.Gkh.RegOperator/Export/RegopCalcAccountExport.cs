namespace Bars.Gkh.RegOperator.Export
{
    using System.Collections;
    using System.Linq;
    using B4;
    using B4.Modules.DataExport.Domain;

    using Bars.Gkh.RegOperator.DomainService;

    public class RegopCalcAccountExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<IRegopCalcAccountService>();
            try
            {
                var loadParams = baseParams.GetLoadParam();

                return service.GetProxyQueryable(baseParams).Order(loadParams).ToList();
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}
