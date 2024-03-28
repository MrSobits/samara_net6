namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;

    public class PrescriptionDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.ContainsKey("dateStart")
                                   ? baseParams.Params["dateStart"].To<DateTime>()
                                   : DateTime.MinValue;

            var dateEnd = baseParams.Params.ContainsKey("dateEnd")
                                   ? baseParams.Params["dateEnd"].To<DateTime>()
                                   : DateTime.MaxValue;

            var realityObjectId = baseParams.Params.ContainsKey("realityObjectId")
                                   ? baseParams.Params["realityObjectId"].ToLong()
                                   : 0;

            return Container.Resolve<IPrescriptionService>().GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MaxValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Filter(loadParam, Container)
                .Order(loadParam)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.InspectorNames,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.ContragentName,
                    x.TypeExecutant,
                    MunicipalityId = x.MunicipalityNames,
                    x.CountRealityObject,
                    x.CountViolation,
                    x.InspectionId,
                    x.TypeBase,
                    x.DateRemoval,
                    x.AppealDate,
                    x.AppealDescription,
                    x.AppealNumber,
                    x.DisposalId,
                    x.ViolationList
                })
                .ToList();
        }
    }
}