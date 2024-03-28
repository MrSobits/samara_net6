namespace Bars.GkhGji.LogMap.Disposal
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class DisposalSurveyObjectiveLogMap : AuditLogMap<DisposalSurveyObjective>
    {
        public DisposalSurveyObjectiveLogMap()
        {
            this.Name("Распоряжение - Задачи проверки");
            this.Description(x => x.Disposal.DocumentNumber ?? "");

            this.MapProperty(x => x.SurveyObjective.Code, "Code", "Код");
            this.MapProperty(x => x.SurveyObjective.Name, "Name", "Наименование");
        }
    }
}