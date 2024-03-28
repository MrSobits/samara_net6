namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class ActRemovalPeriodLogMap : AuditLogMap<ActRemovalPeriod>
    {
        public ActRemovalPeriodLogMap()
        {
            this.Name("Акт проверки предписания - Дата и время проведения проверки");
            this.Description(x => x.ActRemoval.DocumentNumber ?? "");

            this.MapProperty(x => x.DateCheck, "DateCheck", "Дата");
            this.MapProperty(x => x.TimeStart, "TimeStart", "Время начала");
            this.MapProperty(x => x.TimeEnd, "TimeEnd", "Время окончания");
        }
    }
}