using Bars.B4.Modules.NHibernateChangeLog;
using Bars.Gkh.Entities;

namespace Bars.Gkh.LogMap
{
    using Bars.B4.Utils;

    public class ManagingOrganizationLogMap : AuditLogMap<ManagingOrganization>
    {
        public ManagingOrganizationLogMap()
        {
            Name("Управляющие организации");

            Description(x => x.Contragent.Return(y => y.Name ?? string.Empty));

            MapProperty(x => x.Contragent, "Name", "Контрагент", x => x.Return(y => y.Name));
            MapProperty(x => x.Contragent, "Inn", "ИНН", x => x.Return(y => y.Inn));
        }
    }
}
