namespace Bars.GkhGji.LogMap.Disposal
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class DisposalTypeSurveyLogMap : AuditLogMap<DisposalTypeSurvey>
    {
        public DisposalTypeSurveyLogMap()
        {
            this.Name("Распоряжение - Тип обследования");
            this.Description(x => x.Disposal.DocumentNumber ?? "");

            this.MapProperty(x => x.TypeSurvey.Name, "TypeSurvey", "Тип обследования");
        }
    }
}