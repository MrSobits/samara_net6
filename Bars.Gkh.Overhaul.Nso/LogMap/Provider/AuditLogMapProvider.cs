using Bars.B4.Modules.NHibernateChangeLog;

namespace Bars.Gkh.Overhaul.Nso.LogMap.Provider
{
    public class AuditLogMapProvider : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            container.Add<ProgramVersionLogMap>();
        }
    }
}