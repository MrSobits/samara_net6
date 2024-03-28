using Bars.B4.Modules.NHibernateChangeLog;
using Bars.Gkh.Entities;

namespace Bars.Gkh.LogMap
{
    public class InspectorLogMap : AuditLogMap<Inspector>
    {
        public InspectorLogMap()
        {
            Name("Инспекторы");

            Description(x => x.Fio);

            MapProperty(x => x.Fio, "Fio", "ФИО");
        }
    }
}
