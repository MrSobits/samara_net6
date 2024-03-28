namespace Bars.Gkh.Overhaul.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;

    using Entities;

    public class CreditOrgExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            return Container.Resolve<IDomainService<CreditOrg>>().GetAll()
                .Select(x => new
                    {
                        x.Name,
                        x.Inn,
                        x.Kpp,
                        Address = x.FiasAddress != null ? x.Address : x.AddressOutSubject
                    })
                .Filter(loadParams, Container)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name)
                .Order(loadParams)
                .ToList();
        }
    }
}