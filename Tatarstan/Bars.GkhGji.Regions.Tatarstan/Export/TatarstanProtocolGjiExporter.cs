namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiExporter : BaseDataExportService
    {
        /// <inheritdoc />
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ITatarstanProtocolGjiService>();
            var domain = this.Container.ResolveDomain<TatarstanProtocolGji>();
            using (this.Container.Using(service, domain))
            {
                return service.GetListResult(domain, baseParams)
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.DocumentNumber,
                        x.DocumentDate,
                        GisUin = "'" + x.GisUin,
                        x.PenaltyAmount,
                        x.Executant,
                        x.MunicipalityName,
                        x.Inspectors
                    }).ToList();
            }
        }
    }
}
   