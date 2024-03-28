namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Utils;
    using Entities;
    using Enums;

    public class BaseProsClaimDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            /*
             * параметры:
             * dateStart - период с
             * dateEnd - период по
             */

            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);

            return Container.Resolve<IDomainService<ViewBaseProsClaim>>().GetAll()
                .WhereIf(dateStart != DateTime.MinValue, x => x.ProsClaimDateCheck >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.ProsClaimDateCheck <= dateEnd)
                .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.MunicipalityNames,
                    x.ContragentName,
                    x.ProsClaimDateCheck,
                    x.RealityObjectCount,
                    x.DocumentNumber,
                    x.InspectionNumber,
                    x.InspectorNames,
                    x.PersonInspection,
                    TypeJurPerson = x.PersonInspection == PersonInspection.PhysPerson ? null : x.TypeJurPerson,
                    x.State
                })
                .Filter(loadParam, Container)
                .Select(x => new
                {
                    x.Id,
                    x.Municipality,
                    x.ContragentName,
                    x.ProsClaimDateCheck,
                    x.RealityObjectCount,
                    x.DocumentNumber,
                    x.InspectionNumber,
                    x.InspectorNames,
                    x.PersonInspection,
                    TypeJurPerson = x.TypeJurPerson.HasValue
                        ? x.TypeJurPerson.Value.GetEnumMeta().Display
                        : null,
                    x.State
                })
                .Order(loadParam)
                .ToList();
        }
    }
}