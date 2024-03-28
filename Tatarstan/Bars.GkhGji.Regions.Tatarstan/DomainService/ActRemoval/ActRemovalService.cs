namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActRemoval
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.Gkh.Utils;

    public class ActRemovalService : GkhGji.DomainService.ActRemovalService
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
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MaxValue);
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var isExport = baseParams.Params.GetAs<bool>("isExport");

            return this.GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MaxValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ParentContragentMuName,
                    MoSettlement = x.MoNames,
                    PlaceName = x.PlaceNames,
                    MunicipalityId = x.MunicipalityId ?? x.ParentContragentMuId,
                    PersonInspectionAddress = x.RealityObjectAddresses.Replace(";", ","),
                    ParentContragentName = x.ParentContragent,
                    x.CountExecDoc,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.InspectorNames,
                    x.RealityObjectCount,
                    x.ParentDocumentName,
                    x.TypeRemoval,
                    x.InspectionId,
                    x.TypeBase,
                    x.TypeDocumentGji
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container, usePaging: !isExport);
        }

        /// <inheritdoc />
        protected override string GetDataExportRegistrationName() => "Tat ActRemovalDataExport";
    }
}