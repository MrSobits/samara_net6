using Bars.B4.Modules.NHibernateChangeLog;
using Bars.Gkh.Entities;

namespace Bars.Gkh.LogMap
{
    public class MunicipalityLogMap : AuditLogMap<Municipality>
    {
        public MunicipalityLogMap()
        {
            Name("Муниципальные образования");

            Description(x => x.Name);

            MapProperty(x => x.Name, "Name", "Наименование");
        }
    }
}
