using Bars.B4.Utils;

namespace Bars.Gkh.RegOperator.LogMap
{
    using Bars.Gkh.RegOperator.Entities;
    using Bars.B4.Modules.NHibernateChangeLog;

    public class BasePersonalAccountLogMap : AuditLogMap<BasePersonalAccount>
    {
        public BasePersonalAccountLogMap()
        {
            Name("Лицевой счет");

            Description(x => x.PersonalAccountNum);

            //MapProperty(x => x.State, "State", "Статус", x => x.ReturnSafe(y => y.Name));
        }
    }
}
