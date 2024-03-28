namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.Gkh.Domain;

    using Gkh.Authentification;
    using Entities;
    using Enums;
    using Utils;

    using Castle.Windsor;

    public class ResolutionService : IResolutionService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ProtocolMvdRealityObject> ProtocolMvdRealityObjectDomain { get; set; }
        
        public virtual string GetTakingDecisionAuthorityName()
        {
            return "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ";
        }
        
        public IDataResult GetInfo(long? documentId)
        {
            var serviceDocChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                var baseName = "";

                // Пробегаемся по документам на основе которого создано постановление
                var parents = serviceDocChildren.GetAll()
                    .Where(x => x.Children.Id == documentId)
                    .Select(x => new
                    {
                        parentId = x.Parent.Id,
                        x.Parent.TypeDocumentGji,
                        x.Parent.DocumentDate,
                        x.Parent.DocumentNumber
                    })
                    .ToList();

                foreach (var doc in parents)
                {
                    var docName = Utils.GetDocumentName(doc.TypeDocumentGji);

                    if (!string.IsNullOrEmpty(baseName))
                    {
                        baseName += ", ";
                    }

                    baseName += string.Format("{0} №{1} от {2}", docName, doc.DocumentNumber, doc.DocumentDate.ToDateTime().ToShortDateString());
                }

                return new BaseDataResult(new { success = true, baseName });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new { success = false, message = e.Message });
            }
            finally
            {
                this.Container.Release(serviceDocChildren);
            }
        }

        /// <inheritdoc />
        public virtual IDataResult ListView(BaseParams baseParams, bool paging)
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

        public virtual string GetProtocolMvdMuName(long? resolInspId)
        {
            if (resolInspId == null)
            {
                return string.Empty;
            }

            return this.ProtocolMvdRealityObjectDomain.GetAll().Where(x => x.ProtocolMvd.Inspection.Id == resolInspId).Select(x => x.RealityObject.Municipality.Name).FirstOrDefault();
        }
        
        public IQueryable<ViewResolution> GetViewList()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceViewResolution = this.Container.Resolve<IDomainService<ViewResolution>>();

            try
            {
                var municipalityList = userManager.GetMunicipalityIds();

                return serviceViewResolution.GetAll()
                    .WhereIf(municipalityList.Count > 0, x => x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value));
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(serviceViewResolution);
            }
        }

        public IDataResult GetResolutionInfo(BaseParams baseParams)
        {
#warning переделать получение агрегированной информации
            var serviceResolution = this.Container.Resolve<IDomainService<Resolution>>();
            var serviceResolutionPayFine = this.Container.Resolve<IDomainService<ResolutionPayFine>>();

            try
            {
                var listResAnonymObj = new List<object>();

                var resolutionIds = baseParams.Params.GetAs<long[]>("objectIds");
                var resolutionList = serviceResolution.GetAll()
                    .Where(x => resolutionIds.Contains(x.Id))
                    .ToList();

                foreach (var resolutionObj in resolutionList)
                {
                    var protocolViolationsCount = this.GetCountProtocolViolationsByResolution(resolutionObj);

                    var payFineResolutionDate = serviceResolutionPayFine.GetAll()
                        .Where(x => x.Resolution.Id == resolutionObj.Id)
                        .Max(x => x.DocumentDate);

                    listResAnonymObj.Add(new
                    {
                        resolutionObj.DocumentDate,
                        resolutionObj.PenaltyAmount,
                        SupervisoryOrgCode = resolutionObj.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection ? "1" : "6",
                        ProtocolViolationsCount = protocolViolationsCount,
                        PayFineResolutionDate = payFineResolutionDate
                    });
                }

                return new BaseDataResult(listResAnonymObj);
            }
            finally
            {
                this.Container.Release(serviceResolution);
                this.Container.Release(serviceResolutionPayFine);
            }
        }

        private int GetCountProtocolViolationsByResolution(Resolution resolution)
        {
            var serviceDocumentGjiChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceProtocolGjiIViolation = this.Container.Resolve<IDomainService<ProtocolViolation>>();

            try
            {
                var protocol = serviceDocumentGjiChildren
                    .GetAll()
                    .Where(x => x.Children.Id == resolution.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .Select(x => x.Parent)
                    .ToList()
                    .FirstOrDefault()
                    .To<Protocol>();
            
                return serviceProtocolGjiIViolation
                    .GetAll()
                    .Where(x => x.Document.Id == protocol.Id)
                    .Select(x => x.InspectionViolation.Id)
                    .Distinct()
                    .Count();
            }
            finally
            {
                this.Container.Release(serviceDocumentGjiChildren);
                this.Container.Release(serviceProtocolGjiIViolation);
            }
        }
    }
}