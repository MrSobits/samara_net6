namespace Bars.GkhGji.Regions.Tomsk.Export
{
    using System;
    using System.Collections;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class AdminCaseDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId", 0);

            return Container.Resolve<IDomainService<AdministrativeCase>>().GetAll()
                .WhereIf(realityObjectId > 0, x => x.RealityObject.Id == realityObjectId)
                .WhereIf(dateStart > DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd > DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.DocumentNumber,
                    x.DocumentDate,
                    Municipality = x.RealityObject.Municipality.Name,
                    RealityObject = x.RealityObject.Address,
                    Inspector = x.Inspector.Fio,
                    x.Inspection.TypeBase,
                    InspectionId = x.Inspection.Id,
                    x.TypeDocumentGji

                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }
    }
}
