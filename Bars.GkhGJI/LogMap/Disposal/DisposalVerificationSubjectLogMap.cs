namespace Bars.GkhGji.LogMap.Disposal
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class DisposalVerificationSubjectLogMap : AuditLogMap<DisposalVerificationSubject>
    {
        public DisposalVerificationSubjectLogMap()
        {
            this.Name("Распоряжение - Предметы проверки");
            this.Description(x => x.Disposal.DocumentNumber ?? string.Empty);

            this.MapProperty(x => x.SurveySubject.Code, "Code", "Код");
            this.MapProperty(x => x.SurveySubject.Name, "Name", "Наименование");
        }
    }
}
