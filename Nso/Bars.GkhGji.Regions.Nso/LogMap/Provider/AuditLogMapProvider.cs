namespace Bars.GkhGji.Regions.Nso.LogMap.Provider
{
    using Bars.B4.Modules.NHibernateChangeLog;

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
            container.Add<DisposalDocConfirmLogMap>();
            container.Add<PrescriptionBaseDocumentLogMap>();
            container.Add<ProtocolBaseDocumentLogMap>();
            container.Add<NsoActCheckLogMap>();
            container.Add<NsoActRemovalLogMap>();
            container.Add<NsoPrescriptionLogMap>();
            container.Add<NsoProtocolLogMap>();
            container.Add<NsoDisposalLogMap>();
            container.Add<ActCheckProvidedDocLogMap>();
            container.Add<ActCheckRealityObjectLogMap>();
            container.Add<ActCheckViolationLogMap>();
        }
    }
}