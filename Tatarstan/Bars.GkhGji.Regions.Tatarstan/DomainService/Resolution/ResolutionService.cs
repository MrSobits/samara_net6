namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Resolution
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.Gkh.Domain;
    using Bars.GkhGji.Enums;

    public class ResolutionService : GkhGji.DomainService.ResolutionService
    {
        /// <inheritdoc />
        public override IDataResult ListView(BaseParams baseParams, bool paging)
        {
            var loadParam = baseParams.GetLoadParam();

            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var realityObjectId = baseParams.Params.GetAsId("realityObjectId");

            var query = this.GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ContragentName,
                    x.TypeExecutant,
                    x.MunicipalityNames,
                    x.ContragentMuName,
                    MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                    PersonInspectionAddress = x.ProtocolViolRoAddresses.Replace(";", ","),
                    x.ContragentMuId,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.OfficialName,
                    x.PenaltyAmount,
                    x.Sanction,
                    x.SumPays,
                    x.InspectionId,
                    x.TypeBase,
                    x.TypeDocumentGji,
                    x.DeliveryDate,
                    x.Paided,
                    x.BecameLegal,
                    x.RoAddress,
                    x.GisUin,
                    x.TypeInitiativeOrg,
                    ProtocolMvdMuName = x.InspectionId.HasValue ? this.ProtocolMvdRealityObjectDomain.GetAll()
                            .Where(y => y.ProtocolMvd.Inspection.Id == x.InspectionId)
                            .Select(y => y.RealityObject.Municipality.Name)
                            .FirstOrDefault()
                        : null
                })
                .Filter(loadParam, this.Container);

            var totalCount = query.Count();

            query = query.Order(loadParam);

            if (paging)
            {
                query = query.Paging(loadParam);
            }

            var data = query
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ContragentName,
                    x.TypeExecutant,
                    MunicipalityId = x.TypeBase == TypeBase.ProtocolMvd
                        ? x.ProtocolMvdMuName
                        : x.MunicipalityId != null
                            ? x.MunicipalityNames
                            : x.ContragentMuName,
                    x.PersonInspectionAddress,
                    MunicipalityNames = x.TypeBase == TypeBase.ProtocolMvd
                        ? x.ProtocolMvdMuName
                        : x.MunicipalityId != null
                            ? x.MunicipalityNames
                            : x.ContragentMuName,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.OfficialName,
                    x.PenaltyAmount,
                    x.Sanction,
                    x.SumPays,
                    x.InspectionId,
                    x.TypeBase,
                    x.TypeDocumentGji,
                    x.DeliveryDate,
                    x.Paided,
                    x.BecameLegal,
                    x.RoAddress,
                    x.GisUin,
                    x.TypeInitiativeOrg
                });

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}