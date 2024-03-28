using Bars.B4.Modules.NHibernateChangeLog;

namespace Bars.Gkh1468.LogMap.Provider
{
    public class AuditLogMapProvider : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            container.Add<PublicServiceOrgLogMap>();
        }
    }
}