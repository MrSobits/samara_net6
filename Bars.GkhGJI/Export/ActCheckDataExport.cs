namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Entities;
    using Enums;
    using Gkh.Enums;

    public class ActCheckDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var dateStart = baseParams.Params.ContainsKey("dateStart")
                                ? baseParams.Params["dateStart"].To<DateTime>()
                                : DateTime.MinValue;

            var dateEnd = baseParams.Params.ContainsKey("dateEnd")
                                   ? baseParams.Params["dateEnd"].To<DateTime>()
                                   : DateTime.MaxValue;

            var realityObjectId = baseParams.Params.ContainsKey("realityObjectId")
                                   ? baseParams.Params["realityObjectId"].ToLong()
                                   : 0;

            var serviceActCheckRobject = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var serviceDocumentChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            return
                Container.Resolve<IActCheckService>()
                         .GetViewList()
                         .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                         .WhereIf(dateEnd != DateTime.MaxValue, x => x.DocumentDate <= dateEnd)
                         .WhereIf(
                             realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToStr() + "/"))
                         .Filter(loadParam, Container)
                         .Order(loadParam)
                         .Select(
                             x => new
                                 {
                                     x.Id,
                                     x.State,
                                     x.DocumentDate,
                                     x.DocumentNumber,
                                     x.DocumentNum,
                                     MunicipalityId = x.MunicipalityNames,
                                     x.ContragentName,
                                     RealityObjectCount = serviceActCheckRobject.GetAll().Count(y => y.ActCheck.Id == x.Id),
                                     CountExecDoc = serviceDocumentChildren.GetAll()
                                                                           .Count(y => y.Parent.Id == x.Id && (y.Children.TypeDocumentGji == TypeDocumentGji.Protocol || y.Children.TypeDocumentGji == TypeDocumentGji.Prescription)),
                                     HasViolation = serviceActCheckRobject.GetAll().Any(y => y.ActCheck.Id == x.Id && y.HaveViolation == YesNoNotSet.Yes),
                                     x.InspectorNames,
                                     x.InspectionId,
                                     x.TypeBase
                                 })
                         .ToList();
        }
    }
}