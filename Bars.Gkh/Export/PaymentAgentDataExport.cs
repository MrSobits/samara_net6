namespace Bars.Gkh.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService;

    public class PaymentAgentDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<IPaymentAgentService>();

            try
            {
                int totalCount;
                return service.GetViewModelList(baseParams, out totalCount, false);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}