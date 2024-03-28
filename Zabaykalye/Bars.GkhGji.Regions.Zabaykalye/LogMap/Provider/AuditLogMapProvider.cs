namespace Bars.GkhGji.Regions.Zabaykalye.LogMap.Provider
{
    using Bars.B4.Modules.NHibernateChangeLog;

    public class AuditLogMapProvider : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            container.Add<DisposalControlMeasuresLogMap>();
        }
    }
}