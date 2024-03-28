using Bars.B4.Modules.NHibernateChangeLog;
using Bars.GkhCr.Entities;

namespace Bars.GkhCr.LogMap
{
    using Bars.B4.Utils;

    public class PerformedWorkActPaymentLogMap : AuditLogMap<PerformedWorkActPayment>
    {
        public PerformedWorkActPaymentLogMap()
        {
            Name("Оплата актов выполненных работ");

            Description(x => x.Return(z => z.PerformedWorkAct).Return(y => y.DocumentNum));

            MapProperty(x => x.DatePayment, "DatePayment", "Дата оплаты");
            MapProperty(x => x.Sum, "Sum", "Сумма");
            MapProperty(x => x.Paid, "Paid", "Оплачено");
            MapProperty(x => x.Percent, "Percent", "Процент оплаты");
        }
    }
}
