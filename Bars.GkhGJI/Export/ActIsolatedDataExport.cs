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

    /// <summary>
    /// Экспорт Актов без взаимодействия
    /// </summary>
    public class ActIsolatedDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);

            var dateStart = baseParams.Params.ContainsKey("dateStart")
                                ? baseParams.Params["dateStart"].To<DateTime>()
                                : DateTime.MinValue;

            var dateEnd = baseParams.Params.ContainsKey("dateEnd")
                                   ? baseParams.Params["dateEnd"].To<DateTime>()
                                   : DateTime.MaxValue;

            var realityObjectId = baseParams.Params.ContainsKey("realityObjectId")
                                   ? baseParams.Params["realityObjectId"].ToLong()
                                   : 0;

            var serviceActIsolatedRobject = this.Container.Resolve<IDomainService<ActIsolatedRealObj>>();
            var serviceDocumentChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();

            return this.Container.Resolve<IActIsolatedService>()
                .GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MaxValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToStr() + "/"))
                .Filter(loadParam, this.Container)
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
                            RealityObjectCount = serviceActIsolatedRobject.GetAll().Count(y => y.ActIsolated.Id == x.Id),
                            CountExecDoc = serviceDocumentChildren.GetAll()
                                                                .Count(y => y.Parent.Id == x.Id && (y.Children.TypeDocumentGji == TypeDocumentGji.Protocol || y.Children.TypeDocumentGji == TypeDocumentGji.Prescription)),
                            HasViolation = serviceActIsolatedRobject.GetAll().Any(y => y.ActIsolated.Id == x.Id && y.HaveViolation == YesNoNotSet.Yes),
                            x.InspectorNames,
                            x.InspectionId,
                            x.TypeBase
                        })
                .ToList();
        }
    }
}