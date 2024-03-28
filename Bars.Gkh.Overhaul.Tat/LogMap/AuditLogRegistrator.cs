namespace Bars.Gkh.Overhaul.Tat.LogMap
{
    using B4.Modules.NHibernateChangeLog;

    public class AuditLogRegistrator : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            container.Add<PropertyOwnerProtocolsLogMap>();
            container.Add<VersionRecordLogMap>();
            container.Add<DpkrCorrectionLogMap>();
        }
    }
}
