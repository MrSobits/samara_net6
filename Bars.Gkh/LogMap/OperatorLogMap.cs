using Bars.B4.Modules.NHibernateChangeLog;
using Bars.Gkh.Entities;

namespace Bars.Gkh.LogMap
{
    using Bars.B4.Utils;

    public class OperatorLogMap : AuditLogMap<Operator>
    {
        public OperatorLogMap()
        {
            Name("Операторы");

            Description(x => x.User.Return(y => y.Login));

            MapProperty(x => x.User, "Login", "Логин", x => x.Return(y => y.Login));
        }
    }
}
