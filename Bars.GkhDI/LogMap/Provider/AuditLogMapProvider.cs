using Bars.B4.Modules.NHibernateChangeLog;

namespace Bars.GkhDI.LogMap.Provider
{
    public class AuditLogMapProvider : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            container.Add<DisclosureInfoLogMap>();
            container.Add<DisclosureInfoPercentLogMap>();
        }
    }
}