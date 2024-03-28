namespace Bars.GisIntegration.Smev.Tasks.PrepareData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.GisIntegration.Smev.ConfigSections;
    using Bars.GisIntegration.Smev.Dto;
    using Bars.GisIntegration.Smev.SmevExchangeService.ERKNM;
    using Bars.GisIntegration.Smev.Tasks.PrepareData.Base;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmCharacters;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    using Castle.Core.Internal;

    using Fasterflect;

    public class KnmPrepareDataTask : ErknmPrepareDataTask<LetterToErknmType>
    {
        private readonly TypeCheck[] plannedTypeCheckList = { TypeCheck.PlannedExit, TypeCheck.PlannedDocumentation, TypeCheck.PlannedInspectionVisit };
        private readonly TypeCheck[] exitTypeCheckList = { TypeCheck.PlannedExit, TypeCheck.NotPlannedExit };
        private readonly TypeCheck[] docTypeCheckList = { TypeCheck.PlannedDocumentation, TypeCheck.NotPlannedDocumentation };
        private readonly TypeCheck[] reasonDocumentsTypeCheckList = { TypeCheck.NotPlannedExit, TypeCheck.NotPlannedDocumentation, TypeCheck.NotPlannedInspectionVisit };
        private readonly TypeAgreementProsecutor[] typeAgreementProsecutor = { TypeAgreementProsecutor.ImmediateInspection, TypeAgreementProsecutor.RequiresAgreement};
        private readonly TypeFormInspection[] typeFormList = { TypeFormInspection.InspectionVisit, TypeFormInspection.Exit };
        private InspectionGjiRealityObject[] realityObjects;
        private DocumentGjiInspector[] inspectors;
        private DisposalExpert[] experts;
        private ActCheckAction[] events;
        private DisposalProvidedDoc[] organizationDocuments;
        private DecisionInspectionBase[] reasons;
        private Dictionary<long, ControlObjectKind> decisionControlObjectInfoDict;
        private string planGuid;
        private string typeId;
        private string noticeMethodId;
        private string kindDictVersionId;
        private string riskCategoryDictVersionId;
        private string approveRequiredId;
        private InspectionDto inspectionDto;
        private ErknmIntegrationConfig erknmIntegrationConfig;
        private TatDisposalAnnex[] documents;
        private KnmReason[] reasonDocuments;

        private Decision Decision { get; set; }
        
        private LetterToErknmType RequestObject { get; set; }

        private long ObjectId { get; set; }
        
        /// <inheritdoc />
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var id = parameters.GetAsId();
            this.ObjectId = id;
            this.RequestObject = this.GetData();
        }

        /// <inheritdoc />
        protected override List<ValidateObjectResult> ValidateData()
        {
            var errors = new List<string>();

            if (this.typeId.IsNullOrEmpty())
            {
                errors.Add("Указанный вид проверки отсутствует в справочнике \"Характеры КНМ\"");
            }

            if (this.plannedTypeCheckList.Contains(this.Decision.KindCheck.Code) && this.planGuid.IsNullOrEmpty())
            {
                errors.Add("Не заполнен идентификатор плана");
            }

            if (errors.Any())
            {
                throw new Exception(string.Join("; ", errors));
            }
            
            var validateResult = new ValidateObjectResult
            {
                Message = string.Empty,
                State = ObjectValidateState.Success
            };

            return new List<ValidateObjectResult> { validateResult };
        }

        /// <inheritdoc />
        protected override LetterToErknmType GetRequestObject(ref bool isTestMessage, out long objectId)
        {
            objectId = this.ObjectId;
            return this.RequestObject;
        }

        private LetterToErknmType GetData()
        {
            this.InitData();

            var isExitTypeCheck = this.exitTypeCheckList.Contains(this.Decision.KindCheck.Code);
            var isDocTypeCheck = this.docTypeCheckList.Contains(this.Decision.KindCheck.Code);
            var isRemoteTypeForm = this.typeFormList.Contains(this.inspectionDto.TypeForm);
            var isRequiredReasonDocuments = this.reasonDocumentsTypeCheckList.Contains(this.Decision.KindCheck.Code) && this.typeAgreementProsecutor.Contains(this.Decision.TypeAgreementProsecutor) ;
            
            var result = new LetterToErknmType
            {
                Item = new LetterToErknmTypeSet
                {
                    Items = new object[]{ new CreateInspectionRequestType
                    {
                        Inspection  = new InspectionCreate
                        {
                            planGuid = this.planGuid,
                            guid = this.GetErknmGuid(this.Decision, typeof(Decision)),
                            typeId = this.typeId,
                            startDate = (DateTime) this.Decision.DateStart,
                            stopDate = this.Decision.DateEnd ?? DateTime.MinValue,
                            stopDateSpecified = this.Decision.DateEnd.HasValue,
                            prosecutorOrganizationId = "1267",
                            noticeMethodId = isExitTypeCheck ? this.noticeMethodId : null,
                            noticeDate = isExitTypeCheck ? this.Decision.NcDate ?? DateTime.MinValue : DateTime.MinValue,
                            noticeDateSpecified = isExitTypeCheck && this.Decision.NcDate.HasValue,
                            districtId = "1033920000000001",
                            withQRCode = true,
                            withQRCodeSpecified = true,
                            
                            documents = this.documents.Select(x=> new IDocument
                            {
                                documentTypeId = x.ErknmTypeDocument?.Code,
                                guid = this.GetErknmGuid(x, x.GetType()),
                                description = x.Description,
                                attachments = x.File != null ? this.InitAttachment(x, x.File, x.GetType()) : null
                            }).ToArray(),

                            Item = new I
                             {
                                 durationDays = this.Decision.CountDays.HasValue ? this.Decision.CountDays.ToString() : null,
                                 durationHours = this.Decision.CountHours.HasValue ? this.Decision.CountHours.ToString() : null,
                                 
                                 dataContent = this.Decision.TypeAgreementProsecutor == TypeAgreementProsecutor.ImmediateInspection 
                                     ? this.Decision.InformationAboutHarm 
                                     : null,

                                 isRemote = isRemoteTypeForm && this.Decision.UsingMeansRemoteInteraction == YesNoNotSet.Yes,
                                 isRemoteSpecified = isRemoteTypeForm && this.Decision.UsingMeansRemoteInteraction != YesNoNotSet.NotSet,
                                 noteRemote = isRemoteTypeForm && !string.IsNullOrEmpty(this.Decision.InfoUsingMeansRemoteInteraction) 
                                     ? this.Decision.InfoUsingMeansRemoteInteraction 
                                     : null,
                                 
                                 documentRequestDate = isDocTypeCheck
                                     ? this.Decision.SubmissionDate ?? DateTime.MinValue
                                     : DateTime.MinValue,
                                 
                                 documentRequestDateSpecified = isDocTypeCheck && this.Decision.SubmissionDate.HasValue,
                                 
                                 documentResponseDate = isDocTypeCheck
                                     ? this.Decision.ReceiptDate ?? DateTime.MinValue
                                     : DateTime.MinValue,
                                 
                                 documentResponseDateSpecified = isDocTypeCheck && this.Decision.ReceiptDate.HasValue,
                                 
                                 knoOrganization = new IDictionary
                                 {
                                     dictId = this.erknmIntegrationConfig.ControlOrganizationId,
                                     dictVersionId = this.erknmIntegrationConfig.SupervisoryId
                                 },
                                 
                                 decision = new IDecision
                                 {
                                     dateTimeDecision = this.Decision.DocumentDate.HasValue 
                                         ? this.Decision.DocumentTime.HasValue 
                                             ? this.Decision.DocumentDate.Value.Add(this.Decision.DocumentTime.Value.TimeOfDay) 
                                             : this.Decision.DocumentDate.Value
                                         : DateTime.MinValue,
                                     dateTimeDecisionSpecified = this.Decision.DocumentDate.HasValue,
                                     numberDecision = this.Decision.DocumentNumber,
                                     placeDecision = this.Decision.DecisionPlace?.AddressName,
                                     fioSigner = this.Decision.IssuedDisposal?.Fio,
                                     titleSigner = new IDictionary
                                     {
                                         dictId = this.erknmIntegrationConfig.SignerPostId,
                                         dictVersionId = this.GetInspectorPositionDictId(this.Decision.IssuedDisposal, true)
                                     }
                                 },
                                 
                                 kindControl = new IDictionary
                                 {
                                     dictId = this.erknmIntegrationConfig.ControlTypeId,
                                     dictVersionId = this.Decision.ControlType?.ErvkId
                                 },
                                 
                                 kind = new IDictionary
                                 {
                                     dictId = this.erknmIntegrationConfig.KnmTypeId,
                                     dictVersionId = this.kindDictVersionId
                                 },
                                 
                                 reasonDocuments = isRequiredReasonDocuments 
                                     ? this.reasonDocuments.Select(x=> 
                                         new IDocument 
                                         {
                                             documentTypeId = x.ErknmTypeDocument?.Code,
                                             guid = this.GetErknmGuid(x, typeof(KnmReason)),
                                             description = x.Description,
                                             attachments = x.File != null ? this.InitAttachment(x, x.File, x.GetType()) : null
                                         }).ToArray()
                                     : null,
                                 
                                 reasons = this.reasons.Select(x=> new ReasonWithRisk
                                 {
                                     numGuid = this.GetErknmGuid(x, typeof(DecisionInspectionBase)),
                                     reason = new IReason
                                     {
                                         reasonTypeId = x.InspectionBaseType?.ErknmCode,
                                         date = x.FoundationDate ?? DateTime.MinValue,
                                         dateSpecified = x.FoundationDate.HasValue,
                                         note = !x.OtherInspBaseType.IsNullOrEmpty() ? x.OtherInspBaseType : null
                                     },
                                     riskIndikators = x.RiskIndicator != null ? new IDictionary
                                     {
                                         dictId = this.erknmIntegrationConfig.RiskIndicatorId,
                                         dictVersionId = x.RiskIndicator?.ErvkId
                                     } : null
                                 }).ToArray(),
                                 
                                 approve = this.Decision.TypeAgreementProsecutor != TypeAgreementProsecutor.NotSet ? new IApprove
                                 {
                                     approveRequiredId = this.approveRequiredId
                                 } : null,

                                 organizations = new []
                                 {
                                     new ISubject
                                     {
                                         inn = this.inspectionDto.OrganizationInn,
                                         organizationName = this.inspectionDto.OrganizationName,
                                         isFiz = this.inspectionDto.PersonInspection == PersonInspection.PhysPerson,
                                         isFizSpecified = true,
                                         guid = this.GetErknmGuid(this.Decision, typeof(Decision), "OrganizationErknmGuid")
                                     }
                                 },
                                     
                                 objects = this.realityObjects
                                     .Where(x => this.decisionControlObjectInfoDict.ContainsKey(x.Id) && !this.riskCategoryDictVersionId.IsNullOrEmpty())
                                     .Select(x=> new IObject
                                 {
                                     address = x.RealityObject.Address,
                                     guid = this.GetErknmGuid(x,typeof(InspectionGjiRealityObject)),
                                     houseGuid = x.RealityObject.HouseGuid,
                                     objectType = new IDictionary
                                     {
                                         dictId = this.erknmIntegrationConfig.ControlObjectTypeId,
                                         dictVersionId = this.decisionControlObjectInfoDict[x.Id].ControlObjectType?.ErvkId
                                     },
                                     objectKind = new IDictionary
                                     {
                                         dictId = this.erknmIntegrationConfig.ControlObjectKindId,
                                         dictVersionId = this.decisionControlObjectInfoDict[x.Id].ErvkId
                                     },
                                     riskCategory = new IDictionary
                                     {
                                         dictId = this.erknmIntegrationConfig.RiskCategoryId,
                                         dictVersionId = this.riskCategoryDictVersionId
                                     }
                                 }).ToArray(),
                                 
                                 inspectors = this.inspectors.Select(x=> new IInspector
                                 {
                                     fullName = x.Inspector.Fio,
                                     guid = this.GetErknmGuid(x, typeof(DocumentGjiInspector)),
                                     position = new IDictionary
                                     {
                                         dictId = this.erknmIntegrationConfig.InspectorPositionId,
                                         dictVersionId = this.GetInspectorPositionDictId(x.Inspector)
                                     }
                                 }).ToArray(),
                                 
                                 experts = this.experts.Select(x => new IExpert
                                     {
                                         title = x.Expert.Name,
                                         typeId = x.Expert.ExpertType.HasValue ? ((int) x.Expert.ExpertType).ToString(): null,
                                         guid = this.GetErknmGuid(x, typeof(DisposalExpert))
                                     }
                                 ).ToArray(),
                                 
                                 events = this.events.Select(x=> new IEvent
                                 {
                                     guid = this.GetErknmGuid(x, typeof(ActCheckAction)),
                                     startDate = x.StartDate ?? DateTime.MinValue,
                                     startDateSpecified = x.StartDate.HasValue,
                                     stopDate = x.EndDate ?? DateTime.MinValue,
                                     stopDateSpecified = x.EndDate.HasValue,
                                     @event = new IDictionary
                                     {
                                         dictId = this.erknmIntegrationConfig.KnmActionId,
                                         dictVersionId = this.GetEventsDictId(x.ActionType)
                                     }
                                 }).ToArray(),
                                 
                                 organizationDocuments = this.organizationDocuments.Select(x=> new IString
                                 {
                                     value = x.ProvidedDoc.Name,
                                     guid = this.GetErknmGuid(x, typeof(DisposalProvidedDoc)),
                                 }).ToArray()
                             }
                        } 
                    }
                    }
                }
            };
            
            this.SaveErknmEntities();
            
            return result;
        }

        private void InitData()
        {
            var decisionDomain = this.Container.ResolveDomain<Decision>();
            var inspectionGjiRealityObjectDomain = this.Container.ResolveDomain<InspectionGjiRealityObject>();
            var documentGjiInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var disposalExpertDomain = this.Container.ResolveDomain<DisposalExpert>();
            var disposalProvidedDocDomain = this.Container.ResolveDomain<DisposalProvidedDoc>();
            var knmCharacterKindCheckDomain = this.Container.ResolveDomain<KnmCharacterKindCheck>();
            var baseJurPersonDomain = this.Container.ResolveDomain<BaseJurPerson>();
            var knmTypeKindCheckDomain = this.Container.ResolveDomain<KnmTypeKindCheck>();
            var actCheckActionDomain = this.Container.ResolveDomain<ActCheckAction>();
            var documentGjiChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var decisionInspectionBaseDomain = this.Container.ResolveDomain<DecisionInspectionBase>();
            var decisionControlObjectInfoDomain = this.Container.ResolveDomain<DecisionControlObjectInfo>();
            var tatRiskCategoryDomain = this.Container.ResolveDomain<TatRiskCategory>();
            var inspectionRiskDomain = this.Container.ResolveDomain<InspectionRisk>();
            var disposalAnnexDomain = this.Container.ResolveDomain<TatDisposalAnnex>();
            var knmReasonDomain = this.Container.ResolveDomain<KnmReason>();
            
            this.erknmIntegrationConfig = this.Container.GetGkhConfig<ErknmIntegrationConfig>();

            using (this.Container.Using(
                decisionDomain, 
                inspectionGjiRealityObjectDomain, 
                documentGjiInspectorDomain,
                disposalExpertDomain,
                disposalProvidedDocDomain,
                knmCharacterKindCheckDomain,
                knmTypeKindCheckDomain,
                actCheckActionDomain,
                documentGjiChildrenDomain,
                decisionInspectionBaseDomain,
                decisionControlObjectInfoDomain,
                tatRiskCategoryDomain,
                inspectionRiskDomain,
                disposalAnnexDomain,
                knmReasonDomain))
            {
                this.Decision = decisionDomain.Get(this.ObjectId);
                
                this.InitInspectionInfo();
                
                this.realityObjects = inspectionGjiRealityObjectDomain.GetAll().Where(x => x.Inspection.Id == this.inspectionDto.Id).ToArray();
                this.inspectors = documentGjiInspectorDomain.GetAll().Where(x => x.DocumentGji.Id == this.Decision.Id).ToArray();
                this.experts = disposalExpertDomain.GetAll().Where(x => x.Disposal.Id == this.Decision.Id).ToArray();
                this.organizationDocuments = disposalProvidedDocDomain.GetAll().Where(x => x.Disposal.Id == this.Decision.Id).ToArray();
                this.typeId = knmCharacterKindCheckDomain.GetAll()
                    .FirstOrDefault(x => x.KindCheckGji.Id == this.Decision.KindCheck.Id)?.KnmCharacter.ErknmCode?.ToString();

                var actCheckIdsByDisposal = documentGjiChildrenDomain.GetAll()
                    .Where(x => x.Parent.Id == this.Decision.Id && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Select(y => y.Children.Id);
                
                this.events = actCheckActionDomain.GetAll()
                    .Where(x => actCheckIdsByDisposal.Contains(x.ActCheck.Id))
                    .ToArray();

                this.reasons = decisionInspectionBaseDomain.GetAll()
                    .Where(x => x.Decision.Id == this.Decision.Id)
                    .ToArray();

                this.decisionControlObjectInfoDict = decisionControlObjectInfoDomain.GetAll()
                    .Where(x => this.realityObjects.Contains(x.InspGjiRealityObject) && x.Decision.Id == this.Decision.Id)
                    .ToDictionary(key => key.InspGjiRealityObject.Id, value => value.ControlObjectKind);

                this.documents = disposalAnnexDomain.GetAll().Where(x => x.Disposal.Id == this.Decision.Id).ToArray();
                this.reasonDocuments = knmReasonDomain.GetAll().Where(x => x.Decision.Id == this.Decision.Id).ToArray();
                
                var risk = inspectionRiskDomain.GetAll().Where(x => x.Inspection.Id == this.inspectionDto.Id).FirstOrDefault(x => !x.EndDate.HasValue);
                
                if (risk != null)
                {
                    this.riskCategoryDictVersionId = tatRiskCategoryDomain.Get(risk.RiskCategory.Id)?.ErvkGuid; 
                }

                // Сборка поля noticeMethodId
                switch (this.Decision.NotificationType)
                {
                    case NotificationType.Individually:
                        this.noticeMethodId = "1";
                        break;
                    case NotificationType.Courier:
                        this.noticeMethodId = "2";
                        break;
                    case NotificationType.Other:
                        this.noticeMethodId = "44";
                        break;
                    case NotificationType.Agenda:
                        this.noticeMethodId = "3";
                        break;
                }
                
                // Сборка поля approveRequiredId
                switch (this.Decision.TypeAgreementProsecutor)
                {
                    case TypeAgreementProsecutor.RequiresAgreement:
                        this.approveRequiredId = "1";
                        break;
                    case TypeAgreementProsecutor.NotRequiresAgreement:
                        this.approveRequiredId = "2";
                        break;
                    case TypeAgreementProsecutor.ImmediateInspection:
                        this.approveRequiredId = "3";
                        break;
                }

                if (this.plannedTypeCheckList.Contains(this.Decision.KindCheck.Code))
                {
                    this.planGuid = baseJurPersonDomain.Get(this.inspectionDto.Id)?.Plan?.ErknmGuid;
                }
                
                this.kindDictVersionId = knmTypeKindCheckDomain
                    .FirstOrDefault(x => x.KindCheckGji.Id == this.Decision.KindCheck.Id)?.KnmTypes.ErvkId;
            }
        }

        private string GetInspectorPositionDictId(Inspector inspector, bool isIssuer = false)
        {
            var controlTypeInspectorPositionsDomain = this.Container.ResolveDomain<ControlTypeInspectorPositions>();

            using (this.Container.Using(controlTypeInspectorPositionsDomain))
            {
                if (inspector?.InspectorPosition != null)
                {
                    return controlTypeInspectorPositionsDomain
                        .FirstOrDefault(x =>
                            x.IsIssuer == isIssuer &&
                            x.ControlType.Id == this.Decision.ControlType.Id && 
                            x.InspectorPosition.Id == inspector.InspectorPosition.Id)?.ErvkId;
                }

                return null;
            }
        }
        
        private string GetEventsDictId(ActCheckActionType inspector)
        {
            var knmActionDomain = this.Container.ResolveDomain<KnmAction>();

            using (this.Container.Using(knmActionDomain))
            {
                return knmActionDomain
                    .FirstOrDefault(x =>
                        x.ActCheckActionType == inspector)?.ErvkId;
            }
        }

        private void InitInspectionInfo()
        {
            var baseJurPersonDomain = this.Container.ResolveDomain<BaseJurPerson>();
            var baseDispHeadDomain = this.Container.ResolveDomain<BaseDispHead>();
            var baseProsClaimDomain = this.Container.ResolveDomain<BaseProsClaim>();
            var baseStatementDomain = this.Container.ResolveDomain<BaseStatement>();
            var warningInspectionDomain = this.Container.ResolveDomain<WarningInspection>();
            var taskActionIsolatedDomain = this.Container.ResolveDomain<TaskActionIsolated>();
            var inspectionActionIsolatedDomain = this.Container.ResolveDomain<InspectionActionIsolated>();

            using (this.Container.Using(
                baseJurPersonDomain,
                baseDispHeadDomain,
                baseProsClaimDomain,
                baseStatementDomain,
                warningInspectionDomain,
                taskActionIsolatedDomain,
                inspectionActionIsolatedDomain))
            {

                var typeBaseDict = new Dictionary<TypeBase, Type>
                {
                    { TypeBase.PlanJuridicalPerson, typeof(BaseJurPerson) },
                    { TypeBase.ProsecutorsClaim, typeof(BaseProsClaim) },
                    { TypeBase.DisposalHead, typeof(BaseDispHead) },
                    { TypeBase.CitizenStatement, typeof(BaseStatement) },
                    { TypeBase.InspectionActionIsolated, typeof(InspectionActionIsolated) }
                };

                this.inspectionDto = this.Decision.Inspection.CopyIdenticalProperties<InspectionGji, InspectionDto>();

                if (typeBaseDict.ContainsKey(this.inspectionDto.TypeBase))
                {
                    var type = typeof(IDomainService<>).MakeGenericType(typeBaseDict[this.inspectionDto.TypeBase]);
                    var repository = this.Container.Resolve(type) as IDomainService;

                    using (this.Container.Using(repository))
                    {
                        var item = repository.Get(this.inspectionDto.Id);
                        var property = item.GetPropertyValue("TypeForm");

                        if (property is TypeFormInspection inspection)
                        {
                            this.inspectionDto.TypeForm = inspection;
                        }
                    }
                }

                if (this.inspectionDto.TypeBase == TypeBase.InspectionActionIsolated)
                {
                    var inspectionActionIsolated = inspectionActionIsolatedDomain.Get(this.inspectionDto.Id);
                    var taskActionIsolated = taskActionIsolatedDomain
                        .FirstOrDefault(x => x.Inspection.Id == inspectionActionIsolated.ActionIsolated.Id);

                    if (taskActionIsolated?.TypeObject == TypeDocObject.Individual)
                    {
                        this.inspectionDto.OrganizationName = taskActionIsolated?.PersonName;
                        this.inspectionDto.OrganizationInn = taskActionIsolated?.Inn;
                    }
                    else
                    {
                        this.inspectionDto.OrganizationInn = taskActionIsolated?.Contragent?.Inn;
                    }
                }
                else
                {
                    if (this.inspectionDto.PersonInspection == PersonInspection.PhysPerson)
                    {
                        switch (this.inspectionDto.TypeBase)
                        {
                            case TypeBase.DisposalHead:
                                var baseDispHead = baseDispHeadDomain.Get(this.inspectionDto.Id);
                                this.inspectionDto.OrganizationInn = baseDispHead?.Inn;
                                this.inspectionDto.OrganizationName = baseDispHead?.PhysicalPerson;
                                break;
                            case TypeBase.ProsecutorsClaim:
                                var baseProsClaim = baseProsClaimDomain.Get(this.inspectionDto.Id);
                                this.inspectionDto.OrganizationInn = baseProsClaim?.Inn;
                                this.inspectionDto.OrganizationName = baseProsClaim?.PhysicalPerson;
                                break;
                            case TypeBase.CitizenStatement:
                                var baseStatement = baseStatementDomain.Get(this.inspectionDto.Id);
                                this.inspectionDto.OrganizationInn = baseStatement?.Inn;
                                this.inspectionDto.OrganizationName = baseStatement?.PhysicalPerson;
                                break;
                            case TypeBase.GjiWarning:
                                var warningInspection = warningInspectionDomain.Get(this.inspectionDto.Id);
                                this.inspectionDto.OrganizationInn = warningInspection?.Inn;
                                this.inspectionDto.OrganizationName = warningInspection?.PhysicalPerson;
                                break;
                        }
                    }
                    else
                    {
                        this.inspectionDto.OrganizationInn = this.inspectionDto.Contragent.Inn;
                    }
                }
            }
        }
    }
}