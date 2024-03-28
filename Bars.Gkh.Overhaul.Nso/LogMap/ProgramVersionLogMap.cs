using Bars.B4.Modules.NHibernateChangeLog;
using Bars.Gkh.Overhaul.Nso.Entities;

namespace Bars.Gkh.Overhaul.Nso.LogMap
{
    public class ProgramVersionLogMap : AuditLogMap<ProgramVersion>
    {
        public ProgramVersionLogMap()
        {
            Name("Версии программы");

            Description(x => x.Name);

            MapProperty(x => x.Name, "Name", "Наименование");
            MapProperty(x => x.VersionDate, "VersionDate", "Дата");
        }
    }
}
