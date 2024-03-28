namespace Bars.GkhRf.Export
{
    using System.Collections;
    using System.Linq;
    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Utils;
    using Entities;

    public class TransferRfDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            return Container.Resolve<IDomainService<ViewTransferRf>>().GetAll()
                .Select(x => new
                    {
                        x.Id,
                        x.DocumentNum,
                        x.DocumentDate,
                        x.MunicipalityName,
                        x.ManagingOrganizationName,
                        x.ContractRfObjectsCount
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.MunicipalityName)
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
        }
    }
}