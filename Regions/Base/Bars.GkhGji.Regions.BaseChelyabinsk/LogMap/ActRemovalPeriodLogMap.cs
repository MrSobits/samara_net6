namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities.ActRemoval;

    public class ActRemovalPeriodLogMap : AuditLogMap<ActRemovalPeriod>
    {
        public ActRemovalPeriodLogMap()
        {
            this.Name("Акт проверки предписания - Дата и время проведения проверки");
            this.Description(x => x.ActRemoval.DocumentNumber ?? "");

            this.MapProperty(x => x.ActRemoval.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.ActRemoval.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.DateCheck, "DateCheck", "Дата");
            this.MapProperty(x => x.TimeStart, "TimeStart", "Время начала");
            this.MapProperty(x => x.TimeEnd, "TimeEnd", "Время окончания");
        }
    }
}