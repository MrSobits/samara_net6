namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;

    public class DisposalDataExport : BaseDataExportService
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

            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            return Container.Resolve<IDisposalService>().GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.DateEnd,
                    x.DateStart,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.TypeBase,
                    KindCheck = x.KindCheckName,
                    x.ContragentName,
                    MunicipalityId = x.MunicipalityNames,
                    x.IsActCheckExist,
                    x.RealityObjectCount,
                    x.TypeSurveyNames,
                    x.InspectorNames,
                    x.InspectionId,
                    x.TypeDocumentGji,
                    x.TypeAgreementProsecutor,
                    ControlType = x.ControlType != null ? x.ControlType.GetDisplayName() : "",
                    LicenseNumber = x.License != null && x.License.State.FinalState &&
                            (x.License.DateTermination == null || x.License.DateTermination > DateTime.Today)
                                ? x.License.LicNum.ToString()
                                : ""
                })
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }
    }
}