namespace Bars.Gkh.RegOperator.Export
{
    using System.Collections;
    using System.Linq;
    using B4.DataAccess;
    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Utils;

    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    using Entities;

    public class RegOperatorExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            return Container.ResolveDomain<RegOperator>().GetAll()
                .Select(x => new
                {
                    Municipality = x.Contragent.Municipality.Name,
                    Contragent = x.Contragent.Name,
                    x.Contragent.Inn,
                    x.Contragent.Kpp
                })
                .Filter(loadParams, Container)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Contragent)
                .Order(loadParams)
                .ToList();
        }
    }
}