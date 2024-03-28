namespace Bars.GkhDi.Services.Impl
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo;

    using Entities;
    using Gkh.Services.DataContracts;

    public partial class Service
    {
        public GetGenMeetProtocolsResponse GetGenMeetProtocols(string houseId, string periodId)
        {
            var idHouse = houseId.ToLong();
            var idPeriod = periodId.ToLong();

            if (idHouse == 0 || idPeriod == 0)
            {
                return new GetGenMeetProtocolsResponse { Result = Result.DataNotFound };
            }

            var disclosureDomainService = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();
            var documentRealityObjectDomain = this.Container.Resolve<IDomainService<DocumentsRealityObjProtocol>>();
            var manOrgContractRealityObjectDomain = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var disclosureInfoRelationDomain = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();

            using (this.Container.Using(disclosureDomainService, documentRealityObjectDomain, manOrgContractRealityObjectDomain,
                disclosureInfoRelationDomain))
            {
                var diRealObjects = disclosureDomainService.GetAll()
                    .Where(x => x.RealityObject.Id == idHouse && x.PeriodDi.Id == idPeriod);

                if (!diRealObjects.Any())
                {
                    return new GetGenMeetProtocolsResponse { Result = Result.DataNotFound };
                }

                List<ManagingOrgItem> managingOrgItems = null;
                foreach (var diRealObj in diRealObjects)
                {
                    var protocols = documentRealityObjectDomain.GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id && x.Year == diRealObj.PeriodDi.DateEnd.Value.Year && x.File != null)
                        .Select(x => new
                        {
                            x.DocDate,
                            x.DocNum,
                            x.File
                        })
                        .AsEnumerable()
                        .Select(x => new Protocol
                        {
                            IdFile = x.File.Id,
                            NameFile = x.File.FullName,
                            DateProtocol = x.DocDate.ToStr(),
                            NumberProtocol = x.DocNum.ToStr()
                        })
                        .ToArray();

                    var managingOrgItem = new ManagingOrgItem
                    {
                        Protocols = protocols
                    };

                    if (managingOrgItems == null)
                    {
                        managingOrgItems = new List<ManagingOrgItem>();
                    }

                    this.UpdateManagingOrgItemProperties(managingOrgItem,
                        diRealObj,
                        idPeriod,
                        idHouse,
                        manOrgContractRealityObjectDomain,
                        disclosureInfoRelationDomain);
                    managingOrgItems.Add(managingOrgItem);
                }

                return new GetGenMeetProtocolsResponse
                {
                    ManagingOrgItems = managingOrgItems?.ToArray(),
                    Result = Result.NoErrors
                };
            }
        }

        /// <summary>
        /// Обновляет значения свойств переданного экземпляра <see cref="ManagingOrgItem"/>
        /// </summary>
        /// <param name="managingOrgItem">Обновляемый экземпляра <see cref="ManagingOrgItem"/></param>
        /// <param name="diRealObj">Объект недвижимости деятельности управляющей организации в периоде раскрытия информации.</param>
        /// <param name="periodId">Уникальный идентификатор периода.</param>
        /// <param name="houseId">Уникальный идентификатор дома.</param>
        /// <param name="manOrgContractRealityObjectDomain">Домен "Жилой дом договора управляющей организации".</param>
        /// <param name="disclosureInfoRelationDomain">Домен "Связь деятельности УО и деятельности УО в доме".</param>
        private void UpdateManagingOrgItemProperties(
            ManagingOrgItem managingOrgItem,
            DisclosureInfoRealityObj diRealObj,
            long periodId,
            long houseId,
            IDomainService<ManOrgContractRealityObject> manOrgContractRealityObjectDomain,
            IDomainService<DisclosureInfoRelation> disclosureInfoRelationDomain)
        {
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            NumberFormatInfo numberFormat = null;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                numberFormat = ci.NumberFormat;
            }

            this.UpdateManageOrgItemProperties(managingOrgItem,
                diRealObj,
                periodId,
                houseId,
                numberFormat,
                manOrgContractRealityObjectDomain,
                disclosureInfoRelationDomain);
        }
    }
}
