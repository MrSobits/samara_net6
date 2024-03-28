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

    public class BaseInsCheckDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            /*
             * dateStart - период с
             * dateEnd - период по
             * planId - идентификатор плана
             * inspectorIds - идентификаторы инспекторов
             */
            var dateStart = baseParams.Params.ContainsKey("dateStart") ? baseParams.Params["dateStart"].ToDateTime() : DateTime.MinValue;
            var dateEnd = baseParams.Params.ContainsKey("dateEnd") ? baseParams.Params["dateEnd"].ToDateTime() : DateTime.MinValue;
            var planId = baseParams.Params.ContainsKey("planId") ? baseParams.Params["planId"].ToLong() : 0;

            var inspectorIds = baseParams.Params.ContainsKey("inspectorIds") ? baseParams.Params["inspectorIds"].ToString() : string.Empty;

            List<long> baseInsCheckIds = null;

            if (!string.IsNullOrEmpty(inspectorIds))
            {
                var inspIds = inspectorIds.Split(';').Select(x => x.ToLong()).ToList();

                baseInsCheckIds = Container.Resolve<IDomainService<InspectionGjiInspector>>().GetAll()
                    .Where(x => inspIds.Contains(x.Inspector.Id))
                    .Select(x => x.Inspection.Id)
                    .Distinct()
                    .ToList();
            }

            return Container.Resolve<IDomainService<ViewBaseInsCheck>>().GetAll()
                .WhereIf(baseInsCheckIds != null, x => baseInsCheckIds.Contains(x.Id))
                .WhereIf(planId > 0, x => x.PlanId == planId)
                .WhereIf(dateStart != DateTime.MinValue, x => x.InsCheckDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.InsCheckDate <= dateEnd)
                .Select(x => new
                {
                    x.Id,
                    x.InspectionNumber,
                    Municipality = x.MunicipalityNames,
                    Address = x.RealityObjectAddress,
                    Plan = x.PlanName,
                    x.ContragentName,
                    x.InsCheckDate,
                    x.DisposalNumber,
                    x.InspectorNames,
                    x.TypeFact
                })
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }
    }
}