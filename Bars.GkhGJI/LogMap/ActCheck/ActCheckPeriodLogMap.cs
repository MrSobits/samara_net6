namespace Bars.GkhGji.LogMap.ActCheck
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActCheckPeriodLogMap : AuditLogMap<ActCheckPeriod>
    {
        public ActCheckPeriodLogMap()
        {
            this.Name("Дата и время проведения проверки");
            this.Description(x => x.ActCheck.DocumentNumber ?? "");

            this.MapProperty(x => x.DateCheck, "DateCheck", "Дата");
            this.MapProperty(x => x.TimeStart, "DateStart", "Время начала");
            this.MapProperty(x => x.TimeEnd, "DateEnd", "Время окончания");
        }
    }
}