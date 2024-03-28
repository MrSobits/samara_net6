namespace Bars.GkhGji
{
    using System.Linq;

    using B4;
    using B4.Modules.ExtJs;

    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Voronezh.Enums;
    using Contracts.Enums;
    using Enums;
    
    using ControlType = Bars.GkhGji.Enums.ControlType;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/PrescriptionFamiliar.js", new ExtJsEnumResource<PrescriptionFamiliar>("B4.enums.PrescriptionFamiliar"));
            container.Add("libs/B4/enums/StatusAppealCitizens.js", new ExtJsEnumResource<StatusAppealCitizens>("B4.enums.StatusAppealCitizens"));
            container.Add("libs/B4/enums/TypeBaseJurPerson.js", new ExtJsEnumResource<TypeBaseJurPerson>("B4.enums.TypeBaseJurPerson"));
            container.Add("libs/B4/enums/TypeAgreementResult.js", new ExtJsEnumResource<TypeAgreementResult>("B4.enums.TypeAgreementResult"));
            container.Add("libs/B4/enums/TypeDisposalGji.js", new ExtJsEnumResource<TypeDisposalGji>("B4.enums.TypeDisposalGji"));
            container.Add("libs/B4/enums/TypeDocumentGji.js", new ExtJsEnumResource<TypeDocumentGji>("B4.enums.TypeDocumentGji"));
            container.Add("libs/B4/enums/TypeStage.js", new ExtJsEnumResource<TypeStage>("B4.enums.TypeStage"));
            container.Add("libs/B4/enums/TypeActCheckGji.js", new ExtJsEnumResource<TypeActCheckGji>("B4.enums.TypeActCheckGji"));
            container.Add("libs/B4/enums/TypeViolationStage.js", new ExtJsEnumResource<TypeViolationStage>("B4.enums.TypeViolationStage"));
            container.Add("libs/B4/enums/TypeCheck.js", new ExtJsEnumResource<TypeCheck>("B4.enums.TypeCheck"));
            container.Add("libs/B4/enums/PrescriptionState.js", new ExtJsEnumResource<PrescriptionState>("B4.enums.PrescriptionState"));
            container.Add("libs/B4/enums/TypePrescriptionExecution.js", new ExtJsEnumResource<TypePrescriptionExecution>("B4.enums.TypePrescriptionExecution"));
            container.Add("libs/B4/enums/DisputeResult.js", new ExtJsEnumResource<DisputeResult>("B4.enums.DisputeResult"));
            container.RegisterExtJsEnum<ReasonErpChecking>();
            container.Add("libs/B4/enums/LicStatementDocType.js", new ExtJsEnumResource<LicStatementDocType>("B4.enums.LicStatementDocType"));
            container.Add("libs/B4/enums/SurveyResult.js", new ExtJsEnumResource<SurveyResult>("B4.enums.SurveyResult"));
            container.Add("libs/B4/enums/ImageGroupSurveyGji.js", new ExtJsEnumResource<ImageGroupSurveyGji>("B4.enums.ImageGroupSurveyGji"));
            container.Add("libs/B4/enums/TypeInitiativeOrgGji.js", new ExtJsEnumResource<TypeInitiativeOrgGji>("B4.enums.TypeInitiativeOrgGji"));
            container.Add("libs/B4/enums/TypeDocumentPaidGji.js", new ExtJsEnumResource<TypeDocumentPaidGji>("B4.enums.TypeDocumentPaidGji"));
            container.Add("libs/B4/enums/LicStatementResult.js", new ExtJsEnumResource<LicStatementResult>("B4.enums.LicStatementResult"));
            container.Add("libs/B4/enums/TypeBaseDispHead.js", new ExtJsEnumResource<TypeBaseDispHead>("B4.enums.TypeBaseDispHead"));
            container.Add("libs/B4/enums/TypeBaseProsClaim.js", new ExtJsEnumResource<TypeBaseProsClaim>("B4.enums.TypeBaseProsClaim"));
            container.Add("libs/B4/enums/TypeCheck.js", new ExtJsEnumResource<TypeCheck>("B4.enums.TypeCheck"));
            container.Add("libs/B4/enums/TypeKindActivity.js", new ExtJsEnumResource<TypeKindActivity>("B4.enums.TypeKindActivity"));
            container.Add("libs/B4/enums/HeatSeasonDocType.js", new ExtJsEnumResource<HeatSeasonDocType>("B4.enums.HeatSeasonDocType"));
            container.Add("libs/B4/enums/TypeConclusion.js", new ExtJsEnumResource<TypeConclusion>("B4.enums.TypeConclusion"));
            container.Add("libs/B4/enums/TypeState.js", new ExtJsEnumResource<TypeState>("B4.enums.TypeState"));
            container.Add("libs/B4/enums/ResolutionAppealed.js", new ExtJsEnumResource<ResolutionAppealed>("B4.enums.ResolutionAppealed"));
            container.Add("libs/B4/enums/PersonInspection.js", new ExtJsEnumResource<PersonInspection>("B4.enums.PersonInspection"));
            container.Add("libs/B4/enums/TypeDefinitionAct.js", new ExtJsEnumResource<TypeDefinitionAct>("B4.enums.TypeDefinitionAct"));
            container.Add("libs/B4/enums/TypeDefinitionProtocol.js", new ExtJsEnumResource<TypeDefinitionProtocol>("B4.enums.TypeDefinitionProtocol"));
            container.Add("libs/B4/enums/TypeDefinitionProtocolMhc.js", new ExtJsEnumResource<TypeDefinitionProtocolMhc>("B4.enums.TypeDefinitionProtocolMhc"));
            container.Add("libs/B4/enums/TypeDefinitionResolution.js", new ExtJsEnumResource<TypeDefinitionResolution>("B4.enums.TypeDefinitionResolution"));
            container.Add("libs/B4/enums/InspectionGjiType.js", new ExtJsEnumResource<InspectionGjiType>("B4.enums.InspectionGjiType"));
            container.Add("libs/B4/enums/TypeDocumentInsCheck.js", new ExtJsEnumResource<TypeDocumentInsCheck>("B4.enums.TypeDocumentInsCheck"));
            container.Add("libs/B4/enums/TypeCorrespondent.js", new ExtJsEnumResource<TypeCorrespondent>("B4.enums.TypeCorrespondent"));
            container.Add("libs/B4/enums/TypeExecutantProtocolMvd.js", new ExtJsEnumResource<TypeExecutantProtocolMvd>("B4.enums.TypeExecutantProtocolMvd"));
            container.Add("libs/B4/enums/TypeTerminationBasement.js", new ExtJsEnumResource<TypeTerminationBasement>("B4.enums.TypeTerminationBasement"));
            container.Add("libs/B4/enums/TypeSupplierProtocol.js", new ExtJsEnumResource<TypeSupplierProtocol>("B4.enums.TypeSupplierProtocol"));
            container.Add("libs/B4/enums/TypeDefinitionProtocolProsecutor.js", new ExtJsEnumResource<TypeDefinitionProtocolProsecutor>("B4.enums.TypeDefinitionProtocolProsecutor"));
            container.Add("libs/B4/enums/CategoryReminder.js", new ExtJsEnumResource<CategoryReminder>("B4.enums.CategoryReminder"));
            container.Add("libs/B4/enums/TypeReminder.js", new ExtJsEnumResource<TypeReminder>("B4.enums.TypeReminder"));
            container.Add("libs/B4/enums/Accepting.js", new ExtJsEnumResource<Accepting>("B4.enums.Accepting"));
            container.Add("libs/B4/enums/TypeHeatInputObject.js", new ExtJsEnumResource<TypeHeatInputObject>("B4.enums.TypeHeatInputObject"));
            container.Add("libs/B4/enums/OSPDecisionType.js", new ExtJsEnumResource<OSPDecisionType>("B4.enums.OSPDecisionType"));
            container.Add("libs/B4/enums/TypeAnnex.js", new ExtJsEnumResource<TypeAnnex>("B4.enums.TypeAnnex"));
            container.Add("libs/B4/enums/EmailGjiType.js", new ExtJsEnumResource<EmailGjiType>("B4.enums.EmailGjiType"));

            container.Add("libs/B4/enums/PrescriptionCloseReason.js", new ExtJsEnumResource<PrescriptionCloseReason>("B4.enums.PrescriptionCloseReason"));
            container.Add("libs/B4/enums/PrescriptionDocType.js", new ExtJsEnumResource<PrescriptionDocType>("B4.enums.PrescriptionDocType"));
            container.Add("libs/B4/enums/TypePrescriptionCancel.js", new ExtJsEnumResource<TypePrescriptionCancel>("B4.enums.TypePrescriptionCancel"));
            container.Add("libs/B4/enums/TypeProlongation.js", new ExtJsEnumResource<TypeProlongation>("B4.enums.TypeProlongation"));
            container.Add("libs/B4/enums/AppealOperationType.js", new ExtJsEnumResource<AppealOperationType>("B4.enums.AppealOperationType"));

            container.Add("libs/B4/enums/ResolutionPaymentStatus.js", new ExtJsEnumResource<ResolutionPaymentStatus>("B4.enums.ResolutionPaymentStatus"));
            container.Add("libs/B4/enums/KindKNDGJI.js", new ExtJsEnumResource<KindKNDGJI>("B4.enums.KindKNDGJI"));
            container.Add("libs/B4/enums/OfficialReportType.js", new ExtJsEnumResource<OfficialReportType>("B4.enums.OfficialReportType"));

            container.Add("libs/B4/enums/QuestionStatus.js", new ExtJsEnumResource<QuestionStatus>("B4.enums.QuestionStatus"));
            container.Add("libs/B4/enums/SSTUExportState.js", new ExtJsEnumResource<SSTUExportState>("B4.enums.SSTUExportState"));
            container.Add("libs/B4/enums/SSTUSource.js", new ExtJsEnumResource<SSTUSource>("B4.enums.SSTUSource"));
            container.Add("libs/B4/enums/RISExportState.js", new ExtJsEnumResource<RISExportState>("B4.enums.RISExportState"));
            container.Add("libs/B4/enums/TypePrescriptionAnnex.js", new ExtJsEnumResource<TypePrescriptionAnnex>("B4.enums.TypePrescriptionAnnex"));

            var prescrDoc = new ExtJsModelResource<PrescriptionCloseDoc>("B4.model.PrescriptionCloseDoc");
            prescrDoc.GetModelMeta().Controller("PrescriptionCloseDoc");
            container.Add("libs/B4/model/PrescriptionCloseDoc.js", prescrDoc);

			container.RegisterExtJsModel<ActivityDirection>().Controller("ActivityDirection");
			container.RegisterExtJsModel<KindBaseDocument>().Controller("KindBaseDocument");
			container.RegisterExtJsModel<SurveySubject>().Controller("SurveySubject");
			container.RegisterExtJsModel<SurveySubjectRequirement>().Controller("SurveySubjectRequirement");
			container.RegisterExtJsModel<ResolveViolationClaim>().Controller("ResolveViolationClaim");
			container.RegisterExtJsModel<NotificationCause>("dict.NotificationCause").Controller("NotificationCause");
			container.RegisterExtJsModel<MkdManagementMethod>("dict.MkdManagementMethod").Controller("MkdManagementMethod");

            container.RegisterExtJsModel<FuelAmountInfo>("fuelinfo.FuelAmountInfo").Controller("FuelAmountInfo");
            container.RegisterExtJsModel<FuelExtractionDistanceInfo>("fuelinfo.FuelExtractionDistanceInfo").Controller("FuelExtractionDistanceInfo");
            container.RegisterExtJsModel<FuelContractObligationInfo>("fuelinfo.FuelContractObligationInfo").Controller("FuelContractObligationInfo");
            container.RegisterExtJsModel<FuelEnergyDebtInfo>("fuelinfo.FuelEnergyDebtInfo").Controller("FuelEnergyDebtInfo");

            container.RegisterExtJsModel<Citizenship>("dict.Citizenship").Controller("Citizenship");
            container.RegisterExtJsModel<ConcederationResult>("dict.ConcederationResult").Controller("ConcederationResult");
            container.RegisterExtJsModel<FactCheckingType>("dict.FactCheckingType").Controller("FactCheckingType");
            container.RegisterExtJsModel<ApplicantCategory>("dict.ApplicantCategory").Controller("ApplicantCategory");
            container.RegisterExtJsModel<AppealCitsCategory>("appealcits.AppealCitsCategory").Controller("AppealCitsCategory");
            container.RegisterExtJsModel<AppealCitsAttachment>("appealcits.AppealCitsAttachment").Controller("AppealCitsAttachment");
            container.RegisterExtJsModel<AppealCitsAnswerAttachment>("appealcits.AppealCitsAnswerAttachment").Controller("AppealCitsAnswerAttachment");
            container.RegisterExtJsModel<AppealCitsHeadInspector>("appealcits.AppealCitsHeadInspector").Controller("AppealCitsHeadInspector");
            container.RegisterExtJsModel<AppealCitsAnswerStatSubject>("appealcits.AppealCitsAnswerStatSubject")
                .Controller("AppealCitsAnswerStatSubject")
                .AddProperty<string>("Subject")
                .AddProperty<string>("Subsubject")
                .AddProperty<string>("Feature");

            container.RegisterExtJsModel<QuestionKind>("dict.QuestionKind").Controller("QuestionKind");
            container.RegisterExtJsModel<AppealCitsQuestion>("appealcits.AppealCitsQuestion").Controller("AppealCitsQuestion").AddProperty<string>("QuestionType");
            container.RegisterExtJsEnum<AmmountMeasurement>();
            container.RegisterExtJsEnum<YearEnums>();
            container.RegisterExtJsEnum<TypeEntityLogging>();
            container.RegisterExtJsEnum<TypeAppelantPresence>();
            container.RegisterExtJsEnum<TypeRepresentativePresence>();
            container.RegisterExtJsEnum<TypeDecisionAnswer>();
            container.RegisterExtJsEnum<MonthEnums>();//
            container.RegisterExtJsEnum<EmailDenailReason>();
            container.RegisterExtJsEnum<EmailGjiSource>();
            container.RegisterExtJsEnum<SurveySubjectRelevance>();
			container.RegisterExtJsEnum<AcquaintState>();
            container.RegisterExtJsEnum<ControlType>();
            container.Add("libs/B4/enums/RiskCategory.js", new ExtJsEnumResource<Enums.RiskCategory>("B4.enums.RiskCategory"));
            container.RegisterExtJsEnum<AppealStatus>();
            container.RegisterExtJsEnum<ProfVisitResult>();
            container.RegisterExtJsEnum<TypeAppealAnswer>();
            container.RegisterExtJsEnum<QuestionType>();
            container.RegisterExtJsEnum<TypePreventiveAct>();
            container.RegisterGkhEnum(TypeFactInspection.Cancelled, TypeFactInspection.Changed);
         //   container.RegisterGkhEnum(TypeBase.PlanOMSU);
            container.RegisterExtJsEnum<QuestionType>();
            container.RegisterExtJsEnum<TypeBase>();
            container.RegisterExtJsEnum<TypeAppealFinalAnswer>();
            container.RegisterExtJsEnum<ERKNMDocumentType>();
            container.RegisterExtJsEnum<TypeAddress>();
            container.RegisterExtJsEnum<MessageCheck>();
            container.RegisterExtJsEnum<PlaceOffense>();
            container.RegisterExtJsEnum<InspectionKind>();
            container.RegisterGkhEnum(TypeFactInspection.Cancelled, TypeFactInspection.Changed);
            container.RegisterGkhEnum(TypeBase.PlanOMSU);
            container.RegisterExtJsEnum<AuditType>();
            container.RegisterVirtualExtJsEnum("TypeBaseFormatDataExport",
                new[]
                {
                    TypeBase.PlanJuridicalPerson,
                    TypeBase.ProsecutorsClaim,
                    TypeBase.DisposalHead,
                    TypeBase.CitizenStatement
                }.Select(x =>
                    new EnumMemberView
                    {
                        Name = x.ToString(),
                        Display = x.GetDisplayName(),
                        Description = x.GetDescriptionName(),
                        Value = (int)x
                    }));
            container.RegisterExtJsEnum<QuestionType>();
            container.RegisterExtJsEnum<BaseStatementRequestType>();
            container.RegisterExtJsEnum<InspectionCreationBasis>();
            container.RegisterExtJsEnum<CitizenshipType>();
            container.RegisterExtJsEnum<AdmissionType>();
            container.RegisterExtJsEnum<ExpertType>();

            container.RegisterGkhEnum("FormCheck", TypeFormInspection.InspectionVisit);
            container.RegisterGkhEnum("TypeFormInspection", TypeFormInspection.Visual, TypeFormInspection.InspectionVisit);
            container.RegisterGkhEnum("TypeAgreementProsecutor", TypeAgreementProsecutor.ImmediateInspection);
        }
    }
}