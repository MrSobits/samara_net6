using Bars.B4.Modules.NHibernateChangeLog;

namespace Bars.Gkh.Overhaul.LogMap
{
    using Bars.Gkh.Overhaul.Entities;

    public class CreditOrgLogMap : AuditLogMap<CreditOrg>
    {
        public CreditOrgLogMap()
        {
            Name("Кредитные организации");

            Description(x => x.Name);

            MapProperty(x => x.Name, "Name", "Наименование");
            MapProperty(x => x.Inn, "Inn", "ИНН");
        }
    }
}
