namespace Bars.GkhGji.LogMap.Disposal
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class DisposalSurveyPurposeLogMap : AuditLogMap<DisposalSurveyPurpose>
    {
        public DisposalSurveyPurposeLogMap()
        {
            this.Name("Распоряжение - Цели проверки");
            this.Description(x => x.Disposal.DocumentNumber ?? "");

            this.MapProperty(x => x.SurveyPurpose.Code, "Code", "Код");
            this.MapProperty(x => x.SurveyPurpose.Name, "Name", "Эксперт");
        }
    }
}