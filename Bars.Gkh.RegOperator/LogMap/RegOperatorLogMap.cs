using Bars.B4.Modules.NHibernateChangeLog;

namespace Bars.Gkh.RegOperator.LogMap
{
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    public class RegOperatorLogMap : AuditLogMap<RegOperator>
    {
        public RegOperatorLogMap()
        {
            Name("Региональные операторы");

            Description(x => x.ReturnSafe(y => y.Contragent.Name));

            MapProperty(x => x.Contragent, "Contragent", "Контрагент", x => x.Return(y => y.Name));
            MapProperty(x => x.Contragent, "Inn", "ИНН", x => x.Return(y => y.Inn));
        }
    }
}
