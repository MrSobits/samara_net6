using Bars.B4.Modules.NHibernateChangeLog;
using Bars.GkhRf.Entities;

namespace Bars.GkhRF.LogMap
{
    using Bars.B4.Utils;

    public class RequestTransferRfLogMap : AuditLogMap<RequestTransferRf>
    {
        public RequestTransferRfLogMap()
        {
            Name("Заявки на перечисление денежных средств");

            Description(x => "Заявки на перечисление денежных средств");

            MapProperty(x => x.DocumentNum, "Номер завки", "Номер завки");
            MapProperty(x => x.DateFrom, "Дата завки", "Дата завки");
            MapProperty(x => x.ManagingOrganization.Contragent, "Управляющая организация", "Управляющая организация", x => x.Return(y => y.Name));
        }
    }
}
