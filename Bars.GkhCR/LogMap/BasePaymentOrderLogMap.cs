using Bars.B4.Modules.NHibernateChangeLog;
using Bars.GkhCr.Entities;

namespace Bars.GkhCr.LogMap
{
    public class BasePaymentOrderLogMap : AuditLogMap<BasePaymentOrder>
    {
        public BasePaymentOrderLogMap()
        {
            Name("Платежные поручения");

            Description(x => x.DocId);

            MapProperty(x => x.DocId, "DocId", "ID документа");
        }
    }
}
