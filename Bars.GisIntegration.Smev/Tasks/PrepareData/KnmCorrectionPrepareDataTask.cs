namespace Bars.GisIntegration.Smev.Tasks.PrepareData
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DataModels;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.GisIntegration.Smev.ConfigSections;
    using Bars.GisIntegration.Smev.Dto;
    using Bars.GisIntegration.Smev.SmevExchangeService.ERKNM;
    using Bars.GisIntegration.Smev.Tasks.PrepareData.Base;
    using Bars.GisIntegration.Smev.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
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
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    using Castle.Core.Internal;
    
    using Fasterflect;

    /// <summary>
    /// Задача подготовки данных для метода "Корректировка КНМ"
    /// </summary>
    public class KnmCorrectionPrepareDataTask : ErknmPrepareDataTask<LetterToErknmType>
    {
        // Извлеченный и обработанный объект для создания запроса
        private DecisionCorrectionDto extractedObject;
        
        // Настройка параметров интеграции с ЕРКНМ
        private ErknmIntegrationConfig erknmIntegrationConfig;

        private ActCheck[] actChecks;
        private long[] actCheckIds;
        private ActCheckPeriod[] actCheckPeriods;
        private DocumentGjiInspector[] actDocumentGjiInspectors;
        private ControlTypeInspectorPositions[] controlTypeInspectorPositions;
        
        // Код региона (Татарстан)
        private const string DistrictId = "1033920000000001";

        // Код документа типа Решение
        private const string DocumentTypeId = "125";

        // Допустимые типы проверок для отображения информации об уведомлении
        private readonly TypeCheck[] noticeTypeCheckArray = { TypeCheck.PlannedExit, TypeCheck.NotPlannedExit };
        
        // Допустимые типы проверок для отображения информации о датах направления требования и получения документа
        private readonly TypeCheck[] documentDatesTypeCheckArray = { TypeCheck.PlannedDocumentation, TypeCheck.NotPlannedDocumentation };
        
        // Допустимые формы проверок для информации об исп. средств дистанционного взаимодействия
        private readonly TypeFormInspection[] typeFormCheckArray = { TypeFormInspection.InspectionVisit, TypeFormInspection.Exit };

        // Словарь сопоставления типа уведомления с идентификатором метода уведомления
        private readonly IDictionary<NotificationType, string> noticeMethodIdDict = new Dictionary<NotificationType, string>
        {
            {NotificationType.Individually, "1"},
            {NotificationType.Courier, "2"},
            {NotificationType.Other, "44"},
            {NotificationType.Agenda, "3"}
        };

        /// <inheritdoc />
        protected override void ExtractData(DynamicDictionary parameters)
        {
            this.erknmIntegrationConfig = this.Container.GetGkhConfig<ErknmIntegrationConfig>();
            
            var decisionDataExtractor = this.Container.Resolve<IDataExtractor<Decision>>();
            var documentGjiChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var protocolDomain = this.Container.ResolveDomain<Protocol>();
            var protocolAnnexDomain = this.Container.ResolveDomain<ProtocolAnnex>();
            var protocolArticleLawDomain = this.Container.ResolveDomain<ProtocolArticleLaw>();
            var prescriptionDomain = this.Container.ResolveDomain<Prescription>();
            var prescriptionAnnexDomain = this.Container.ResolveDomain<PrescriptionAnnex>();
            var prescriptionViolationDomain = this.Container.ResolveDomain<PrescriptionViol>();
            var knmCharacterKindCheckDomain = this.Container.ResolveDomain<KnmCharacterKindCheck>();
            var actCheckDomain = this.Container.ResolveDomain<ActCheck>();
            var actCheckPeriodDomain = this.Container.ResolveDomain<ActCheckPeriod>();
            var actCheckActionDomain = this.Container.ResolveDomain<ActCheckAction>();
            var inspectionGjiRealityObjectDomain = this.Container.ResolveDomain<InspectionGjiRealityObject>();
            var decisionControlObjectInfoDomain = this.Container.ResolveDomain<DecisionControlObjectInfo>();
            var tatRiskCategoryDomain = this.Container.ResolveDomain<TatRiskCategory>();
            var inspectionRiskDomain = this.Container.ResolveDomain<InspectionRisk>();
            var documentGjiInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var controlTypeInspectorPositionsDomain = this.Container.ResolveDomain<ControlTypeInspectorPositions>();
            var baseDispHeadDomain = this.Container.ResolveDomain<BaseDispHead>();
            var baseProsClaimDomain = this.Container.ResolveDomain<BaseProsClaim>();
            var baseStatementDomain = this.Container.ResolveDomain<BaseStatement>();
            var warningInspectionDomain = this.Container.ResolveDomain<WarningInspection>();
            var taskActionIsolatedDomain = this.Container.ResolveDomain<TaskActionIsolated>();
            var inspectionActionIsolatedDomain = this.Container.ResolveDomain<InspectionActionIsolated>();
            var disposalProvidedDocDomain = this.Container.ResolveDomain<DisposalProvidedDoc>();
            var decisionInspectionBaseDomain = this.Container.ResolveDomain<DecisionInspectionBase>();
            var disposalExpertDomain = this.Container.ResolveDomain<DisposalExpert>();
            var knmActionDomain = this.Container.ResolveDomain<KnmAction>();
            var knmReasonDomain = this.Container.ResolveDomain<KnmReason>();
            var disposalAnnexDomain = this.Container.ResolveDomain<TatDisposalAnnex>();
            var resolutionsDomain = this.Container.ResolveDomain<TatarstanResolution>();

            using (this.Container.Using(
                       decisionDataExtractor,
                       documentGjiChildrenDomain,
                       protocolDomain,
                       protocolAnnexDomain,
                       prescriptionDomain,
                       prescriptionAnnexDomain,
                       knmCharacterKindCheckDomain,
                       inspectionGjiRealityObjectDomain,
                       decisionControlObjectInfoDomain,
                       tatRiskCategoryDomain,
                       inspectionRiskDomain,
                       disposalAnnexDomain,
                       documentGjiInspectorDomain,
                       controlTypeInspectorPositionsDomain,
                       baseDispHeadDomain,
                       baseProsClaimDomain,
                       baseStatementDomain,
                       warningInspectionDomain,
                       taskActionIsolatedDomain,
                       inspectionActionIsolatedDomain,
                       disposalProvidedDocDomain,
                       actCheckDomain,
                       actCheckPeriodDomain,
                       actCheckActionDomain,
                       decisionInspectionBaseDomain,
                       disposalExpertDomain,
                       knmActionDomain,
                       knmReasonDomain,
                       prescriptionViolationDomain,
                       resolutionsDomain,
                       protocolArticleLawDomain))
            {
                var decision = decisionDataExtractor.Extract(parameters).FirstOrDefault();

                if (decision == null)
                {
                    throw new Exception("Нет данных для извлечения");
                }

                this.actChecks = documentGjiChildrenDomain.GetAll()
                    .Join(
                        actCheckDomain.GetAll(),
                        x => x.Children,
                        y => y,
                        (x, y) => new
                        {
                            ActCheck = y,
                            ParentChildrenDoc = x
                        })
                    .Where(x => x.ParentChildrenDoc.Parent.Id == decision.Id)
                    .Select(x => x.ActCheck)
                    .ToArray();
                this.actCheckIds = this.actChecks.Select(x => x.Id).ToArray();
                this.actCheckPeriods = actCheckPeriodDomain.GetAll()
                    .Where(x => this.actCheckIds.Contains(x.ActCheck.Id))
                    .ToArray();
                
                this.actDocumentGjiInspectors = documentGjiInspectorDomain.GetAll()
                    .Where(x => this.actCheckIds.Contains(x.DocumentGji.Id))
                    .ToArray();
                
                this.controlTypeInspectorPositions = controlTypeInspectorPositionsDomain.GetAll()
                    .ToArray();
                
                var realityObjects = inspectionGjiRealityObjectDomain.GetAll()
                    .Where(x => x.Inspection.Id == decision.Inspection.Id)
                    .ToArray();
                
                var decisionControlObjectInfoDict = decisionControlObjectInfoDomain.GetAll()
                    .Where(x => realityObjects.Contains(x.InspGjiRealityObject) && x.Decision.Id == decision.Id)
                    .ToDictionary(key => key.InspGjiRealityObject.Id, value => value.ControlObjectKind);
                var risk = inspectionRiskDomain.GetAll()
                    .Where(x => x.Inspection.Id == decision.Inspection.Id).FirstOrDefault(x => !x.EndDate.HasValue);
                var riskCategoryDictVersionId = risk != null
                    ? tatRiskCategoryDomain.Get(risk.RiskCategory.Id)?.ErvkGuid
                    : null;
                
                var documentGjiInspectors = documentGjiInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == decision.Id)
                    .ToArray();
                
                var documentDateTime = decision.DocumentDate.GetValueOrDefault().Date.Add(decision.DocumentTime.GetValueOrDefault().TimeOfDay);
                
                string approveRequiredId = null;
                switch (decision.TypeAgreementProsecutor)
                {
                    case TypeAgreementProsecutor.RequiresAgreement:
                        approveRequiredId = "1";
                        break;
                    case TypeAgreementProsecutor.NotRequiresAgreement:
                        approveRequiredId = "2";
                        break;
                    case TypeAgreementProsecutor.ImmediateInspection:
                        approveRequiredId = "3";
                        break;
                    case TypeAgreementProsecutor.NotSet:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var inspectorPositionDictId = decision.IssuedDisposal?.InspectorPosition != null
                    ? controlTypeInspectorPositionsDomain
                        .FirstOrDefault(x =>
                            x.IsIssuer == true &&
                            x.ControlType.Id == decision.ControlType.Id &&
                            x.InspectorPosition.Id == decision.IssuedDisposal.InspectorPosition.Id)?.ErvkId
                    : null;
                
                var organizationDocuments = disposalProvidedDocDomain.GetAll()
                    .Where(x => x.Disposal.Id == decision.Id).ToArray();
                
                var reasons = decisionInspectionBaseDomain.GetAll()
                    .Where(x => x.Decision.Id == decision.Id).ToArray();
                
                var isNoticeInformationAllowed = this.noticeTypeCheckArray.Contains(decision.KindCheck.Code);
                var isDocumentDateInformationAllowed = this.documentDatesTypeCheckArray.Contains(decision.KindCheck.Code);
                var isRemoteInformationAllowed = false;
                var actCheckActions = actCheckActionDomain.GetAll()
                    .Where(x => this.actChecks.Contains(x.ActCheck))
                    .ToArray();
                var actionTypes = actCheckActions.Select(x => x.ActionType as ActCheckActionType?);
                var actionTypeDict = knmActionDomain.GetAll()
                    .Where(x => actionTypes.Contains(x.ActCheckActionType))
                    .ToDictionary(x => x.ActCheckActionType, y => y.ErvkId);
                
                var typeBaseDict = new Dictionary<TypeBase, Type>
                {
                    { TypeBase.PlanJuridicalPerson, typeof(BaseJurPerson) },
                    { TypeBase.ProsecutorsClaim, typeof(BaseProsClaim) },
                    { TypeBase.DisposalHead, typeof(BaseDispHead) },
                    { TypeBase.CitizenStatement, typeof(BaseStatement) },
                    { TypeBase.InspectionActionIsolated, typeof(InspectionActionIsolated) }
                };
                
                if (typeBaseDict.ContainsKey(decision.Inspection.TypeBase))
                {
                    var domainType = typeof(IDomainService<>).MakeGenericType(typeBaseDict[decision.Inspection.TypeBase]);
                    var domainService = this.Container.Resolve(domainType) as IDomainService;

                    using (this.Container.Using(domainService))
                    {
                        var inspectionBase = domainService.Get(decision.Inspection.Id);
                        var typeForm = inspectionBase?.GetPropertyValue("TypeForm");
                        
                        if (typeForm is TypeFormInspection inspectionTypeForm)
                        {
                            isRemoteInformationAllowed = this.typeFormCheckArray.Contains(inspectionTypeForm);
                        }
                    }
                }

                var objectObjectTypeUpdate = new List<IObjectObjectTypeUpdate>();
                var objectObjectKindUpdate = new List<IObjectObjectKindUpdate>();
                var objectRiskCategoryUpdate = new List<IObjectRiskCategoryUpdate>();

                foreach (var realityObject in realityObjects.Where(x => this.HasErknmGuid(x) && 
                             decisionControlObjectInfoDict.ContainsKey(x.Id) && !riskCategoryDictVersionId.IsNullOrEmpty()))
                {
                    var objectGuid = realityObject.ErknmGuid;

                    objectObjectTypeUpdate.Add(
                        new IObjectObjectTypeUpdate
                        {
                            dictId = this.erknmIntegrationConfig.ControlObjectTypeId,
                            dictVersionId = decisionControlObjectInfoDict[realityObject.Id].ControlObjectType?.ErvkId,
                            IObjectGuid = objectGuid,
                            isDelSpecified = true
                        });

                    objectObjectKindUpdate.Add(
                        new IObjectObjectKindUpdate
                        {
                            dictId = this.erknmIntegrationConfig.ControlObjectKindId,
                            dictVersionId = decisionControlObjectInfoDict[realityObject.Id].ErvkId,
                            IObjectGuid = objectGuid,
                            isDelSpecified = true
                        });

                    objectRiskCategoryUpdate.Add(
                        new IObjectRiskCategoryUpdate
                        {
                            dictId = this.erknmIntegrationConfig.RiskCategoryId,
                            dictVersionId = riskCategoryDictVersionId,
                            IObjectGuid = objectGuid,
                            isDelSpecified = true
                        });
                }

                var inspectorsInsert = new List<IInspectorsInsert>();
                var inspectorPositionUpdate = new List<IInspectorPositionUpdate>();

                foreach (var documentGjiInspector in documentGjiInspectors)
                {
                    var inspector = documentGjiInspector.Inspector;
                    
                    var dictVersionId = inspector.InspectorPosition != null
                        ? this.controlTypeInspectorPositions
                            .FirstOrDefault(x =>
                                x.ControlType?.Id == decision.ControlType?.Id &&
                                x.InspectorPosition.Id == inspector.InspectorPosition.Id)?.ErvkId
                        : null;
                    
                    var inspectorGuid = documentGjiInspector.ErknmGuid;
                    if (inspectorGuid == null)
                    {
                        inspectorsInsert.Add(
                            new IInspectorsInsert
                            {
                                fullName = inspector.Fio,
                                guid = this.GetErknmGuid(documentGjiInspector, typeof(DocumentGjiInspector)),
                                position = new IDictionary
                                {
                                    dictId = this.erknmIntegrationConfig.InspectorPositionId,
                                    dictVersionId = dictVersionId
                                }
                            });
                    }
                    else
                    {
                        inspectorPositionUpdate.Add(
                            new IInspectorPositionUpdate
                            {
                                dictId = this.erknmIntegrationConfig.InspectorPositionId,
                                dictVersionId = dictVersionId,
                                IInspectorGuid = documentGjiInspector.ErknmGuid,
                                isDelSpecified = true
                            });
                    }
                }

                IOrganizationsUpdate organizationsUpdate = null;
                if (decision.OrganizationErknmGuid != null)
                {
                    organizationsUpdate = new IOrganizationsUpdate
                    {
                        guid = decision.OrganizationErknmGuid,
                        isFiz = decision.Inspection.PersonInspection == PersonInspection.PhysPerson,
                        isFizSpecified = true,
                        isDelSpecified = true
                    };

                    switch (decision.Inspection.TypeBase)
                    {
                        case TypeBase.InspectionActionIsolated:
                            var inspectionActionIsolated = inspectionActionIsolatedDomain.Get(decision.Inspection.Id);
                            var taskActionIsolated = taskActionIsolatedDomain
                                .FirstOrDefault(x => x.Inspection.Id == inspectionActionIsolated.ActionIsolated.Id);

                            if (taskActionIsolated?.TypeObject == TypeDocObject.Individual)
                            {
                                organizationsUpdate.inn = taskActionIsolated?.Inn;
                                organizationsUpdate.organizationName = taskActionIsolated?.PersonName;
                            }
                            else
                            {
                                organizationsUpdate.inn = taskActionIsolated?.Contragent?.Inn;
                            }
                            break;

                        default:
                            if (decision.Inspection.PersonInspection == PersonInspection.PhysPerson)
                            {
                                switch (decision.Inspection.TypeBase)
                                {
                                    case TypeBase.DisposalHead:
                                        var baseDispHead = baseDispHeadDomain.Get(decision.Inspection.Id);
                                        organizationsUpdate.inn = baseDispHead?.Inn;
                                        organizationsUpdate.organizationName = baseDispHead?.PhysicalPerson;
                                        break;
                                    case TypeBase.ProsecutorsClaim:
                                        var baseProsClaim = baseProsClaimDomain.Get(decision.Inspection.Id);
                                        organizationsUpdate.inn = baseProsClaim?.Inn;
                                        organizationsUpdate.organizationName = baseProsClaim?.PhysicalPerson;
                                        break;
                                    case TypeBase.CitizenStatement:
                                        var baseStatement = baseStatementDomain.Get(decision.Inspection.Id);
                                        organizationsUpdate.inn = baseStatement?.Inn;
                                        organizationsUpdate.organizationName = baseStatement?.PhysicalPerson;
                                        break;
                                    case TypeBase.GjiWarning:
                                        var warningInspection = warningInspectionDomain.Get(decision.Inspection.Id);
                                        organizationsUpdate.inn = warningInspection?.Inn;
                                        organizationsUpdate.organizationName = warningInspection?.PhysicalPerson;
                                        break;
                                }
                            }
                            else
                            {
                                organizationsUpdate.inn = decision.Inspection.Contragent.Inn;
                            }
                            break;
                    }
                }

                var experts = disposalExpertDomain.GetAll()
                    .Where(x => x.Disposal.Id == decision.Id)
                    .ToList();

                var knmReasons = knmReasonDomain.GetAll()
                    .Where(x => x.Decision.Id == decision.Id)
                    .ToList();
                
                var decisionAttachments = disposalAnnexDomain.GetAll()
                    .Where(x => x.Disposal.Id == decision.Id)
                    .ToArray();

                var protocols = protocolDomain.GetAll()
                    .Where(x => x.Inspection == decision.Inspection)
                    .ToArray();
                
                var prescriptions = prescriptionDomain.GetAll()
                    .Where(x => x.Inspection == decision.Inspection)
                    .ToArray();

                var resolutions = resolutionsDomain.GetAll()
                    .Where(x => x.Inspection == decision.Inspection)
                    .ToArray();

                var protocolIds = protocols.Select(x => x.Id).ToArray();
                var prescriptionIds = prescriptions.Select(x => x.Id).ToArray();
                
                var protocolAnnexList = protocolAnnexDomain
                    .GetAll()
                    .Where(x => x.Protocol != null &&
                        x.File != null &&
                        x.SendFileToErknm == YesNoNotSet.Yes &&
                        x.ErknmGuid == null &&
                        protocolIds.Contains(x.Protocol.Id))
                    .ToArray();

                var prescriptionAnnexList = prescriptionAnnexDomain
                    .GetAll()
                    .Where(x => x.Prescription != null &&
                        x.File != null &&
                        x.SendFileToErknm == YesNoNotSet.Yes &&
                        x.ErknmGuid == null &&
                        prescriptionIds.Contains(x.Prescription.Id))
                    .ToArray();

                var actCorrectionDto = this.GetActInfo(decision);

                var inspectorsLookup = documentGjiInspectorDomain.GetAll()
                    .Where(x =>
                        protocolIds
                            .Union(prescriptionIds)
                            .Contains(x.DocumentGji.Id))
                    .ToLookup(x => x.DocumentGji.Id);

                var prescriptionViolLookup = prescriptionViolationDomain.GetAll()
                    .Where(x => prescriptionIds.Contains(x.Document.Id))
                    .ToLookup(x => x.Document.Id);

                var resolutionProtocolDict = documentGjiChildrenDomain.GetAll()
                    .Where(x => protocolIds.Contains(x.Parent.Id)
                        && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution 
                        && x.Parent.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .ToDictionary(x => x.Children.Id, 
                        x => protocols.FirstOrDefault(y => y.Id == x.Parent.Id && this.HasErknmGuid(y)));
                
                var protocolLawArticles = protocolArticleLawDomain
                    .GetAll()
                    .Where(x => protocolIds.Contains(x.Protocol.Id))
                    .ToArray();
                
                FillSubjectResultDecisionLists
                (
                    decision,
                    protocols,
                    prescriptions,
                    protocolAnnexList,
                    prescriptionAnnexList,
                    prescriptionViolationDomain,
                    protocolIds,
                    prescriptionIds,
                    out var subjectResultDecisionsInsert,
                    out var subjectResultDecisionsUpdate
                );

                this.FillResultDecisionResponsibleEntities(
                    protocolLawArticles,
                    resolutionProtocolDict,
                    resolutions,
                    out var resultDecisionResponsibleEntitiesUpdate,
                    out var resultDecisionResponsibleEntitiesInsert);

                var resultDecisionTitleSignerUpdateList = new List<IResultDecisionTitleSignerUpdate>();
                resultDecisionTitleSignerUpdateList.AddRange(this.GetResultDecisionTitleSignerUpdates(protocols, inspectorsLookup, decision));
                resultDecisionTitleSignerUpdateList.AddRange(this.GetResultDecisionTitleSignerUpdates(prescriptions, inspectorsLookup, decision));
                
                var resultDecisionInspectorsInsertList = new List<IResultDecisionInspectorsInsert>();
                resultDecisionInspectorsInsertList.AddRange(this.GetResultDecisionInspectorsInsert(protocols, inspectorsLookup, decision));
                resultDecisionInspectorsInsertList.AddRange(this.GetResultDecisionInspectorsInsert(prescriptions, inspectorsLookup, decision));
                
                this.extractedObject = new DecisionCorrectionDto
                {
                    ObjectId = decision.Id,
                    IGuid = decision.ErknmGuid,
                    InspectionUpdate = new InspectionUpdate
                    {
                        typeId = knmCharacterKindCheckDomain.GetAll()
                            .FirstOrDefault(x => x.KindCheckGji.Id == decision.KindCheck.Id)?
                            .KnmCharacter?
                            .ErknmCode?
                            .ToString(),
                        prosecutorOrganizationId = "1267",
                        noticeMethodId = isNoticeInformationAllowed && decision.NotificationType.HasValue
                            ? this.noticeMethodIdDict.Get(decision.NotificationType.Value) 
                            : null,
                        districtId = DistrictId,
                        startDate = decision.DateStart ?? DateTime.MinValue,
                        stopDate = decision.DateEnd ?? DateTime.MinValue,
                        stopDateSpecified = decision.DateEnd.HasValue,
                        noticeDate = isNoticeInformationAllowed && decision.NcDate.HasValue 
                            ? decision.NcDate.Value 
                            : DateTime.MinValue,
                        noticeDateSpecified = isNoticeInformationAllowed && decision.NcDate.HasValue,
                        isDelSpecified = true
                    },
                    InspectionKnmUpdate = new InspectionKnmUpdate
                    {
                        durationDays = decision.CountDays.HasValue 
                            ? decision.CountDays.ToString() 
                            : null,
                        durationHours = decision.CountHours.HasValue 
                            ? decision.CountHours.ToString() 
                            : null,
                        dataContent = decision.TypeAgreementProsecutor == TypeAgreementProsecutor.ImmediateInspection 
                            ? decision.InformationAboutHarm
                            : null,
                        isRemote = isRemoteInformationAllowed && decision.UsingMeansRemoteInteraction == YesNoNotSet.Yes,
                        isRemoteSpecified = isRemoteInformationAllowed && decision.UsingMeansRemoteInteraction != YesNoNotSet.NotSet,
                        noteRemote = isRemoteInformationAllowed && !string.IsNullOrEmpty(decision.InfoUsingMeansRemoteInteraction) 
                            ? decision.InfoUsingMeansRemoteInteraction 
                            : null,
                        documentRequestDate = isDocumentDateInformationAllowed && decision.SubmissionDate.HasValue 
                            ? decision.SubmissionDate.Value 
                            : DateTime.MinValue,
                        documentRequestDateSpecified = isDocumentDateInformationAllowed && decision.SubmissionDate.HasValue,
                        documentResponseDate = isDocumentDateInformationAllowed && decision.ReceiptDate.HasValue 
                            ? decision.ReceiptDate.Value 
                            : DateTime.MinValue,
                        documentResponseDateSpecified = isDocumentDateInformationAllowed && decision.ReceiptDate.HasValue,
                        isDelSpecified = true
                    },
                    DecisionUpdate = new IDecisionUpdate
                    {
                        dateTimeDecision = documentDateTime,
                        dateTimeDecisionSpecified = true,
                        numberDecision = decision.DocumentNumber,
                        placeDecision = decision.DecisionPlace?.AddressName,
                        fioSigner = decision.IssuedDisposal?.Fio,
                        isDelSpecified = true,
                        titleSigner = new IDictionary
                        {
                            dictId = this.erknmIntegrationConfig.SignerPostId,
                            dictVersionId = inspectorPositionDictId,
                            isDelSpecified = true
                        }
                    },
                    ApproveUpdate = !string.IsNullOrEmpty(approveRequiredId) 
                        ? new IApproveUpdate 
                        {
                            approveRequiredId = approveRequiredId,
                            isDelSpecified = true
                        } 
                        : null,
                    DocumentPlacesToInsert = this.actChecks
                        .Where(x => string.IsNullOrEmpty(x.PlaceErknmGuid))
                        .Select(x => new IPlacesInsert
                        {
                            value = x.DocumentPlaceFias?.AddressName,
                            guid = this.GetErknmGuid(x, x.GetType(), "PlaceErknmGuid")
                        })
                        .ToArray(),
                    DocumentPlacesToUpdate = this.actChecks
                        .Where(x => !string.IsNullOrEmpty(x.PlaceErknmGuid))
                        .Select(x => new IPlacesUpdate
                        {
                            value = x.DocumentPlaceFias?.AddressName,
                            guid = x.PlaceErknmGuid,
                            isDelSpecified = true
                        })
                        .ToArray(),
                    ObjectsInsert = realityObjects
                        .Where(x => x.ErknmGuid == null && decisionControlObjectInfoDict.ContainsKey(x.Id) && 
                            !riskCategoryDictVersionId.IsNullOrEmpty())
                        .Select(x => new IObjectsInsert
                        {
                            address = x.RealityObject.Address,
                            guid = this.GetErknmGuid(x, typeof(InspectionGjiRealityObject)),
                            houseGuid = x.RealityObject.HouseGuid,
                            objectType = new IDictionary
                            {
                                dictId = this.erknmIntegrationConfig.ControlObjectTypeId,
                                dictVersionId = decisionControlObjectInfoDict[x.Id].ControlObjectType?.ErvkId
                            },
                            objectKind = new IDictionary
                            {
                                dictId = this.erknmIntegrationConfig.ControlObjectKindId,
                                dictVersionId = decisionControlObjectInfoDict[x.Id].ErvkId
                            },
                            riskCategory = new IDictionary
                            {
                                dictId = this.erknmIntegrationConfig.RiskCategoryId,
                                dictVersionId = riskCategoryDictVersionId
                            }
                        }).ToArray(),
                    ObjectObjectTypeUpdate = objectObjectTypeUpdate.ToArray(),
                    ObjectObjectKindUpdate = objectObjectKindUpdate.ToArray(),
                    ObjectRiskCategoryUpdate = objectRiskCategoryUpdate.ToArray(),
                    InspectorsInsert = inspectorsInsert.ToArray(),
                    InspectorPositionUpdate = inspectorPositionUpdate.ToArray(),
                    OrganizationDocumentsInsert = organizationDocuments
                        .Where(x => x.ErknmGuid == null)
                        .Select(x => new IOrganizationDocumentsInsert
                        {
                            value = x.ProvidedDoc.Name,
                            guid = this.GetErknmGuid(x, typeof(DisposalProvidedDoc))
                        }).ToArray(),
                    OrganizationDocumentsUpdate = organizationDocuments
                        .Where(this.HasErknmGuid)
                        .Select(x => new IOrganizationDocumentsUpdate
                        {
                            value = x.ProvidedDoc.Name,
                            guid = x.ErknmGuid,
                            isDel = false,
                            isDelSpecified = true
                        }).ToArray(),
                    OrganizationsUpdate = organizationsUpdate != null 
                    ? new []{ organizationsUpdate }
                    : null,
                    ReasonsInsert = reasons
                        .Where(x => x.ErknmGuid == null)
                        .Select(x=> new IReasonsInsert
                    {
                        numGuid = this.GetErknmGuid(x, typeof(DecisionInspectionBase)),
                        reason = new IReason
                        {
                            reasonTypeId = x.InspectionBaseType?.ErknmCode,
                            date = x.FoundationDate ?? DateTime.MinValue,
                            note = x.OtherInspBaseType != string.Empty
                            ? x.OtherInspBaseType
                            : null,
                            dateSpecified = x.FoundationDate.HasValue
                        },
                        riskIndikators = x.RiskIndicator != null ? new IDictionary
                        {
                            dictId = this.erknmIntegrationConfig.RiskIndicatorId,
                            dictVersionId = x.RiskIndicator.ErvkId
                        } : null
                    }).ToArray(),
                    ReasonWithRiskReasonUpdate = reasons
                        .Where(this.HasErknmGuid)
                        .Select(x => new ReasonWithRiskReasonUpdate
                            {
                                reasonTypeId = x.InspectionBaseType?.ErknmCode,
                                date = x.FoundationDate ?? DateTime.MinValue,
                                note = x.OtherInspBaseType != string.Empty
                                    ? x.OtherInspBaseType
                                    : null,
                                ReasonWithRiskNumGuid = x.ErknmGuid,
                                dateSpecified = x.FoundationDate.HasValue,
                                isDelSpecified = true
                            }
                        ).ToArray(),
                    ReasonWithRiskRiskIndikatorsUpdate = reasons
                        .Where(x => this.HasErknmGuid(x) && x.RiskIndicator != null)
                        .Select(x => new ReasonWithRiskRiskIndikatorsUpdate
                            {
                                dictId = this.erknmIntegrationConfig.RiskIndicatorId,
                                dictVersionId = x.RiskIndicator.ErvkId,
                                ReasonWithRiskNumGuid = x.ErknmGuid,
                                isDelSpecified = true
                            }
                        ).ToArray(),
                    IEventsInsert = actCheckActions
                        .Where(x => string.IsNullOrEmpty(x.ErknmGuid))
                        .Select(x => new IEventsInsert
                        {
                            guid = this.GetErknmGuid(x, x.GetType()),
                            startDate = x.StartDate.GetValueOrDefault().Date.Add(x.StartTime.GetValueOrDefault().TimeOfDay),
                            stopDate = x.EndDate.GetValueOrDefault().Date.Add(x.EndTime.GetValueOrDefault().TimeOfDay),
                            startDateSpecified = x.StartDate.HasValue,
                            stopDateSpecified = x.EndDate.HasValue,
                            @event = new IDictionary
                            {
                                dictId = this.erknmIntegrationConfig.KnmActionId,
                                dictVersionId = actionTypeDict.Get(x.ActionType)
                            }
                        })
                        .ToArray(),
                    IEventsUpdate = actCheckActions
                        .Where(this.HasErknmGuid)
                        .Select(x => new IEventsUpdate
                        {
                            guid = x.ErknmGuid,
                            startDate = x.StartDate.GetValueOrDefault().Date.Add(x.StartTime.GetValueOrDefault().TimeOfDay),
                            stopDate = x.EndDate.GetValueOrDefault().Date.Add(x.EndTime.GetValueOrDefault().TimeOfDay),
                            startDateSpecified = x.StartDate.HasValue,
                            stopDateSpecified = x.EndDate.HasValue,
                            isDelSpecified = true,
                        })
                        .ToArray(),
                    ExpertsInsert = experts
                        .Where(x => x.ErknmGuid == null)
                        .Select(x => new IExpertsInsert
                        {
                            title = x.Expert.Name,
                            typeId = x.Expert.ExpertType.HasValue ? ((int)x.Expert.ExpertType).ToString() : null,
                            guid = this.GetErknmGuid(x, typeof(DisposalExpert))
                        }).ToArray(),
                    ExpertsUpdate = experts
                        .Where(this.HasErknmGuid)
                        .Select(x => new IExpertsUpdate
                        {
                            typeId = x.Expert.ExpertType.HasValue ? ((int)x.Expert.ExpertType).ToString() : null,
                            title = x.Expert.Name,
                            guid = x.ErknmGuid,
                            isDelSpecified = true
                        }).ToArray(),
                    ReasonDocumentsInsert = knmReasons
                        .Where(x => x.ErknmGuid == null)
                        .Select(x => new IReasonDocumentsInsert
                        {
                            guid = this.GetErknmGuid(x, x.GetType()),
                            documentTypeId = x.ErknmTypeDocument?.Code,
                            description = x.Description,
                            attachments = x.File != null 
                                ? this.InitAttachment(x, x.File, x.GetType()) 
                                : null
                        })
                        .ToArray(),
                    ReasonDocumentsUpdate = knmReasons
                        .Where(this.HasErknmGuid)
                        .Select(x => new IReasonDocumentsUpdate
                        {
                            guid = x.ErknmGuid,
                            documentTypeId = x.ErknmTypeDocument?.Code,
                            description = x.Description,
                            isDelSpecified = true
                        })
                        .ToArray(),
                    InspectionDocumentAttachmentsToInsert = decisionAttachments
                        .Where(this.HasErknmGuid)
                        .Select(x => new InspectionDocumentsInsert
                        {
                            documentTypeId = x.ErknmTypeDocument?.Code,
                            guid = this.GetErknmGuid(x, x.GetType()),
                            description = x.Description,
                            attachments = x.File != null 
                                ? this.InitAttachment(x, x.File, x.GetType()) 
                                : null
                        }).ToArray(),
                    InspectionDocumentAttachmentsToUpdate = decisionAttachments
                        .Where(this.HasErknmGuid)
                        .Select(x => new InspectionDocumentsUpdate
                        {
                            documentTypeId = x.ErknmTypeDocument?.Code,
                            guid = x.ErknmGuid,
                            description = x.Description,
                            isDelSpecified = true
                        })
                        .ToArray(),
                    SubjectActInsert = actCorrectionDto.SubjectActInsert,
                    SubjectActUpdate = actCorrectionDto.SubjectActUpdate,
                    ActTitleSignerUpdate = actCorrectionDto.ActTitleSignerUpdate,
                    ActKnoInspectorsInsert = actCorrectionDto.ActKnoInspectorsInsert,
                    ActDocumentInsert = actCorrectionDto.ActDocumentInsert,
                    SubjectResultDecisionsInsert = subjectResultDecisionsInsert.ToArray(),
                    SubjectResultDecisionsUpdate = subjectResultDecisionsUpdate.ToArray(),
                    ResultDecisionDocumentInsert = this.GetResultDecisionDocumentInsert(protocolAnnexList, prescriptionAnnexList),
                    ResultDecisionInjunctionUpdate = prescriptions
                        .Where(this.HasErknmGuid)
                        .Select(p => new IResultDecisionInjunctionUpdate
                        {
                            note = $"Предписание №{p.DocumentNumber} от {p.DocumentDate:d}",
                            dateResolved = prescriptionViolLookup[p.Id].FirstOrDefault()?.DatePlanRemoval ?? DateTime.MinValue,
                            dateResolvedSpecified = prescriptionViolLookup[p.Id]
                                .Where(x => x.DatePlanRemoval.HasValue)
                                .Distinct(x => x.DatePlanRemoval)
                                .Count() == 1,
                            guid = this.GetErknmGuid(prescriptionViolLookup[p.Id].ToList(), typeof(PrescriptionViol)),
                            IResultDecisionGuid = p.ErknmGuid,
                            isDelSpecified = true
                        })
                        .ToArray(),
                    ResultDecisionTitleSignerUpdate = resultDecisionTitleSignerUpdateList.ToArray(),
                    ResultDecisionInspectorsInsert = resultDecisionInspectorsInsertList.ToArray(),
                    ResultDecisionResponsibleEntitiesUpdate = resultDecisionResponsibleEntitiesUpdate.ToArray(),
                    ResultDecisionResponsibleEntitiesInsert = resultDecisionResponsibleEntitiesInsert.ToArray(),
                    ResponsibleSubjectStructuresNpaUpdate = 
                        this.GetResponsibleSubjectStructuresNpa<IResponsibleSubjectStructuresNPAUpdate>(protocolLawArticles, resolutions, resolutionProtocolDict),
                    ResponsibleSubjectStructuresNpaInsert = 
                        this.GetResponsibleSubjectStructuresNpa<IResponsibleSubjectStructuresNPAInsert>(protocolLawArticles, resolutions, resolutionProtocolDict)
                };
            }
        }
        
        /// <inheritdoc />
        protected override List<ValidateObjectResult> ValidateData()
        {
            return new List<ValidateObjectResult>();
        }

        /// <inheritdoc />
        protected override LetterToErknmType GetRequestObject(ref bool isTestMessage, out long objectId)
        {
            var requestBuilder = new KnmCorrectionRequestBuilder(this.extractedObject.IGuid);
            var result = requestBuilder
                .AddUpdatingObject(this.extractedObject.InspectionUpdate)
                .AddUpdatingObject(this.extractedObject.InspectionKnmUpdate)
                .AddUpdatingObject(this.extractedObject.DecisionUpdate)
                .AddUpdatingObject(this.extractedObject.ApproveUpdate)
                .AddUpdatingObject(this.extractedObject.DocumentPlacesToInsert)
                .AddUpdatingObject(this.extractedObject.DocumentPlacesToUpdate)
                .AddUpdatingObject(this.extractedObject.ObjectsInsert)
                .AddUpdatingObject(this.extractedObject.ObjectObjectTypeUpdate)
                .AddUpdatingObject(this.extractedObject.ObjectObjectKindUpdate)
                .AddUpdatingObject(this.extractedObject.ObjectRiskCategoryUpdate)
                .AddUpdatingObject(this.extractedObject.InspectorsInsert)
                .AddUpdatingObject(this.extractedObject.InspectorPositionUpdate)
                .AddUpdatingObject(this.extractedObject.OrganizationsUpdate)
                .AddUpdatingObject(this.extractedObject.OrganizationDocumentsInsert)
                .AddUpdatingObject(this.extractedObject.OrganizationDocumentsUpdate)
                .AddUpdatingObject(this.extractedObject.ReasonsInsert)
                .AddUpdatingObject(this.extractedObject.ReasonWithRiskReasonUpdate)
                .AddUpdatingObject(this.extractedObject.ReasonWithRiskRiskIndikatorsUpdate)
                .AddUpdatingObject(this.extractedObject.IEventsInsert)
                .AddUpdatingObject(this.extractedObject.IEventsUpdate)
                .AddUpdatingObject(this.extractedObject.ExpertsInsert)
                .AddUpdatingObject(this.extractedObject.ExpertsUpdate)
                .AddUpdatingObject(this.extractedObject.ReasonDocumentsInsert)
                .AddUpdatingObject(this.extractedObject.ReasonDocumentsUpdate)
                .AddUpdatingObject(this.extractedObject.InspectionDocumentAttachmentsToInsert)
                .AddUpdatingObject(this.extractedObject.InspectionDocumentAttachmentsToUpdate)
                .AddUpdatingObject(this.extractedObject.SubjectActInsert)
                .AddUpdatingObject(this.extractedObject.SubjectActUpdate)
                .AddUpdatingObject(this.extractedObject.ActTitleSignerUpdate)
                .AddUpdatingObject(this.extractedObject.ActKnoInspectorsInsert)
                .AddUpdatingObject(this.extractedObject.ActDocumentInsert)
                .AddUpdatingObject(this.extractedObject.SubjectResultDecisionsInsert)
                .AddUpdatingObject(this.extractedObject.SubjectResultDecisionsUpdate)
                .AddUpdatingObject(this.extractedObject.ResultDecisionDocumentInsert)
                .AddUpdatingObject(this.extractedObject.ResultDecisionInjunctionUpdate)
                .AddUpdatingObject(this.extractedObject.ResultDecisionTitleSignerUpdate)
                .AddUpdatingObject(this.extractedObject.ResultDecisionInspectorsInsert)
                .AddUpdatingObject(this.extractedObject.ResultDecisionResponsibleEntitiesInsert)
                .AddUpdatingObject(this.extractedObject.ResultDecisionResponsibleEntitiesUpdate)
                .AddUpdatingObject(this.extractedObject.ResponsibleSubjectStructuresNpaInsert)
                .AddUpdatingObject(this.extractedObject.ResponsibleSubjectStructuresNpaUpdate)
                .GetRequestObject();
            
            objectId = this.extractedObject.ObjectId;
            isTestMessage = false;
            
            this.SaveErknmEntities();

            return result;
        }

        /// <summary>
        /// Обновить ErknmGuid в БД для списка сущностей
        /// </summary>
        /// <param name="entities">Сущности</param>
        /// <param name="type">Тип сущности</param>
        /// <param name="guid">ErknmGuid</param>
        /// <param name="fieldName">Дополнительный параметр для гуидов отличных от ErknmGuid</param>
        private void UpdateErknmGuid(IEnumerable<IHaveId> entities, Type type, string guid, string fieldName = "ErknmGuid")
        {
            var entityType = typeof(IDomainService<>).MakeGenericType(type);
            var entityDomain = this.Container.Resolve(entityType) as IDomainService;

            using (this.Container.Using(entityDomain))
            {
                foreach (var entity in entities)
                {
                    entity.SetPropertyValue(fieldName, guid.ToUpper());
                    entityDomain.Update(entity);
                }
            }
        }

        private void FillSubjectResultDecisionLists(
            Decision decision,
            Protocol[] protocols,
            Prescription[] prescriptions,
            ProtocolAnnex[] protocolAnnexList,
            PrescriptionAnnex[] prescriptionAnnexList,
            IDomainService<PrescriptionViol> prescriptionViolationDomain,
            IEnumerable<long> protocolIds,
            IEnumerable<long> prescriptionIds,
            out ICollection<ISubjectResultDecisionsInsert> subjectResultDecisionsInsert,
            out ICollection<ISubjectResultDecisionsUpdate> subjectResultDecisionsUpdate)
        {
            subjectResultDecisionsInsert = new List<ISubjectResultDecisionsInsert>();
            subjectResultDecisionsUpdate = new List<ISubjectResultDecisionsUpdate>();
            
            if (!protocols.Any() && !prescriptions.Any())
            {
                return;
            }

            ILookup<long, TatarstanResolution> prosecutionResolutionLookup;
            ILookup<long, PrescriptionViol> prescriptionViolLookup;
            ILookup<long, DocumentGjiInspector> inspectorsLookup;

            var documentChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var resolutionDomain = this.Container.ResolveDomain<TatarstanResolution>();
            var documentInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();

            using (this.Container.Using(
                       documentChildrenDomain,
                       resolutionDomain,
                       documentInspectorDomain,
                       prescriptionViolationDomain))
            {
                var allowedSanctionCodes = new[]
                {
                    "0", // Не задано
                    "2", // Прекращено
                    "3"  // Устное замечание
                };
                
                prosecutionResolutionLookup = documentChildrenDomain.GetAll()
                    .Join(resolutionDomain.GetAll(),
                        childParent => childParent.Children.Id,
                        resolution => resolution.Id,
                        (childParent, resolution) => new
                        {
                            ParentId = childParent.Parent.Id,
                            Resolution = resolution
                        })
                    .Where(x => protocolIds.Contains(x.ParentId))
                    .Where(x => x.Resolution.Sanction != null)
                    .Where(x => !allowedSanctionCodes.Contains(x.Resolution.Sanction.Code))
                    .ToLookup(x => x.ParentId, x => x.Resolution);

                inspectorsLookup = documentInspectorDomain.GetAll()
                    .Where(x =>
                        protocolIds
                            .Union(prescriptionIds)
                            .Contains(x.DocumentGji.Id))
                    .ToLookup(x => x.DocumentGji.Id);

                prescriptionViolLookup = prescriptionViolationDomain.GetAll()
                    .Where(x => prescriptionIds.Contains(x.Document.Id))
                    .ToLookup(x => x.Document.Id);
            }
            
            // Сопоставление значений словаря "Исполнитель документа ГЖИ" для IResponsibleSubject
            var kindDecisionDict = new Dictionary<string, string>();
            NHibernate.Util.EnumerableExtensions.ForEach(new[] { "15", "9", "0", "2", "4", "8", "11", "17", "18", "21" }, x => kindDecisionDict.Add(x, "1"));
            NHibernate.Util.EnumerableExtensions.ForEach(new[] { "6", "7", "20" }, x => kindDecisionDict.Add(x, "2"));
            NHibernate.Util.EnumerableExtensions.ForEach(new[] { "16", "10", "12", "13", "1", "3", "5", "19", "22" }, x => kindDecisionDict.Add(x, "3"));

            foreach (var protocol in protocols)
            {
                if (this.HasErknmGuid(protocol))
                {
                    var result = new ISubjectResultDecisionsUpdate();
                    
                    FillResultProtocol(result, protocol);
                    subjectResultDecisionsUpdate.Add(result);
                }
                else
                {
                    var result = new ISubjectResultDecisionsInsert();
                    
                    FillResultProtocol(result, protocol);
                    subjectResultDecisionsInsert.Add(result);
                }
            }

            foreach (var prescription in prescriptions)
            {
                if (this.HasErknmGuid(prescription))
                {
                    var result = new ISubjectResultDecisionsUpdate();
                    
                    FillResultPrescription(result, prescription);
                    subjectResultDecisionsUpdate.Add(result);
                }
                else
                {
                    var result = new ISubjectResultDecisionsInsert();
                    
                    FillResultPrescription(result, prescription);
                    subjectResultDecisionsInsert.Add(result);
                }
            }
            
            void FillResultProtocol(IResultDecision result, Protocol protocol)
            {
                var inspectors = inspectorsLookup[protocol.Id].ToArray();
                var prosecutionResolutions = prosecutionResolutionLookup[protocol.Id].ToArray();

                result.kindDecisionId = "3";
                result.dateDecision = protocol.DocumentDate ?? DateTime.MinValue;
                result.dateDecisionSpecified = protocol.DocumentDate.HasValue;
                result.numberDecision = protocol.DocumentNumber;
                result.isProsecution = prosecutionResolutions.Any();
                result.isProsecutionSpecified = true;
                result.fioSigner = inspectors.FirstOrDefault()?.Inspector.Fio;
                result.guid = this.GetErknmGuid(protocol, typeof(Protocol));
                result.ISubjectGuid = decision.OrganizationErknmGuid;
                result.isDelSpecified = result is ISubjectResultDecisionsUpdate;

                if (result is ISubjectResultDecisionsInsert)
                {
                    var protocolAnnex = protocolAnnexList.FirstOrDefault(x => x.Protocol.Id == protocol.Id);
                    
                    result.document = protocolAnnex != null
                        ? new IDocument
                        {
                            documentTypeId = DocumentTypeId,
                            guid = this.GetErknmGuid(protocolAnnex, typeof(ProtocolAnnex)),
                            attachments = protocolAnnex.File != null 
                                ? this.InitAttachment(protocolAnnex, protocolAnnex.File, protocolAnnex.GetType()) 
                                : null
                        }
                        : null;
                    result.noteLawsuits = new IString[]
                    {
                        new IStringCreate()
                        {
                            value = $"Протокол №{protocol.DocumentNumber} от {protocol.DocumentDate:d}"
                        }
                    };
                    
                    if (inspectors.Any())
                    {
                        result.titleSigner = new IDictionaryCreate()
                        {
                            dictId = this.erknmIntegrationConfig.InspectorPositionId,
                            dictVersionId = this.GetMemberInspectorPositionDictId(inspectors.First().Inspector, decision)
                        };

                        result.inspectors = inspectors.Select(x => new IInspectorCreate()
                        {
                            fullName = x.Inspector.Fio,
                            guid = this.GetErknmGuid(x, x.GetType()),
                            position = new IDictionaryCreate()
                            {
                                dictId = this.erknmIntegrationConfig.InspectorPositionId,
                                dictVersionId = this.GetMemberInspectorPositionDictId(x.Inspector, decision)
                            }
                        }).ToArray();
                    }

                    if (prosecutionResolutions.Any())
                    {
                        result.responsibleEntities = prosecutionResolutions
                            .Select(x => new IResponsibleSubjectCreate()
                            {
                                typeId = kindDecisionDict[x.Executant.Code],
                                inn = x.Contragent.Inn,
                                ogrn = x.Contragent.Ogrn,
                                snils = x.Snils,
                                organizationName = x.Contragent.Name,
                                position = x.Company,
                                fullName = string.Join(" ", x.SurName, x.Name, x.Patronymic),

                                // Если вид санкции - "Административный штраф"
                                punishmentAmount = x.Sanction.Code == "1" && x.PenaltyAmount.HasValue ? (double)x.PenaltyAmount.Value : double.MinValue,
                                punishmentAmountSpecified = x.Sanction.Code == "1" && x.PenaltyAmount.HasValue,
                                punishmentAmountMeasure = x.Sanction.Code == "1" && x.PenaltyAmount.HasValue ? "рублей" : null,

                                // Если вид санкции - "Административное приостановление деятельности" ИЛИ "Дисквалификация"
                                punishmentTerm = new[] { "6", "7" }.Contains(x.Sanction.Code) ? x.SanctionsDuration : null,
                                responsibilityTypeId = x.Sanction.ErknmGuid,
                                guid = this.GetErknmGuid(x, x.GetType())
                            }).ToArray();
                    }
                }
            }
            
            void FillResultPrescription(IResultDecision result, Prescription prescription)
            {
                var inspectors = inspectorsLookup[prescription.Id].ToArray();
                
                result.kindDecisionId = "1";
                result.dateDecision = prescription.DocumentDate ?? DateTime.MinValue;
                result.dateDecisionSpecified = prescription.DocumentDate.HasValue;
                result.numberDecision = prescription.DocumentNumber;
                result.fioSigner = inspectors.FirstOrDefault()?.Inspector?.Fio;
                result.guid = this.GetErknmGuid(prescription, typeof(Prescription));
                result.ISubjectGuid = decision.OrganizationErknmGuid;
                result.isDelSpecified = result is ISubjectResultDecisionsUpdate;

                if (result is ISubjectResultDecisionsInsert)
                {
                    var prescriptionAnnex = prescriptionAnnexList.FirstOrDefault(x => x.Prescription.Id == prescription.Id);
                    var violations = prescriptionViolLookup[prescription.Id].ToArray();

                    result.document = prescriptionAnnex != null
                        ? new IDocument
                        {
                            documentTypeId = DocumentTypeId,
                            guid = this.GetErknmGuid(prescriptionAnnex, typeof(PrescriptionAnnex)),
                            attachments = prescriptionAnnex.File != null 
                                ? this.InitAttachment(prescriptionAnnex, prescriptionAnnex.File, prescriptionAnnex.GetType()) 
                                : null
                        }
                        : null;
                    
                    if (violations.Any())
                    {
                        result.injunction = new IInjunctionCreate()
                        {
                            note = $"Предписание №{prescription.DocumentNumber} от {prescription.DocumentDate:d}",
                            dateResolved = violations.FirstOrDefault()?.DatePlanRemoval ?? DateTime.MinValue,
                            dateResolvedSpecified = violations
                                .Where(x => x.DatePlanRemoval.HasValue)
                                .Distinct(x => x.DatePlanRemoval)
                                .Count() == 1,
                            guid = this.GetErknmGuid(violations, typeof(PrescriptionViol))
                        };
                    }

                    if (inspectors.Any())
                    {
                        result.titleSigner = new IDictionaryCreate()
                        {
                            dictId = this.erknmIntegrationConfig.InspectorPositionId,
                            dictVersionId = this.GetMemberInspectorPositionDictId(inspectors.First().Inspector, decision)
                        };

                        result.inspectors = inspectors.Select(x => new IInspectorCreate()
                        {
                            fullName = x.Inspector.Fio,
                            guid = this.GetErknmGuid(x, x.GetType()),
                            position = new IDictionaryCreate()
                            {
                                dictId = this.erknmIntegrationConfig.InspectorPositionId,
                                dictVersionId = this.GetMemberInspectorPositionDictId(x.Inspector, decision)
                            }
                        }).ToArray();
                    }
                }
            }
        }
        
        private T[] GetResponsibleSubjectStructuresNpa<T>(ProtocolArticleLaw[] protocolLawArticles, IEnumerable<TatarstanResolution> resolutions,
            IReadOnlyDictionary<long, Protocol> resolutionProtocolDict) where T : IString, new() =>
            resolutions.Where(this.HasErknmGuid)
                .SelectMany(x => protocolLawArticles
                    .Where(y => y.Protocol.Id == resolutionProtocolDict[x.Id].Id)
                    .Select(y => new T
                    {
                        value = y.ArticleLaw.Name,
                        guid = typeof(T) == typeof(IResponsibleSubjectStructuresNPAUpdate) ? 
                            y.ErknmGuid
                            : this.GetErknmGuid(y, typeof(ProtocolArticleLaw)),
                        IResponsibleSubjectGuid = x.ErknmGuid
                    }))
                .ToArray();

        private void FillResultDecisionResponsibleEntities(
            ProtocolArticleLaw[] protocolArticleLaws,
            IReadOnlyDictionary<long, Protocol> resolutionProtocolDict,
            IEnumerable<TatarstanResolution> resolutions,
            out ICollection<IResultDecisionResponsibleEntitiesUpdate> resultDecisionResponsibleEntitiesUpdate,
            out ICollection<IResultDecisionResponsibleEntitiesInsert> resultDecisionResponsibleEntitiesInsert)
        {
            resultDecisionResponsibleEntitiesUpdate = new List<IResultDecisionResponsibleEntitiesUpdate>();
            resultDecisionResponsibleEntitiesInsert = new List<IResultDecisionResponsibleEntitiesInsert>();
            
            var allowedSanctionCodes = new[]
            {
                "0", // Не задано
                "2", // Прекращено
                "3"  // Устное замечание
            };
            
            foreach (var resolution in resolutions.Where(x => !allowedSanctionCodes.Contains(x.Sanction.Code)))
            {
                if (this.HasErknmGuid(resolution))
                {
                    var responsibleSubjectUpdate = new IResultDecisionResponsibleEntitiesUpdate
                    {
                        guid = resolution.ErknmGuid
                    };
                    FillResponsibleSubject(responsibleSubjectUpdate, resolution);
                    resultDecisionResponsibleEntitiesUpdate.Add(responsibleSubjectUpdate);
                }
                else
                {
                    var responsibleSubjectInsert = new IResultDecisionResponsibleEntitiesInsert
                    {
                        guid = this.GetErknmGuid(resolution, typeof(TatarstanResolution))
                    };
                    FillResponsibleSubject(responsibleSubjectInsert, resolution);
                    resultDecisionResponsibleEntitiesInsert.Add(responsibleSubjectInsert);
                }
            }

            void FillResponsibleSubject(IResponsibleSubject result, TatarstanResolution resolution)
            {
                result.typeId = resolution.Executant.ErknmCode;
                result.responsibilityTypeId = string.IsNullOrEmpty(resolution.Sanction.ErknmGuid) ? resolution.Sanction.ErknmGuid : null;
                result.guid = resolution.ErknmGuid;
                result.inn = resolution.Contragent.Inn;
                result.ogrn = resolution.Contragent.Ogrn;
                result.snils = resolution.Snils;
                result.organizationName = resolution.Contragent.Name;
                result.position = resolution.Company;
                result.fullName = string.Join(" ", resolution.SurName, resolution.Name, resolution.Patronymic);
                result.punishmentAmount = resolution.Sanction.Code == "1" && resolution.PenaltyAmount.HasValue
                    ? (double) resolution.PenaltyAmount.Value
                    : double.MinValue;
                result.punishmentAmountSpecified = resolution.Sanction.Code == "1" && resolution.PenaltyAmount.HasValue;
                result.punishmentAmountMeasure = resolution.Sanction.Code == "1" && resolution.PenaltyAmount.HasValue ? "рублей" : null;
                result.punishmentTerm =
                    resolution.Sanction.Code == "1" || (resolution.Sanction.Code == "6" && !string.IsNullOrEmpty(resolution.SanctionsDuration))
                        ? resolution.SanctionsDuration
                        : null;
                result.IResultDecisionGuid = resolutionProtocolDict[resolution.Id].ErknmGuid;

                if (result is IResultDecisionResponsibleEntitiesInsert)
                {
                    result.structuresNPA = protocolArticleLaws
                        .Where(law => law.Protocol.Id == resolutionProtocolDict[resolution.Id].Id)
                        .AsEnumerable()
                        .Select(law => new IString
                        { 
                            value = law.ArticleLaw.Name, 
                            guid = this.GetErknmGuid(law, typeof(ProtocolArticleLaw))
                        })
                        .ToArray();
                }
            }
        }

        private IEnumerable<IResultDecisionInspectorsInsert> GetResultDecisionInspectorsInsert<T>(IEnumerable<T> documents,
            ILookup<long, DocumentGjiInspector> inspectorsLookup, Decision decision) where T : DocumentGji =>
            documents
                .Where(this.HasErknmGuid)
                .Select(doc => inspectorsLookup[doc.Id]
                    .Select(i => new IResultDecisionInspectorsInsert()
                    {
                        fullName = i.Inspector.Fio,
                        guid = this.GetErknmGuid(doc, doc.GetType()),
                        IResultDecisionGuid = doc.ErknmGuid,
                        position = new IDictionary()
                        {
                            dictId = this.erknmIntegrationConfig.InspectorPositionId,
                            dictVersionId = this.GetMemberInspectorPositionDictId(i.Inspector, decision)
                        }
                    })
                    .First());

        private IEnumerable<IResultDecisionTitleSignerUpdate> GetResultDecisionTitleSignerUpdates<T>(IEnumerable<T> documents, 
            ILookup<long,DocumentGjiInspector> inspectorsLookup, Decision decision) where T : DocumentGji => 
                documents.Where(this.HasErknmGuid)
                    .Select(x => new IResultDecisionTitleSignerUpdate()
                    {
                        dictId = this.erknmIntegrationConfig.InspectorPositionId,
                        dictVersionId = this.GetMemberInspectorPositionDictId(inspectorsLookup[x.Id].First().Inspector, decision),
                        IResultDecisionGuid = x.ErknmGuid,
                        isDelSpecified = true
                    });
        
        private IResultDecisionDocumentInsert[] GetResultDecisionDocumentInsert(
            ProtocolAnnex[] protocolAnnexList,
            PrescriptionAnnex[] prescriptionAnnexList)
        {
            var resultDecisionDocumentInsert = protocolAnnexList
                .Where(x => this.HasErknmGuid(x.Protocol))
                .Select(x => new IResultDecisionDocumentInsert
                {
                    documentTypeId = DocumentTypeId,
                    guid = this.GetErknmGuid(x, typeof(ProtocolAnnex)),
                    IResultDecisionGuid = x.Protocol.ErknmGuid,
                    attachments = x.File != null 
                        ? this.InitAttachment(x, x.File, x.GetType()) 
                        : null
                })
                .ToList();

            resultDecisionDocumentInsert.AddRange(prescriptionAnnexList
                .Where(this.HasErknmGuid)
                .Select(x => new IResultDecisionDocumentInsert
                {
                    guid = this.GetErknmGuid(x, typeof(PrescriptionAnnex)),
                    documentTypeId = DocumentTypeId,
                    IResultDecisionGuid = x.Prescription.ErknmGuid,
                    attachments = x.File != null 
                        ? this.InitAttachment(x, x.File, x.GetType()) 
                        : null
                }));

            return resultDecisionDocumentInsert.ToArray();
        }

        private ActCorrectionDto GetActInfo(Decision decision)
        {
            var actCorrectionDto = new ActCorrectionDto();

            if (!this.actChecks.Any())
            {
                return actCorrectionDto;
            }

            BuildActInfo(this.actChecks.All(x => string.IsNullOrEmpty(x.ErknmGuid)));

            void BuildActInfo(bool isInsert)
            {
                // Номер акта КНМ
                var numberAct = this.actChecks.OrderBy(x => x.DocumentDate).FirstOrDefault().DocumentNumber;
                if (numberAct?.Split('/')?.Length > 1)
                {
                    numberAct = numberAct.Split('/')[0];
                }

                // Дата и время составления акта КНМ
                var creationActDateTimeList = this.actChecks
                    .Where(x => x.DocumentDate != null && x.DocumentTime != null)
                    .Select(x => x.DocumentDate.Value.Date.Add(x.DocumentTime.Value.TimeOfDay));

                var creationActDateTime = creationActDateTimeList
                    .OrderBy(x => x.Date)
                    .ThenBy(x => x.TimeOfDay)
                    ?.FirstOrDefault();

                // Дата и время проведения КНМ
                var actCheckPeriod = this.actCheckPeriods.OrderBy(x => x.DateCheck).FirstOrDefault();

                var holdingDateTime = actCheckPeriod?.DateCheck != null && actCheckPeriod?.DateStart != null
                    ? actCheckPeriod.DateCheck.Value.Date.Add(actCheckPeriod.DateStart.Value.TimeOfDay)
                    : default(DateTime?);

                var actCheck = this.actChecks.FirstOrDefault();

                // ФИО подписавшего акт
                var signer = actCheck.Signer;
                var fioSigner = signer != null && this.actChecks.All(x => x.Signer?.Id == signer.Id)
                    ? signer.Fio
                    : null;

                var signerPositionDictVersionId = GetMemberInspectorPositionDictId(signer, decision);

                // Факт устранения выявленного нарушения
                bool? isViolationResolved = true;
                string isViolationResolvedNote = null;

                var actRemovalDomain = this.Container.ResolveDomain<ActRemoval>();
                var actRemovalViolationDomain = this.Container.ResolveDomain<ActRemovalViolation>();
                var violStageDomain = this.Container.ResolveDomain<InspectionGjiViolStage>();
                var prescriptionDomain = this.Container.Resolve<IDomainService<Prescription>>();
                var documentGjiChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();

                using (this.Container.Using(prescriptionDomain, 
                                   actRemovalDomain, 
                                   actRemovalViolationDomain, 
                                   documentGjiChildrenDomain, 
                                   violStageDomain))
                {
                    var violations = violStageDomain.GetAll()
                        .Where(x => this.actCheckIds.Contains(x.Document.Id) && x.TypeViolationStage == TypeViolationStage.Detection);

                    var violationsRemoval = violStageDomain.GetAll()
                        .Where(x => violations.Any(y => y.InspectionViolation.Id == x.InspectionViolation.Id && y.Id != x.Id)
                            && x.TypeViolationStage == TypeViolationStage.Removal)
                        .Join(actRemovalDomain.GetAll(),
                            stage => stage.Document,
                            act => act,
                            (stage, act) => new { stage.InspectionViolation, ActRemoval = act })
                        .Where(x => x.ActRemoval.Inspection.Id == decision.Inspection.Id)
                        .Join(documentGjiChildrenDomain.GetAll(),
                            firstJoin => firstJoin.ActRemoval,
                            child => child.Children,
                            (firstJoin, child) =>
                                new
                                {
                                    firstJoin.InspectionViolation,
                                    firstJoin.ActRemoval,
                                    child.Parent
                                })
                        .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription && 
                            x.Parent.Inspection.Id == decision.Inspection.Id)
                        .Select(x => new
                        {
                            x.InspectionViolation,
                            ActRemoval = new { x.ActRemoval, x.Parent.DocumentNumber, x.Parent.DocumentDate }
                        })
                        .ToList();

                    var prescriptionViols = violationsRemoval
                        .GroupBy(x => x.InspectionViolation, y=> y.ActRemoval)
                        .Select(x =>
                        {
                            var lastDoc = x.OrderByDescending(y => y.DocumentDate)
                                .Select(y=> y).FirstOrDefault();

                            return new
                            {
                                InspectionViolation = x.Key,
                                lastDoc.ActRemoval,
                                PrescriptionInfo = $"№{lastDoc.DocumentNumber} от {lastDoc.DocumentDate.ToDateTime().ToShortDateString()}"
                            };
                        }).ToList();
                    
                    if (prescriptionViols.Count == 0 || prescriptionViols.Count != violations.Count())
                    {
                        isViolationResolved = null;
                    }
                    else
                    {
                        var isViolationResolvedNoteList = new List<string>();
                        foreach (var viol in prescriptionViols)
                        {
                            var typeRemoval = viol.ActRemoval.TypeRemoval;

                            switch (typeRemoval)
                            {
                                case YesNoNotSet.No:
                                    isViolationResolved = false;
                                    break;

                                case YesNoNotSet.NotSet:
                                    isViolationResolved = null;
                                    break;

                                case YesNoNotSet.Yes:
                                    isViolationResolvedNoteList.Add($"Предписание {viol.PrescriptionInfo} исполнено;");
                                    break;
                            }

                            if (isViolationResolved == false || isViolationResolved == null)
                            {
                                break;
                            }
                        }

                        if (isViolationResolved.HasValue && isViolationResolved.Value)
                        {
                            isViolationResolvedNote = string.Join(" ", isViolationResolvedNoteList);
                        }
                    }
                }

                // Сведения об ознакомлении контролируемых лиц с результатами КНМ
                var firstAcquaintState = actCheck.AcquaintState;
                var acquaintState = firstAcquaintState != null && this.actChecks.All(x => x.AcquaintState == firstAcquaintState)
                    ? firstAcquaintState
                    : null;

                string fioReader = null;
                string titleReader = null;
                string acquaintanceTypeId = null;

                switch (acquaintState)
                {
                    case AcquaintState.Acquainted:

                        var acquaintedPerson = actCheck.AcquaintedPerson;
                        var acquaintedPersonTitle = actCheck.AcquaintedPersonTitle;

                        fioReader = !string.IsNullOrEmpty(acquaintedPerson)
                            && this.actChecks.All(x => string.Compare(x.AcquaintedPerson,
                                acquaintedPerson,
                                CultureInfo.CurrentCulture,
                                CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
                                ? acquaintedPerson
                                : null;

                        titleReader = fioReader != null && !string.IsNullOrEmpty(acquaintedPersonTitle)
                            && this.actChecks.All(x => string.Compare(x.AcquaintedPersonTitle,
                                acquaintedPersonTitle,
                                CultureInfo.CurrentCulture,
                                CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
                                ? acquaintedPersonTitle
                                : null;

                        acquaintanceTypeId = "1";
                        break;

                    case AcquaintState.NotAcquainted:

                        titleReader = null;
                        acquaintanceTypeId = "2";
                        break;

                    case AcquaintState.RefuseToAcquaint:
                        fioReader = actCheck.RefusedToAcquaintPerson;
                        acquaintanceTypeId = "3";
                        break;
                }

                // Документ со вкладки "Приложения" связанного акта
                ActCheckAnnex actCheckAnnex = null;
                if (this.actCheckIds.Length == 1)
                {
                    var actCheckAnnexDomain = this.Container.ResolveDomain<ActCheckAnnex>();
                    using (this.Container.Using(actCheckAnnexDomain))
                    {
                        actCheckAnnex = actCheckAnnexDomain
                            .GetAll()
                            .FirstOrDefault(x => x.ActCheck != null &&
                                x.File != null &&
                                x.SendFileToErknm == YesNoNotSet.Yes &&
                                x.ErknmGuid == null &&
                                this.actCheckIds.FirstOrDefault() == x.ActCheck.Id);
                    }
                }

                var actDocumentGjiInspectorsByInspector = this.actDocumentGjiInspectors
                    .GroupBy(x => x.Inspector)
                    .ToArray();
                
                var durationDays = this.actCheckIds.Count() == 1
                    ? this.actCheckPeriods.OrderBy(x => x.ObjectCreateDate).FirstOrDefault()?.DurationDays?.ToString()
                    : decision.CountDays?.ToString();

                var durationHours = this.actCheckIds.Count() == 1
                    ? this.actCheckPeriods.OrderBy(x => x.ObjectCreateDate).FirstOrDefault()?.DurationHours?.ToString()
                    : decision.CountHours?.ToString();

                if (isInsert)
                {
                    var actGuid = this.GetErknmGuid(this.actChecks, typeof(ActCheck));

                    actCorrectionDto.SubjectActInsert = new ISubjectActInsert
                    {
                        numberAct = numberAct,
                        creationActDateTime = creationActDateTime ?? DateTime.MinValue,
                        holdingDateTime = holdingDateTime ?? DateTime.MinValue,
                        fioSigner = fioSigner,
                        durationDays = durationDays,
                        durationHours = durationHours,
                        isViolationResolved = isViolationResolved ?? false,
                        isViolationResolvedNote = isViolationResolvedNote,
                        acquaintanceTypeId = acquaintanceTypeId,
                        fioReader = fioReader,
                        titleReader = titleReader,
                        creationActDateTimeSpecified = creationActDateTime.HasValue,
                        holdingDateTimeSpecified = holdingDateTime.HasValue,
                        isViolationResolvedSpecified = isViolationResolved.HasValue,
                        guid = actGuid,
                        ISubjectGuid = decision.OrganizationErknmGuid,
                        titleSigner = fioSigner != null
                            ? new IDictionary
                            {
                                dictId = this.erknmIntegrationConfig.InspectorPositionId,
                                dictVersionId = signerPositionDictVersionId
                            }
                            : null,
                        knoInspectors = actDocumentGjiInspectorsByInspector.Select(x =>
                            {
                                var guid = this.GetErknmGuid(x, typeof(DocumentGjiInspector));
                                var inspector = x.Key;

                                return new IInspector
                                {
                                    fullName = inspector.Fio,
                                    guid = guid,
                                    position = new IDictionary
                                    {
                                        dictId = this.erknmIntegrationConfig.InspectorPositionId,
                                        dictVersionId = this.GetMemberInspectorPositionDictId(inspector, decision)
                                    }
                                };
                            }
                        ).ToArray(),
                        document = actCheckAnnex != null
                            ? new IDocument
                            {
                                guid = this.GetErknmGuid(actCheckAnnex, typeof(ActCheckAnnex)),
                                attachments = actCheckAnnex.File != null 
                                    ? this.InitAttachment(actCheckAnnex, actCheckAnnex.File, actCheckAnnex.GetType()) 
                                    : null
                            }
                            : null
                    };
                }
                else
                {
                    var actGuid = this.actChecks.FirstOrDefault(x => x.ErknmGuid != null).ErknmGuid;

                    // Обновить ErknmGuid для актов, которые еще не были отправлены
                    var notSentActChecks = this.actChecks.Where(x => x.ErknmGuid == null);
                    this.UpdateErknmGuid(notSentActChecks, typeof(ActCheck), actGuid);

                    actCorrectionDto.SubjectActUpdate = new ISubjectActUpdate
                    {
                        acquaintanceTypeId = acquaintanceTypeId,
                        numberAct = numberAct,
                        creationActDateTime = creationActDateTime ?? DateTime.MinValue,
                        holdingDateTime = holdingDateTime ?? DateTime.MinValue,
                        fioSigner = fioSigner,
                        durationDays = durationDays,
                        durationHours = durationHours,
                        isViolationResolved = isViolationResolved ?? false,
                        isViolationResolvedNote = isViolationResolvedNote,
                        fioReader = fioReader,
                        titleReader = titleReader,
                        creationActDateTimeSpecified = creationActDateTime.HasValue,
                        holdingDateTimeSpecified = holdingDateTime.HasValue,
                        isViolationResolvedSpecified = isViolationResolved.HasValue,
                        guid = actGuid,
                        ISubjectGuid = decision.OrganizationErknmGuid,
                        isDelSpecified = true
                    };

                    actCorrectionDto.ActTitleSignerUpdate = new IActTitleSignerUpdate
                    {
                        dictId = this.erknmIntegrationConfig.InspectorPositionId,
                        dictVersionId = signerPositionDictVersionId,
                        IActGuid = actGuid,
                        isDelSpecified = true
                    };

                    // Проверяем, что в каждом акте проверки содержится одинаковый набор инспекторов
                    if (actDocumentGjiInspectorsByInspector.Select(x => x.Count()).
                        All(x => x == this.actCheckIds.Length))
                    {
                        actCorrectionDto.ActKnoInspectorsInsert = actDocumentGjiInspectorsByInspector
                            .Select(x =>
                            {
                                var notSentDocumentGjiInspectors = x.Where(y => y.ErknmGuid == null).ToArray();
                                if (!notSentDocumentGjiInspectors.Any())
                                {
                                    return null;
                                }

                                var sentDocumentGjiInspectors = x.Where(this.HasErknmGuid);
                                var documentGjiInspectorGuid = sentDocumentGjiInspectors.FirstOrDefault()?.ErknmGuid;

                                var guid = this.AddToErknmEntityList(notSentDocumentGjiInspectors, typeof(DocumentGjiInspector), documentGjiInspectorGuid);
                                var inspector = x.Key;

                                return new IActKnoInspectorsInsert
                                {
                                    fullName = inspector.Fio,
                                    guid = guid,
                                    IActGuid = actGuid,
                                    position = new IDictionary
                                    {
                                        dictId = this.erknmIntegrationConfig.InspectorPositionId,
                                        dictVersionId = this.GetMemberInspectorPositionDictId(inspector, decision)
                                    }
                                };
                            }).Where(x => x != null)
                            .ToArray();
                    }

                    actCorrectionDto.ActDocumentInsert = actCheckAnnex != null
                        ? new IActDocumentInsert
                        {
                            guid = this.GetErknmGuid(actCheckAnnex, typeof(ActCheckAnnex)),
                            IActGuid = actGuid,
                            attachments = actCheckAnnex.File != null 
                                ? this.InitAttachment(actCheckAnnex, actCheckAnnex.File, actCheckAnnex.GetType()) 
                                : null
                        }
                        : null;
                }
            }

            return actCorrectionDto;
        }
        
        /// <summary>
        /// Получить guid версии записи справочники должностей инспекторов
        /// </summary>
        /// <param name="inspector"></param>
        /// <param name="decision"></param>
        /// <returns></returns>
        private string GetMemberInspectorPositionDictId(Inspector inspector, Decision decision)
        {
            return inspector?.InspectorPosition != null
                ? this.controlTypeInspectorPositions
                    .FirstOrDefault(x =>
                        x.ControlType?.Id == decision.ControlType?.Id &&
                        x.InspectorPosition.Id == inspector.InspectorPosition.Id &&
                        x.IsMember)?.ErvkId
                : null;
        }
    }
}