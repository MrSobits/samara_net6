namespace Bars.GkhDi.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    public class TemplateServiceDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var kindTemplateService = baseParams.Params.GetAs<long>("kindTemplateService");
            var isTemplateService = baseParams.Params.GetAs("isTemplateService", false);

            return Container.Resolve<IDomainService<TemplateService>>()
                .GetAll()
                .WhereIf(isTemplateService, x => x.KindServiceDi == kindTemplateService.To<KindServiceDi>())
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.Characteristic,
                    UnitMeasureName = x.UnitMeasure.Name,
                    x.TypeGroupServiceDi,
                    x.KindServiceDi
                 })
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
        }
    }
}