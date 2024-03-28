namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ResolProsDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            /*
             * параметры:
             * dateStart - период с
             * dateEnd - период по
             * realityObjectId - жилой дом
             */

            var dateStart = baseParams.Params.ContainsKey("dateStart") ? baseParams.Params["dateStart"].ToDateTime() : DateTime.MinValue;
            var dateEnd = baseParams.Params.ContainsKey("dateEnd") ? baseParams.Params["dateEnd"].ToDateTime() : DateTime.MinValue;
            var realityObjectId = baseParams.Params.ContainsKey("realityObjectId") ? baseParams.Params["realityObjectId"].ToLong() : 0;

            List<long> ids = null;

            if (realityObjectId > 0)
            {
                ids = new List<long>();

                ids.AddRange(
                    Container.Resolve<IDomainService<ResolProsRealityObject>>().GetAll()
                    .Where(x => x.RealityObject.Id == realityObjectId)
                    .Select(x => x.ResolPros.Id).ToList());
            }

            var stageId = baseParams.Params.ContainsKey("stageId")
                                   ? baseParams.Params["stageId"].ToLong()
                                   : 0;

            var service = Container.Resolve<IDomainService<ResolPros>>();

            return service.GetAll()
                .WhereIf(ids != null, x => ids.Contains(x.Id))
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(stageId > 0, x => x.Stage.Id == stageId)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.DocumentNumber,
                    x.DocumentDate,
                    Municipality = x.Contragent.Municipality.Name,
                    Executant = x.Executant.Name,
                    InspectionId = x.Inspection.Id,
                    Contragent = x.Contragent.Name,
                    x.PhysicalPerson
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }
    }
}