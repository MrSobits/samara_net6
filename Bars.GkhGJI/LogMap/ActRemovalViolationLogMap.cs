namespace Bars.GkhGji.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class ActRemovalViolationLogMap : AuditLogMap<ActRemovalViolation>
    {
        public ActRemovalViolationLogMap()
        {
            this.Name("Акт проверки предписания - Нарушения");
            this.Description(x => x.Document.DocumentNumber ?? "");

            this.MapProperty(x => x.DatePlanRemoval, "DatePlanRemoval", "Плановая дата устранения");
            this.MapProperty(x => x.Document.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.CircumstancesDescription, "CircumstancesDescription", "Описание обстоятельств");
            this.MapProperty(x => x.DateFactRemoval, "DateFactRemoval", "Фактическая дата устранения");
        }
    }
}