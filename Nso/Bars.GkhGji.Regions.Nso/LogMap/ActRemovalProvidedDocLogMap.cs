namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class ActRemovalProvidedDocLogMap : AuditLogMap<ActRemovalProvidedDoc>
    {
        public ActRemovalProvidedDocLogMap()
        {
            this.Name("Акт проверки предписания - Предоставленные документы");

            this.Description(x => x.ActRemoval.DocumentNumber ?? "");

            this.MapProperty(x => x.ProvidedDoc, "ProvidedDoc", "Предоставляемый документа");
            this.MapProperty(x => x.DateProvided, "DateProvided", "Время начала");
        }
    }
}