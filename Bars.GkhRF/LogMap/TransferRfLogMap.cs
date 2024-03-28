using Bars.B4.Modules.NHibernateChangeLog;
using Bars.GkhRf.Entities;

namespace Bars.GkhRF.LogMap
{
    using Bars.B4.Utils;

    public class TransferRfLogMap : AuditLogMap<TransferRf>
    {
        public TransferRfLogMap()
        {
            Name("Информация о перечислениях средств в фонд");

            Description(x => "Информация о перечислениях средств в фонд");

            MapProperty(x => x.ContractRf, "Договор", "Договор", x => x.Return(y => y.DocumentNum));
        }
    }
}
