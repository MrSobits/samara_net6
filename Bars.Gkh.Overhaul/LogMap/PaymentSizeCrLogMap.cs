using Bars.B4.Modules.NHibernateChangeLog;
using Bars.B4.Utils;
using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.LogMap
{
    public class PaymentSizeCrLogMap : AuditLogMap<PaymentSizeCr>
    {
        public PaymentSizeCrLogMap()
        {
            Name("Размеры взноса на КР");

            Description(x => x.TypeIndicator.GetEnumMeta().Display);

            MapProperty(x => x.PaymentSize, "PaymentSize", "Размер взноса");
            MapProperty(x => x.DateEndPeriod, "DateEndPeriod", "Период действия с");
            MapProperty(x => x.DateStartPeriod, "DateStartPeriod", "Период действия по");
        }
    }
}
