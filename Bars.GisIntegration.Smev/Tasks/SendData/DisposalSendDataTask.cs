namespace Bars.GisIntegration.Smev.Tasks.SendData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.SendData;
    using Bars.GisIntegration.Smev.SmevExchangeService.Erp;
    using Bars.GisIntegration.Smev.Tasks.SendData.Base;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    using Fasterflect;

    public class DisposalSendDataTask : ErpSendDataTask<MessageFromErpSetType>
    {
        /// <inheritdoc />
        protected override PackageProcessingResult ProcessSmevResponse(MessageFromErpSetType response, Dictionary<Type, Dictionary<string, long>> transportGuidDictByType)
        {
            if (!response.Items?.Any() ?? true)
            {
                throw new ArgumentException();
            }

            var hasError = response.Errors?.Any() ?? false;

            if (!hasError)
            {
                var disposalDomain = this.Container.ResolveDomain<TatarstanDisposal>();

                using (this.Container.Using(disposalDomain))
                {
                    var objectId = transportGuidDictByType.Get(typeof(TatarstanDisposal)).Get("Id");
                    var disposal = disposalDomain.Get(objectId);

                    if (disposal is null)
                    {
                        throw new NullReferenceException("Не найден объект в базе данных");
                    }

                    foreach (var inspectionResponse in response.Items)
                    {
                        switch (inspectionResponse)
                        {
                            case UpdateInspectionResponseType updateInspectionResponseType:
                                if ((updateInspectionResponseType.Errors?.Any() ?? false) || updateInspectionResponseType.RequestStatus?.Value != "SUCCESS")
                                {
                                    hasError = true;
                                    continue;
                                }

                                this.UpdateGuid(updateInspectionResponseType.Item?.TryGetPropertyValue("numGuid")?.ToString() ??
                                    updateInspectionResponseType.RequestStatus?.guid);
                                break;
                            case CreateInspectionResponseType createInspectionResponseType:
                                if (string.IsNullOrEmpty(disposal.ErpGuid))
                                {
                                    disposal.ErpRegistrationNumber = createInspectionResponseType.Inspection.erpId;
                                    disposal.ErpId = createInspectionResponseType.Inspection.guid;
                                    disposal.ErpGuid = createInspectionResponseType.Inspection.guid;
                                    disposal.ErpRegistrationDate = createInspectionResponseType.Inspection.dateOfCreation;

                                    disposalDomain.Update(disposal);
                                }

                                this.UpdateErpGuids(createInspectionResponseType, disposal);
                                break;
                            default:
                                continue;
                        }
                    }
                }
            }

            return new PackageProcessingResult
            {
                State = hasError
                    ? PackageState.ProcessedWithErrors
                    : PackageState.SuccessProcessed,
                Objects = new List<ObjectProcessingResult>()
            };
        }

        private void UpdateErpGuids(CreateInspectionResponseType createInspectionResponse, TatarstanDisposal disposal)
        {
            var baseJurPersonContragentDomain = this.Container.ResolveDomain<BaseJurPersonContragent>();
            var inspectionContragentDomain = this.Container.ResolveDomain<InspectionBaseContragent>();
            var contragentDomain = this.Container.ResolveDomain<Contragent>();
            
            if (createInspectionResponse.Inspection?.IObject?.Any() ?? false)
            {
                foreach (var iObject in createInspectionResponse.Inspection.IObject)
                {
                    this.UpdateGuid(iObject.numGuid);

                    if (!(iObject.IResult is null))
                    {
                        this.UpdateGuid(iObject.IResult.numGuid);
                    }
                    if (iObject.IResult?.IResultInspector?.Any() ?? false)
                    {
                        foreach (var iResultInspector in iObject.IResult.IResultInspector)
                        {
                            this.UpdateGuid(iResultInspector.numGuid);
                        }
                    }
                    if (iObject.IResult?.IViolation?.Any() ?? false)
                    {
                        foreach (var iViolation in iObject.IResult.IViolation)
                        {
                            this.UpdateGuid(iViolation.numGuid);

                            if (iViolation.VInjunction?.Any() ?? false)
                            {
                                foreach (var vInjunction in iViolation.VInjunction)
                                {
                                    this.UpdateGuid(vInjunction.numGuid);
                                }
                            }
                        }
                    }
                    if (iObject.IResult?.IResultInformation?.Any() ?? false)
                    {
                        foreach (var iResultInformation in iObject.IResult.IResultInformation)
                        {
                            this.UpdateGuid(iResultInformation.numGuid);
                        }
                    }
                    if (iObject.IResult?.IResultRepresentative?.Any() ?? false)
                    {
                        foreach (var iResultRepresentative in iObject.IResult.IResultRepresentative)
                        {
                            this.UpdateGuid(iResultRepresentative.numGuid);
                        }
                    }
                }
            }

            if (createInspectionResponse.Inspection?.IAuthority?.IInspector?.Any() ?? false)
            {
                foreach (var inspector in createInspectionResponse.Inspection.IAuthority.IInspector)
                {
                    this.UpdateGuid(inspector.numGuid);
                }
            }

            using (this.Container.Using(baseJurPersonContragentDomain, inspectionContragentDomain, contragentDomain))
            {
                if (createInspectionResponse.Inspection?.IAuthority?.IAuthorityJointlyFrgu?.Any() ?? false)
                {
                    foreach (var authorityJointlyFrgu in createInspectionResponse.Inspection.IAuthority.IAuthorityJointlyFrgu)
                    {
                        var contragentGuid = authorityJointlyFrgu.frguJointlyGuid;

                        var jurContragents = baseJurPersonContragentDomain.GetAll()
                            .Where(x => x.BaseJurPerson.Id == disposal.Inspection.Id)
                            .Select(x => x.Contragent)
                            .ToList();

                        var inspContragents = inspectionContragentDomain.GetAll()
                            .Where(x => x.InspectionGji.Id == disposal.Inspection.Id)
                            .Select(x => x.Contragent)
                            .ToList();

                        Contragent contragent = null;

                        if (jurContragents.Count == 1 && inspContragents.Count == 0)
                        {
                            contragent = jurContragents.First();
                        }
                        else if (jurContragents.Count == 0 && inspContragents.Count == 1)
                        {
                            contragent = inspContragents.First();
                        }

                        if (contragent != null
                            && !string.IsNullOrWhiteSpace(contragentGuid)
                            && !string.Equals(contragent.ErpGuid, contragentGuid))
                        {
                            contragent.ErpGuid = contragentGuid;
                            contragentDomain.Update(contragent);
                        }
                    }
                }
            }

            if (createInspectionResponse.Inspection?.IClassification?.IClassificationLb?.Any() ?? false)
            {
                foreach (var iClassificationLb in createInspectionResponse.Inspection.IClassification.IClassificationLb)
                {
                    this.UpdateGuid(iClassificationLb.numGuid);
                }
            }
        }
    }
}