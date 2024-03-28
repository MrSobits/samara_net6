using Bars.B4.Modules.NHibernateChangeLog;
using Bars.GkhRf.Entities;

namespace Bars.GkhRF.LogMap
{
    using Bars.B4.Utils;

    public class PaymentLogMap : AuditLogMap<Payment>
    {
        public PaymentLogMap()
        {
            Name("Реестр оплат капитального ремонта");

            Description(x => "Реестр оплат капитального ремонта");

            MapProperty(x => x.RealityObject.Municipality, "Муниципальное образование", "Муниципальное образование", x => x.Return(y => y.Name));
            MapProperty(x => x.RealityObject, "Адрес", "Адрес", x => x.Return(y => y.Address));
        }
    }
}
