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

    public class BaseJurPersonDataExport : BaseDataExportService
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
            var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);
            var inspectorIds = baseParams.Params.GetAs<long[]>("inspectorIds");

            var serviceInspInspector = Container.Resolve<IDomainService<InspectionGjiInspector>>();

            return Container.Resolve<IDomainService<ViewBaseJurPerson>>().GetAll()
                .WhereIf(inspectorIds != null && inspectorIds.Length > 0, y => serviceInspInspector.GetAll().Any(x => x.Inspection.Id == y.Id && inspectorIds.Contains(x.Inspector.Id)))
                .WhereIf(planId > 0, x => x.PlanId == planId)
                .WhereIf(dateStart != DateTime.MinValue, x => x.DateStart >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DateStart <= dateEnd)
                .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.MunicipalityNames,
                    Contragent = x.ContragentName,
                    Plan = x.PlanName,
                    x.InspectionNumber,
                    x.DateStart,
                    x.CountDays,
                    x.RealityObjectCount,
                    x.DisposalNumber,
                    x.InspectorNames,
                    x.TypeFact,
                    x.State
                })
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }
    }
}