using Bars.B4.Modules.NHibernateChangeLog;
using Bars.Gkh.Entities;

namespace Bars.Gkh.LogMap
{
    using Bars.B4.Utils;

    public class BuilderLogMap : AuditLogMap<Builder>
    {
        public BuilderLogMap()
        {
            Name("Подрядные организации");

            Description(x => x.Contragent.Return(y => y.Name));

            MapProperty(x => x.Contragent, "Contragent", "Контрагент", x => x.Return(y => y.Name));
            MapProperty(x => x.Contragent, "Inn", "ИНН", x => x.Return(y => y.Inn));
        }
    }
}
