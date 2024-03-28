namespace Bars.GkhGji.LogMap.ActCheck
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class ActCheckProvidedDocLogMap : AuditLogMap<ActCheckProvidedDoc>
    {
        public ActCheckProvidedDocLogMap()
        {
            this.Name("Акт проверки - Предоставленные документы");
            this.Description(x => x.ActCheck.DocumentNumber ?? "");

            this.MapProperty(x => x.DateProvided, "DateProvided", "Дата предосталвения");
            this.MapProperty(x => x.ProvidedDoc.Name, "Name", "Предоставляемый документ");

        }
    }
}