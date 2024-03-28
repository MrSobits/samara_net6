namespace Bars.Gkh.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class ContragentDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            return Container.Resolve<IDomainService<Contragent>>().GetAll()
                .Select(x => new
                    {
                        x.Id,
                        Municipality = x.Municipality.Name,
                        JuridicalAddress = (x.AddressOutsideSubject != null && x.AddressOutsideSubject != "" ? x.AddressOutsideSubject : x.JuridicalAddress),
                        x.Name,
                        x.Phone,
                        x.Inn,
                        x.Kpp,
                        x.ContragentState,
                        x.Ogrn,
                        x.EgrulExcDate
                    })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.JuridicalAddress)
                .Filter(loadParam, this.Container)
                .Order(loadParam)
                .ToList();
        }
    }
}