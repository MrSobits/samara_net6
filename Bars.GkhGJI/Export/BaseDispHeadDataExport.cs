namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class BaseDispHeadDataExport : BaseDataExportService
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

            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            return Container.Resolve<IDomainService<ViewBaseDispHead>>().GetAll()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DispHeadDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DispHeadDate <= dateEnd)
                .WhereIf(realityObjectId > 0, y => Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                    .Any(x => x.RealityObject.Id == realityObjectId && x.Inspection.Id == y.Id))
                .Select(x => new
                {
                    x.Id,
                    x.MunicipalityNames,
                    x.ContragentName,
                    x.DispHeadDate,
                    x.RealityObjectCount,
                    Head = x.HeadFio,
                    x.InspectorNames,
                    x.InspectionNumber,
                    DispHeadNumber = x.DocumentNumber,
                    x.DisposalTypeSurveys,
                    x.PersonInspection,
                    TypeJurPerson = x.PersonInspection == PersonInspection.PhysPerson ? null : x.TypeJurPerson,
                    x.State
                })
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }
    }
}