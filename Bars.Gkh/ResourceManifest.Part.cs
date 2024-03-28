namespace Bars.Gkh
{
    using System;

    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.Gkh.Config.Impl;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Administration.SystemDataTransfer;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.Suggestion;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Administration.EmailMessage;
    using Bars.Gkh.Enums.Administration.FormatDataExport;
    using Bars.Gkh.Enums.BasePassport;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Enums.EfficiencyRating;
    using Bars.Gkh.Enums.Licensing;
    using Bars.Gkh.Enums.Notification;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.MetaValueConstructor.Enums;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Modules.Gkh1468.Enums;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.SignalR;
    using Bars.Gkh.SystemDataTransfer.Enums;
    using Bars.Gkh.Utils;

    using Enums.Decisions;
    using RegOperator.Enums.PaymentDocumentOptions;

    using CourtType = Bars.Gkh.Modules.ClaimWork.Enums.CourtType;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/Gkh/Config.js", new ConfigContent());

            container.Add("libs/B4/enums/TypeManOrgTypeDocLicense.js", new ExtJsEnumResource<TypeManOrgTypeDocLicense>("B4.enums.TypeManOrgTypeDocLicense")); 
            container.Add("libs/B4/enums/TypeCancelationQualCertificate.js", new ExtJsEnumResource<TypeCancelationQualCertificate>("B4.enums.TypeCancelationQualCertificate"));
            container.Add("libs/B4/enums/TypePersonDisqualification.js", new ExtJsEnumResource<TypePersonDisqualification>("B4.enums.TypePersonDisqualification"));
            container.Add("libs/B4/enums/QualificationDocumentType.js", new ExtJsEnumResource<QualificationDocumentType>("B4.enums.QualificationDocumentType"));
            container.Add("libs/B4/enums/RequestSupplyMethod.js", new ExtJsEnumResource<RequestSupplyMethod>("B4.enums.RequestSupplyMethod"));
            container.Add("libs/B4/enums/DebtCalcMethod.js", new ExtJsEnumResource<DebtCalcMethod>("B4.enums.DebtCalcMethod"));

            container.Add("libs/B4/enums/TypeIdentityDocument.js", new ExtJsEnumResource<TypeIdentityDocument>("B4.enums.TypeIdentityDocument"));
            container.Add("libs/B4/enums/TypeEntrepreneurship.js", new ExtJsEnumResource<TypeEntrepreneurship>("B4.enums.TypeEntrepreneurship"));
            container.Add("libs/B4/enums/TypeWork.js", new ExtJsEnumResource<TypeWork>("B4.enums.TypeWork"));
            container.Add("libs/B4/enums/TypeRoof.js", new ExtJsEnumResource<TypeRoof>("B4.enums.TypeRoof"));
            container.Add("libs/B4/enums/TypeManagementManOrg.js", new ExtJsEnumResource<TypeManagementManOrg>("B4.enums.TypeManagementManOrg"));
            container.Add("libs/B4/enums/TypeHouse.js", new ExtJsEnumResource<TypeHouse>("B4.enums.TypeHouse"));
            container.Add("libs/B4/enums/TypeDocument.js", new ExtJsEnumResource<TypeDocument>("B4.enums.TypeDocument"));
            container.Add("libs/B4/enums/TypeAuthor.js", new ExtJsEnumResource<TypeAuthor>("B4.enums.TypeAuthor"));
            container.Add("libs/B4/enums/TypeAssessment.js", new ExtJsEnumResource<TypeAssessment>("B4.enums.TypeAssessment"));
            container.Add("libs/B4/enums/TypeAccounting.js", new ExtJsEnumResource<TypeAccounting>("B4.enums.TypeAccounting"));
            container.Add("libs/B4/enums/OrgStateRole.js", new ExtJsEnumResource<OrgStateRole>("B4.enums.OrgStateRole"));
            container.Add("libs/B4/enums/KindRightToObject.js", new ExtJsEnumResource<KindRightToObject>("B4.enums.KindRightToObject"));
            container.Add("libs/B4/enums/ImagesGroup.js", new ExtJsEnumResource<ImagesGroup>("B4.enums.ImagesGroup"));
            container.Add("libs/B4/enums/HeatingSystem.js", new ExtJsEnumResource<HeatingSystem>("B4.enums.HeatingSystem"));
            container.Add("libs/B4/enums/GroundsTermination.js", new ExtJsEnumResource<GroundsTermination>("B4.enums.GroundsTermination"));
            container.Add("libs/B4/enums/CouncilResult.js", new ExtJsEnumResource<CouncilResult>("B4.enums.CouncilResult"));
            container.Add("libs/B4/enums/ContragentState.js", new ExtJsEnumResource<ContragentState>("B4.enums.ContragentState"));
            container.Add("libs/B4/enums/ConditionHouse.js", new ExtJsEnumResource<ConditionHouse>("B4.enums.ConditionHouse"));
            container.Add("libs/B4/enums/YesNoNotSet.js", new ExtJsEnumResource<YesNoNotSet>("B4.enums.YesNoNotSet"));
            container.Add("libs/B4/enums/HasValuesNotSet.js", new ExtJsEnumResource<HasValuesNotSet>("B4.enums.HasValuesNotSet"));
            container.Add("libs/B4/enums/ManOrgContractOwnersFoundation.js", new ExtJsEnumResource<ManOrgContractOwnersFoundation>("B4.enums.ManOrgContractOwnersFoundation"));
            container.Add("libs/B4/enums/ManOrgTransferContractFoundation.js", new ExtJsEnumResource<ManOrgTransferContractFoundation>("B4.enums.ManOrgTransferContractFoundation"));
            container.Add("libs/B4/enums/ManOrgJskTsjContractFoundation.js", new ExtJsEnumResource<ManOrgJskTsjContractFoundation>("B4.enums.ManOrgJskTsjContractFoundation"));
            container.Add("libs/B4/enums/StateResettlementProgram.js", new ExtJsEnumResource<StateResettlementProgram>("B4.enums.StateResettlementProgram"));
            container.Add("libs/B4/enums/PolicyAction.js", new ExtJsEnumResource<PolicyAction>("B4.enums.PolicyAction"));
            container.Add("libs/B4/enums/VisibilityResettlementProgram.js", new ExtJsEnumResource<VisibilityResettlementProgram>("B4.enums.VisibilityResettlementProgram"));
            container.Add("libs/B4/enums/TypeResettlementProgram.js", new ExtJsEnumResource<TypeResettlementProgram>("B4.enums.TypeResettlementProgram"));
            container.Add("libs/B4/enums/TypeContractManOrgRealObj.js", new ExtJsEnumResource<TypeContractManOrg>("B4.enums.TypeContractManOrgRealObj"));
            container.Add("libs/B4/enums/TypeCouncillors.js", new ExtJsEnumResource<TypeCouncillors>("B4.enums.TypeCouncillors"));
            container.Add("libs/B4/enums/TypeWorkplace.js", new ExtJsEnumResource<TypeWorkplace>("B4.enums.TypeWorkplace"));
            container.Add("libs/B4/enums/TypeMode.js", new ExtJsEnumResource<TypeMode>("B4.enums.TypeMode"));
            container.Add("libs/B4/enums/TypeDayOfWeek.js", new ExtJsEnumResource<TypeDayOfWeek>("B4.enums.TypeDayOfWeek"));
            container.Add("libs/B4/enums/TypeServiceOrg.js", new ExtJsEnumResource<TypeServiceOrg>("B4.enums.TypeServiceOrg"));
            container.Add("libs/B4/enums/YesNo.js", new ExtJsEnumResource<YesNo>("B4.enums.YesNo"));
            container.Add("libs/B4/enums/TypeGroup.js", new ExtJsEnumResource<TypeGroup>("B4.enums.TypeGroup"));
            container.Add("libs/B4/enums/TypeEngineerSystem.js", new ExtJsEnumResource<TypeEngineerSystem>("B4.enums.TypeEngineerSystem"));
            container.Add("libs/B4/enums/TypeCharacteristics.js", new ExtJsEnumResource<TypeCharacteristics>("B4.enums.TypeCharacteristics"));
            container.Add("libs/B4/enums/MethodFormFundCr.js", new ExtJsEnumResource<MethodFormFundCr>("B4.enums.MethodFormFundCr"));
            container.Add("libs/B4/enums/OwnerType.js", new ExtJsEnumResource<OwnerType>("B4.enums.OwnerType"));
            container.Add("libs/B4/enums/TypeMunicipality.js", new ExtJsEnumResource<TypeMunicipality>("B4.enums.TypeMunicipality"));
            container.Add("libs/B4/enums/MoLevel.js", new ExtJsEnumResource<MoLevel>("B4.enums.MoLevel"));
            container.Add("libs/B4/enums/DebtorState.js", new ExtJsEnumResource<DebtorState>("B4.enums.DebtorState"));
            container.Add("libs/B4/enums/TypeMunicipality.js", new ExtJsEnumResource<TypeMunicipality>("B4.enums.TypeMunicipality"));

            container.Add("libs/B4/enums/GisDictionaryType.js", new ExtJsEnumResource<GisDictionaryType>("B4.enums.GisDictionaryType"));

            container.Add("libs/B4/enums/TypeCompleteWork.js", new ExtJsEnumResource<TypeCompleteWork>("B4.enums.TypeCompleteWork"));
            container.Add("libs/B4/enums/TypeRepair.js", new ExtJsEnumResource<TypeRepair>("B4.enums.TypeRepair"));
            container.Add("libs/B4/enums/VolumeRepair.js", new ExtJsEnumResource<VolumeRepair>("B4.enums.VolumeRepair"));

            container.Add("libs/B4/enums/TypeJurPerson.js", new ExtJsEnumResource<TypeJurPerson>("B4.enums.TypeJurPerson"));

            container.Add("libs/B4/enums/DebtorType.js", new ExtJsEnumResource<DebtorType>("B4.enums.DebtorType"));

            container.Add("libs/B4/enums/PretensionType.js", new ExtJsEnumResource<PretensionType>("B4.enums.PretensionType"));

            container.Add("libs/B4/enums/ContragentType.js", new ExtJsEnumResource<ContragentType>("B4.enums.ContragentType"));
            container.Add("libs/B4/enums/ExecutorType.js", new ExtJsEnumResource<ExecutorType>("B4.enums.ExecutorType"));
            container.Add("libs/B4/enums/MeteringDeviceGroup.js", new ExtJsEnumResource<MeteringDeviceGroup>("B4.enums.MeteringDeviceGroup"));
            container.Add("libs/B4/enums/TypePresence.js", new ExtJsEnumResource<TypePresence>("B4.enums.TypePresence"));
            container.Add("libs/B4/enums/WorkpriceMoLevel.js", new ExtJsEnumResource<WorkpriceMoLevel>("B4.enums.WorkpriceMoLevel"));
            container.Add("libs/B4/enums/realty/RoomType.js", new ExtJsEnumResource<RoomType>("B4.enums.realty.RoomType"));

            container.RegisterExtJsModel<Room>().Controller("Room").AddProperty<DateTime>("ObjectCreateDate");
            container.RegisterExtJsModel<EntityLogLight>().Controller("EntityLogLight").Exclude(x => x.ObjectCreateDate).AddProperty<object>("ObjectCreateDate");
            
            container.Add("libs/B4/enums/CrFundFormationDecisionType.js", new ExtJsEnumResource<CrFundFormationDecisionType>("B4.enums.CrFundFormationDecisionType"));
            container.Add("libs/B4/enums/MkdManagementDecisionType.js", new ExtJsEnumResource<MkdManagementDecisionType>("B4.enums.MkdManagementDecisionType"));
            container.Add("libs/B4/enums/AccountOwnerDecisionType.js", new ExtJsEnumResource<AccountOwnerDecisionType>("B4.enums.AccountOwnerDecisionType"));
            container.Add("libs/B4/enums/TsjInfoType.js", new ExtJsEnumResource<TsjInfoType>("B4.enums.TsjInfoType"));
            container.Add("libs/B4/enums/ContractStopReasonEnum.js", new ExtJsEnumResource<ContractStopReasonEnum>("B4.enums.ContractStopReasonEnum"));
            container.Add("libs/B4/enums/SymbolsLocation.js", new ExtJsEnumResource<SymbolsLocation>("B4.enums.SymbolsLocation"));
            container.Add("libs/B4/enums/ClaimWork/CourtType.js", new ExtJsEnumResource<Enums.ClaimWork.CourtType>("B4.enums.ClaimWork.CourtType"));
            container.Add("libs/B4/enums/ResOrgReason.js", new ExtJsEnumResource<ResOrgReason>("B4.enums.ResOrgReason"));
            container.Add("libs/B4/enums/HeatingSystemType.js", new ExtJsEnumResource<HeatingSystemType>("B4.enums.HeatingSystemType"));
            container.Add("libs/B4/enums/SchemeConnectionType.js", new ExtJsEnumResource<SchemeConnectionType>("B4.enums.SchemeConnectionType"));
            container.Add("libs/B4/enums/TypeContractPart.js", new ExtJsEnumResource<TypeContractPart>("B4.enums.TypeContractPart"));
            container.Add("libs/B4/enums/TypeContactPerson.js", new ExtJsEnumResource<TypeContactPerson>("B4.enums.TypeContactPerson"));
            container.Add("libs/B4/enums/TypeOwnerContract.js", new ExtJsEnumResource<TypeOwnerContract>("B4.enums.TypeOwnerContract"));
            container.Add("libs/B4/enums/OwnerDocumentType.js", new ExtJsEnumResource<OwnerDocumentType>("B4.enums.OwnerDocumentType"));
            container.Add("libs/B4/enums/GenderR.js", new ExtJsEnumResource<GenderR>("B4.enums.GenderR"));

            container.Add("libs/B4/enums/AntennaRange.js", new ExtJsEnumResource<AntennaRange>("B4.enums.AntennaRange"));
            container.Add("libs/B4/enums/AntennaReason.js", new ExtJsEnumResource<AntennaReason>("B4.enums.AntennaReason"));
            container.Add("libs/B4/enums/YesNoMinus.js", new ExtJsEnumResource<YesNoMinus>("B4.enums.YesNoMinus"));

            container.Add("libs/B4/enums/CommercialMeteringResourceType.js", 
                new ExtJsEnumResource<CommercialMeteringResourceType>("B4.enums.CommercialMeteringResourceType"));
            
            container.Add("libs/B4/enums/ClaimWork/TypeManagementManOrg.js", new ExtJsEnumResource<TypeManagementManOrg>("B4.enums.ClaimWork.TypeManagementManOrg"));
            container.Add("libs/B4/enums/ClaimWork/TypeLawsuitDocument.js", new ExtJsEnumResource<TypeLawsuitDocument>("B4.enums.ClaimWork.TypeLawsuitDocument"));
            container.Add("libs/B4/enums/ClaimWork/CollectDebtFrom.js", new ExtJsEnumResource<CollectDebtFrom>("B4.enums.ClaimWork.CollectDebtFrom"));

            var transition = new ExtJsModelResource<Transition>("B4.model.suggestion.Transition");
            transition.GetModelMeta().Controller("Transition");
            container.Add("libs/B4/model/suggestion/Transition.js", transition);

            var suggHistory = new ExtJsModelResource<CitizenSuggestionHistory>("B4.model.suggestion.History");
            suggHistory.GetModelMeta().Controller("CitizenSuggestionHistory").AddProperty<string>("ExecutorName");
            container.Add("libs/B4/model/suggestion/History.js", suggHistory);

            container.Add("libs/B4/enums/efficiencyrating/DataValueType.js", new ExtJsEnumResource<DataValueType>("B4.enums.efficiencyrating.DataValueType"));
            container.Add("libs/B4/enums/efficiencyrating/DataMetaObjectType.js", new ExtJsEnumResource<DataMetaObjectType>("B4.enums.efficiencyrating.DataMetaObjectType"));
            container.Add("libs/B4/enums/efficiencyrating/AnaliticsLevel.js", new ExtJsEnumResource<AnaliticsLevel>("B4.enums.efficiencyrating.AnaliticsLevel"));
            container.Add("libs/B4/enums/efficiencyrating/Category.js", new ExtJsEnumResource<Category>("B4.enums.efficiencyrating.Category"));
            container.Add("libs/B4/enums/efficiencyrating/DiagramType.js", new ExtJsEnumResource<DiagramType>("B4.enums.efficiencyrating.DiagramType"));
            container.Add("libs/B4/enums/efficiencyrating/ViewParam.js", new ExtJsEnumResource<ViewParam>("B4.enums.efficiencyrating.ViewParam"));

            container.Add("libs/B4/enums/RequestSMEVType.js", new ExtJsEnumResource<RequestSMEVType>("B4.enums.RequestSMEVType"));
            container.Add("libs/B4/enums/SMEVRequestState.js", new ExtJsEnumResource<SMEVRequestState>("B4.enums.SMEVRequestState"));
            container.Add("libs/B4/enums/OperatorExportFormat.js", new ExtJsEnumResource<OperatorExportFormat>("B4.enums.OperatorExportFormat"));

            this.RegisterModels(container);

            this.RegisterEnums(container);

            this.RegisterServices(container);

            this.RegisterInlineDicts(container);
        }

        private void RegisterEnums(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<FillType>();
            container.RegisterExtJsEnum<TypeCommunalResource>();
            container.RegisterExtJsEnum<TypeHousingResource>();
            container.RegisterExtJsEnum<TypeServiceGis>();
            container.RegisterExtJsEnum<ManagementContractServiceType>();

            container.RegisterExtJsEnum<Month>();
            container.RegisterExtJsEnum<YesNoNotApplicable>();

            container.RegisterExtJsEnum<AttributeType>();
            container.RegisterExtJsEnum<PriceCalculateBy>();
            container.RegisterExtJsEnum<DocumentFormationKind>();
            container.RegisterExtJsEnum<ZVSPCourtDecision>();

            container.RegisterExtJsEnum<Archiving>();
            container.RegisterExtJsEnum<GroupingOption>();
            container.RegisterExtJsEnum<FileFormat>();

            container.RegisterExtJsEnum<AccountManagementType>();
            container.RegisterExtJsEnum<NormativeDocCategory>();
            container.RegisterExtJsEnum<PeriodKind>();
            container.RegisterExtJsEnum<ConditionStructElement>();

            container.RegisterExtJsEnum<RequirementSatisfaction>();
            container.RegisterExtJsEnum<ActViolIdentificationType>();
            container.RegisterExtJsEnum<FactOfReceiving>();
            container.RegisterGkhEnum<LawsuitCollectionDebtDocumentType>();
            container.RegisterExtJsEnum<LawsuitCollectionDebtReasonStoppedType>();
            container.RegisterExtJsEnum<LawsuitCollectionDebtType>();
            container.RegisterExtJsEnum<LawsuitConsiderationType>();
            container.RegisterExtJsEnum<LawsuitCourtType>();
            container.RegisterExtJsEnum<LawsuitDocumentType>();
            container.RegisterExtJsEnum<LawsuitFactInitiationType>();
            container.RegisterGkhEnum<LawsuitResultConsideration>();
            container.RegisterExtJsEnum<JurInstitutionType>();
            container.RegisterExtJsEnum<CourtType>();
            container.RegisterExtJsEnum<FactOfSigning>();
            container.RegisterExtJsEnum<ClaimWorkDocumentType>();
            container.RegisterExtJsEnum<ClaimWorkTypeBase>();
            container.RegisterExtJsEnum<AgentPIRDebtorStatus>();
            container.RegisterExtJsEnum<AgentPIRDocumentType>();

            container.RegisterExtJsEnum<FactOfSigning>();
            container.RegisterExtJsEnum<LogOperationType>();
            container.RegisterExtJsEnum<Gender>();

            container.RegisterExtJsEnum<WorkAssignment>();
            container.RegisterGkhEnum(CrFundFormationType.Unknown);
            container.RegisterExtJsEnum<UseFiasHouseIdentification>();

            container.RegisterExtJsEnum<ExecutionActionStatus>();
            container.RegisterExtJsEnum<ManOrgSetPaymentsOwnersFoundation>();
            container.RegisterExtJsEnum<LiftAvailabilityDevices>();
            container.RegisterExtJsEnum<LicenseRequestType>();

            container.RegisterExtJsEnum<Quarter>();
            container.RegisterExtJsEnum<VoteForm>();
            container.RegisterExtJsEnum<ServiceDetailSectionType>();
            container.RegisterExtJsEnum<FormGovernmentServiceType>();
            container.RegisterExtJsEnum<FormadDataExportRemoteStatus>();
            container.RegisterExtJsEnum<FormatDataExportProviderType>();
            container.RegisterGkhEnum(FormatDataExportType.All);

            container.RegisterExtJsEnum<DataTransferOperationType>();
            container.RegisterExtJsEnum<TransferingState>();
            container.RegisterGkhEnum(FormatDataExportStatus.Default);
            container.RegisterExtJsEnum<SendMethod>();
            container.RegisterGkhEnum(RoomOwnershipType.MunicipalAdm);
            container.RegisterGkhEnum(
                TypeManOrgTerminationLicense.DecisionAuthorizedBody,
                TypeManOrgTerminationLicense.DecisionStateControlBody,
                TypeManOrgTerminationLicense.InputError);

            container.RegisterExtJsEnum<FormatDataExportEntityState>();
            container.RegisterExtJsEnum<FormatDataExportState>();
            container.RegisterExtJsEnum<FormatDataExportObjectType>();
            container.RegisterExtJsEnum<EntityType>();
            container.RegisterExtJsEnum<TaskPeriodType>();

            container.RegisterExtJsEnum<ActivityStageType>();
            container.RegisterExtJsEnum<ActivityStageOwner>();
            container.RegisterExtJsEnum<TimeZoneType>();
            container.RegisterExtJsEnum<PaymentDocumentEmailOwnerType>();
            container.RegisterExtJsEnum<GkhStructureType>();
            container.RegisterExtJsEnum<RequestRPGUState>();
            container.RegisterExtJsEnum<RequestRPGUType>();

            container.RegisterGkhEnum("ActionKindChangeLog", ActionKind.Delete);
            container.RegisterGkhEnum(RestructDebtStatus.NotSet);
            container.RegisterExtJsEnum<EntityHistoryType>();
            container.RegisterGkhEnum(RestructDebtDocumentState.NotSet);
            container.RegisterExtJsEnum<PretensionType>();

            container.RegisterExtJsEnum<AntennaRange>();
            container.RegisterExtJsEnum<AntennaReason>();
            container.RegisterExtJsEnum<YesNoMinus>();

            container.RegisterExtJsEnum<DebtCalc>();
            container.RegisterExtJsEnum<PenaltyCharge>();
            container.RegisterExtJsEnum<ButtonType>();
            container.RegisterExtJsEnum<TypeEditor>();
            container.RegisterExtJsEnum<CategoryInformationNpa>();
            container.RegisterExtJsEnum<ActionLevelNormativeAct>();
            container.RegisterExtJsEnum<FormVoting>();
            container.RegisterExtJsEnum<LegalityMeeting>();
            container.RegisterExtJsEnum<VotingStatus>();
            container.RegisterExtJsEnum<NpaStatus>();
            container.RegisterExtJsEnum<TaskStatus>();
            container.RegisterExtJsEnum<EmailMessageType>();
            container.RegisterExtJsEnum<EmailSendStatus>();
        }

        private void RegisterServices(IResourceManifestContainer container)
        {
            container.Add("WS/Service.svc", "Bars.Gkh.dll/Bars.Gkh.Services.Service.svc");
            container.Add("WS/RestService.svc", "Bars.Gkh.dll/Bars.Gkh.Services.RestService.svc");
            container.Add("WS/Suggestion.svc", "Bars.Gkh.dll/Bars.Gkh.Services.Suggestion.svc");
            container.Add("WS/GlonassIntegService.svc", "Bars.Gkh.dll/Bars.Gkh.Services.GlonassIntegService.svc");
            container.Add("WS/DataTransfer.svc", "Bars.Gkh.dll/Bars.Gkh.Services.DataTransfer.svc");
        }

        private void RegisterModels(IResourceManifestContainer container)
        {
            container.RegisterExtJsModel<TableLock>().Controller("TableLock");

            container.RegisterExtJsModel<Entrance>().Controller("Entrance")
                .AddProperty<decimal?>("Tariff")
                .AddProperty<int?>("Number");

            container.RegisterExtJsModel<HousingFundMonitoringInfo>("housingfundmonitoring.HousingFundMonitoringInfo").Controller("HousingFundMonitoringInfo");
            container.RegisterExtJsModel<LicenseRegistrationReason>("dict.LicenseRegistrationReason").Controller("LicenseRegistrationReason");
            container.RegisterExtJsModel<LicenseRejectReason>("dict.LicenseRejectReason").Controller("LicenseRejectReason");
            container.RegisterExtJsModel<DataTransferIntegrationSession>("administration.DataTransferIntegrationSession").Controller("DataTransferIntegrationSession");
        }

        private void RegisterInlineDicts(IResourceManifestContainer container)
        {
            container.RegisterBaseDictController<TypeFloor>("Тип перекрытия",
                "Gkh.Dictionaries.TypeFloor",
                "B4.controller.dict.TypeFloor");

            container.RegisterBaseDictController<RiskCategory>("Категории риска", "Gkh.Dictionaries.RiskCategory", "B4.controller.dict.RiskCategory");
            container.RegisterBaseDictController<InspectorPositions>("Должности инспекторов", "GkhGji.Dict.InspectorPositions");
        }
    }
}