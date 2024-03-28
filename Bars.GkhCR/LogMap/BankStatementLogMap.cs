using Bars.B4.Modules.NHibernateChangeLog;
using Bars.GkhCr.Entities;

namespace Bars.GkhCr.LogMap
{
    using Bars.B4.Utils;

    public class BankStatementLogMap : AuditLogMap<BankStatement>
    {
        public BankStatementLogMap()
        {
            Name("Банковские выписки");

            Description(x => x.DocumentNum);

            MapProperty(x => x.DocumentNum, "DocumentNum", "Номер");
            MapProperty(x => x.DocumentDate, "DocumentDate", "Дата");
            MapProperty(x => x.ObjectCr.RealityObject, "RealityObject", "Адрес", x => x.Return(y => y.Address));
        }
    }
}
