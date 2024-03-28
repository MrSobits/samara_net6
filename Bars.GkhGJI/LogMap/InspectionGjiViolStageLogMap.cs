namespace Bars.GkhGji.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class InspectionGjiViolStageLogMap : AuditLogMap<InspectionGjiViolStage>
    {
        public InspectionGjiViolStageLogMap()
        {
            this.Name("Предписание - Нарушения");
            this.Description(x => x.Document.DocumentNumber ?? "");

            this.MapProperty(x => x.InspectionViolation.Action, "Action", "Мероприяти");
            this.MapProperty(x => x.InspectionViolation.DatePlanRemoval, "DatePlanRemoval", "Плановая дата устранения");
            this.MapProperty(x => x.InspectionViolation.DateFactRemoval, "DateFactRemoval", "фактическая дата устранения");
        }
    }
}