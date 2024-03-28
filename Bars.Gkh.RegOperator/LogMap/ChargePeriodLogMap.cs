namespace Bars.Gkh.RegOperator.LogMap
{
    using B4.Modules.NHibernateChangeLog;

    using Bars.Gkh.Entities;

    using Entities;

    public class ChargePeriodLogMap : AuditLogMap<ChargePeriod>
    {
        public ChargePeriodLogMap()
        {
            Name("Периоды начислений");
            Description(x => x.Name);
            MapProperty(x => x.IsClosed, "IsClosed", "Закрыт", x => x ? "Да" : "Нет");
        }
    }
}
