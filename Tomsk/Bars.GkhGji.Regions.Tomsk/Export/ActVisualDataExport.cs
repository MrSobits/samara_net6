namespace Bars.GkhGji.Regions.Tomsk.Export
{
    using System;
    using System.Collections;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class ActVisualDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId", 0);

            var dictDocGjiInspector = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                               .GroupBy(x => x.DocumentGji.Id)
                                               .ToDictionary(x => x.Key,
                                                             y => string.Join(", ", y.Select(x => x.Inspector.Fio)));

            return Container.Resolve<IDomainService<ActVisual>>().GetAll()
                .WhereIf(dateStart > DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd > DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObject.Id == realityObjectId)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.Address,
                    InspectorNames = dictDocGjiInspector.ContainsKey(x.Id) ? dictDocGjiInspector[x.Id] : "",
                    x.Inspection.TypeBase,
                    InspectionId = x.Inspection.Id,
                    x.TypeDocumentGji
                })
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }
    }
}
