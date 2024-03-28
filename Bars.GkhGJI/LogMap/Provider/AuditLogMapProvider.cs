namespace Bars.GkhGji.LogMap.Provider
{
    using B4.Modules.NHibernateChangeLog;
    using ActCheck;
    using Disposal;
    using Prescription;
    using Protocol;
    using Resolution;

    public class AuditLogMapProvider : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            container.Add<ActivityTsjLogMap>();
            container.Add<AppealCitsLogMap>();
            container.Add<BusinessActivityLogMap>();
            container.Add<DocumentGjiLogMap>();
            container.Add<HeatSeasonLogMap>();

            container.Add<ActCheckAnnexLogMap>();
            container.Add<ActCheckDefinitionLogMap>();
            container.Add<ActCheckInspectedPartLogMap>();
            container.Add<ActCheckPeriodLogMap>();
            container.Add<ActCheckProvidedDocLogMap>();
            container.Add<ActCheckViolationLogMap>();
            container.Add<ActCheckWitnessLogMap>();

            container.Add<DisposalAdminRegulationLogMap>();
            container.Add<DisposalAnnexLogMap>();
            container.Add<DisposalExpertLogMap>();
            container.Add<DisposalInspFoundCheckNormDocItemLogMap>();
            container.Add<DisposalProvidedDocLogMap>();
            container.Add<DisposalSurveyObjectiveLogMap>();
            container.Add<DisposalSurveyPurposeLogMap>();
            container.Add<DisposalTypeSurveyLogMap>();
            container.Add<DisposalVerificationSubjectLogMap>();

            container.Add<PrescriptionAnnexLogMap>();
            container.Add<PrescriptionArticleLawLogMap>();
            container.Add<PrescriptionCancelLogMap>();
            container.Add<InspectionGjiViolStageLogMap>();

            container.Add<ProtocolAnnexLogMap>();
            container.Add<ProtocolArticleLawLogMap>();
            container.Add<ProtocolDefinitionLogMap>();
            container.Add<ProtocolViolationLogMap>();

            container.Add<ActRemovalViolationLogMap>();
            container.Add<ResolutionAnnexLogMap>();
            container.Add<ResolutionDefinitionLogMap>();
            container.Add<ResolutionDisputeLogMap>();
            container.Add<ResolutionPayFineLogMap>();

            container.Add<ActCheckLogMap>();
            container.Add<PrescriptionLogMap>();
            container.Add<ProtocolLogMap>();
            container.Add<ResolutionLogMap>();
            container.Add<ActRemovalLogMap>();
            container.Add<DocumentGjiInspectorLogMap>();

            container.Add<DisposalLogMap>();
            container.Add<ActCheckRealityObjectLogMap>();
        }
    }
}