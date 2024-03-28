namespace Bars.Gkh.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class ManagingOrganizationDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            /*
             * Галочка над гридом показывать не действующие организации
             */
            var showNotValid = !baseParams.Params.ContainsKey("showNotValid") || baseParams.Params["showNotValid"].ToBool();

            return Container.Resolve<IDomainService<ManagingOrganization>>().GetAll()
                .WhereIf(!showNotValid, x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                .Select(x => new
                {
                    x.Id,
                    ContragentName = x.Contragent.Name,
                    ContragentInn = x.Contragent.Inn,
                    ContragentKpp = x.Contragent.Kpp,
                    ContragentOgrn = x.Contragent.Ogrn,
                    Municipality = x.Contragent.Municipality.Name,
                    x.NumberEmployees,
                    x.OfficialSite,
                    x.ActivityGroundsTermination,
                    x.TypeManagement
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParam, this.Container)
                .Order(loadParam)
                .ToList();
        }
    }
}