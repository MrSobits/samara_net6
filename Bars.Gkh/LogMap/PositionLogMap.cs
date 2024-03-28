using Bars.B4.Modules.NHibernateChangeLog;
using Bars.Gkh.Entities;

namespace Bars.Gkh.LogMap
{
    public class PositionLogMap : AuditLogMap<Position>
    {
        public PositionLogMap()
        {
            Name("Справочник должностей");

            Description(x => x.Name);

            MapProperty(x => x.Name, "Name", "Наименование");
        }
    }
}
