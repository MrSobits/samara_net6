namespace Bars.Gkh.RegOperator.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;

    internal class TransferCtrExport : BaseDataExportService
    {
        public IDomainService<TransferCtr> DomainService { get; set; }

        public ITransferCtrService TransferCtrService { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            return this.TransferCtrService.List(baseParams)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .ToList();
        }
    }
}