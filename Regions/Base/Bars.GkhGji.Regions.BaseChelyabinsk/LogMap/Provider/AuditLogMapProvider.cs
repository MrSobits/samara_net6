namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap.Provider
{
    using B4.Modules.NHibernateChangeLog;

    public class AuditLogMapProvider : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            container.Add<ActCheckRoLongDescriptionLogMap>();
            container.Add<ActRemovalAnnexLogMap>();
            container.Add<ActRemovalDefinitionLogMap>();
            container.Add<ActRemovalInspectedPartLogMap>();
            container.Add<ActRemovalPeriodLogMap>();
            container.Add<ActRemovalProvidedDocLogMap>();
            container.Add<ActRemovalWitnessLogMap>();

            container.Add<ChelyabinskDisposalLogMap>();

            container.Add<DisposalControlMeasuresLogMap>();
            container.Add<DisposalDocConfirmLogMap>();
            container.Add<PrescriptionBaseDocumentLogMap>();
            container.Add<ProtocolBaseDocumentLogMap>();
            container.Add<ChelyabinskActRemovalLogMap>();
            container.Add<ChelyabinskPrescriptionLogMap>();
            container.Add<ChelyabinskProtocolLogMap>();
            container.Add<ChelyabinskActCheckLogMap>();

            container.Add<ActCheckRealityObjectLogMap>();
            container.Add<ActCheckProvidedDocLogMap>();
            container.Add<ActCheckViolationLogMap>();
        }
    }
}
