namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    using Bars.B4.DataAccess;

    public class ResolutionDataExport : BaseDataExportService
    {
        public IDomainService<ProtocolMvdRealityObject> ProtocolMvdRealityObjectDomain { get; set; }
        public IRepository<Resolution> ResolutionDomain { get; set; }
        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }

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
            var resolViewService = Container.Resolve<IResolutionService>();


            var predata = resolViewService.GetViewList()
               .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
               .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
               .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
               .ToList();
            var data = predata
                 .Select(x => new
                 {
                     x.Id,
                     x.State,
                     x.ContragentName,
                     x.TypeExecutant,
                     MunicipalityNames = x.TypeBase == TypeBase.ProtocolMvd ? GetProtocolMvdMuName(x.InspectionId.ToLong()) : x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                     MoSettlement = x.MoNames,
                     PlaceName = x.PlaceNames,
                     MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                     x.DocumentDate,
                     DocumentNumber = $"№ {x.DocumentNumber}",
                     x.DocumentNum,
                     x.OfficialName,
                     x.TypeInitiativeOrg,
                     x.PenaltyAmount,
                     x.Sanction,
                     x.SumPays,
                     x.InspectionId,
                     x.TypeBase,
                     x.ConcederationResult,
                     x.TypeTerminationBasement,
                     x.TypeDocumentGji,
                     x.DeliveryDate,
                     x.Paided,
                     ControlType = GetControlType(x.ControlType, x.Id),
                     x.BecameLegal,
                     x.InLawDate,
                     x.DueDate,
                     x.PaymentDate,
                     x.RoAddress,
                     x.ArticleLaw,
                     x.ViolatorFIO,
                     x.ViolatorPosition
                 })
                 .AsQueryable()
                 .Filter(loadParam, Container);


            return data.ToList(); ;
        }

        public virtual string GetProtocolMvdMuName(long? resolInspId)
        {
            if (resolInspId == null)
            {
                return string.Empty;
            }

            return ProtocolMvdRealityObjectDomain.GetAll().Where(x => x.ProtocolMvd.Inspection.Id == resolInspId).Select(x => x.RealityObject.Municipality.Name).FirstOrDefault();
        }

        private ControlType GetControlType(ControlType ctype, long resId)
        {
            if (ctype != ControlType.NotSet)
            {
                return ctype;
            }
            var res = ResolutionDomain.Load(resId);
            if (res.Contragent == null)
            {
                return ControlType.HousingSupervision;
            }
            var lisenses = ManOrgLicenseDomain.GetAll().Where(x => x.Contragent == res.Contragent).ToList();
            if (lisenses.Count == 0)
            {
                return ControlType.HousingSupervision;
            }
            foreach (var license in lisenses)
            {
                if (license.State != null && (license.State.Code == "002" || license.State.Code == "004"))
                {
                    if (license.DateIssued <= res.DocumentDate)
                    {
                        return ControlType.LicensedControl;
                    }
                }
                if (license.State != null && (license.State.Code == "003" || license.State.Code == "005"))
                {
                    if (license.DateIssued <= res.DocumentDate && (license.DateTermination> res.DocumentDate || license.DateValidity> res.DocumentDate))
                    {
                        return ControlType.LicensedControl;
                    }
                }
            }
            return ControlType.HousingSupervision;
        }
    }
}