using Bars.B4.Modules.NHibernateChangeLog;
using Bars.GkhDi.Entities;

namespace Bars.GkhDI.LogMap
{
    using Bars.B4.Utils;

    public class DisclosureInfoLogMap : AuditLogMap<DisclosureInfo>
    {
        public DisclosureInfoLogMap()
        {
            Name("Раскрытие информации по 731 (988) ПП РФ");

            Description(x => x.ReturnSafe(y => string.Format("{0} - {1}", y.ManagingOrganization.Contragent.Name, y.PeriodDi.Name)));

            MapProperty(x => x.ManagingOrganization.Contragent.Name, "ManagingOrganization", "Управляющая организация");
            MapProperty(x => x.PeriodDi.Name, "PeriodDi", "Период");
        }
    }
}
