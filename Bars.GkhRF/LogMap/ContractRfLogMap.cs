using Bars.B4.Modules.NHibernateChangeLog;
using Bars.GkhRf.Entities;

namespace Bars.Gkh.UserActionRetention.LogMap
{
    using Bars.B4.Utils;

    public class ContractRfLogMap : AuditLogMap<ContractRf>
    {
        public ContractRfLogMap()
        {
            Name("Реестр договоров с управляющими компаниями");

            Description(x => "Реестр договоров с управляющими компаниями");

            MapProperty(x => x.DocumentNum, "Номер", "Номер");
            MapProperty(x => x.ManagingOrganization.Contragent, "Управляющая организация", "Управляющая организация", x => x.Return(y => y.Name));
        }
    }
}
