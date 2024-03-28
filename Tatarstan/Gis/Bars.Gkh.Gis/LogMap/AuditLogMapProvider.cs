namespace Bars.Gkh.Gis.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;

    public class AuditLogMapProvider : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            container.Add<GisTariffDictLogMap>();
        }
    }
}