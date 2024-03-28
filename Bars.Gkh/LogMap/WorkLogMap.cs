using Bars.B4.Modules.NHibernateChangeLog;
using Bars.Gkh.Entities.Dicts;

namespace Bars.Gkh.LogMap
{
    public class WorkLogMap : AuditLogMap<Work>
    {
        public WorkLogMap()
        {
            Name("Виды работ");

            Description(x => x.Name);

            MapProperty(x => x.Name, "Name", "Наименование");
        }
    }
}
