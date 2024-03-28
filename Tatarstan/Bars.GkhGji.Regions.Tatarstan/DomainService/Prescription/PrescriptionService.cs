namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Prescription
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    public class PrescriptionService : GkhGji.DomainService.PrescriptionService
    {
        public override IDataResult ListView(BaseParams baseParams)
        {
            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObjectId = baseParams.Params.GetAsId("realityObjectId");
            var isExport = baseParams.Params.GetAs<bool>("isExport");

            return this.GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
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
                    MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                    MoSettlement = x.MoNames,
                    PlaceName = x.PlaceNames,
                    MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                    PersonInspectionAddress = x.RealityObjectAddresses.Replace(";", ","),
                    x.CountRealityObject,
                    x.CountViolation,
                    x.InspectionId,
                    x.TypeBase,
                    x.TypeDocumentGji,
                    x.DateRemoval,
                    x.DisposalId
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container, usePaging: !isExport);
        }

        /// <inheritdoc />
        protected override string GetDataExportRegistrationName() => "Tat PrescriptionDataExport";
    }
}