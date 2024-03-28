namespace Bars.GkhGji.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class AppealCitsLogMap : AuditLogMap<AppealCits>
    {
        public AppealCitsLogMap()
        {
            this.Name("Реестр обращений");
            this.Description(x => x.DocumentNumber ?? string.Empty);

            this.MapProperty(x => x.Number, "Number", "Номер");
            this.MapProperty(x => x.DateFrom, "DateFrom", "Дата обращения от");
        }
    }
}
