namespace Bars.Gkh.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class BuilderDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            /*
             * Галочка над гридом показывать не действующие организации
             */
            var showNotValid = !baseParams.Params.ContainsKey("showNotValid") || baseParams.Params["showNotValid"].ToBool();

            return this.Container.Resolve<IDomainService<Builder>>().GetAll()
                .WhereIf(!showNotValid, x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Contragent.Municipality.Name,
                    ContragentName = x.Contragent.Name,
                    x.Contragent.Inn,
                    x.AdvancedTechnologies,
                    x.ConsentInfo,
                    x.WorkWithoutContractor,
                    x.Rating,
                    x.ActivityGroundsTermination,
                    x.File
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParam, this.Container)
                .Order(loadParam)
                .ToList();
        }
    }
}