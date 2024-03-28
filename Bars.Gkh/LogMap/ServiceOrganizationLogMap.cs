using Bars.B4.Modules.NHibernateChangeLog;
using Bars.Gkh.Entities;

namespace Bars.Gkh.LogMap
{
    using Bars.B4.Utils;

    public class ServiceOrganizationLogMap : AuditLogMap<ServiceOrganization>
    {
        public ServiceOrganizationLogMap()
        {
            Name("Поставщики жилищных услуг");

            Description(x => x.Contragent.Return(y => y.Name ?? string.Empty));

            MapProperty(x => x.Contragent, "Contragent", "Контрагент", x => x.Return(y => y.Name));
            MapProperty(x => x.Contragent, "Inn", "ИНН", x => x.Return(y => y.Inn));
        }
    }
}
