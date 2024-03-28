namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities.ActRemoval;

    public class ActRemovalProvidedDocLogMap : AuditLogMap<ActRemovalProvidedDoc>
    {
        public ActRemovalProvidedDocLogMap()
        {
            this.Name("Акт проверки предписания - Предоставленные документы");
            this.Description(x => x.ActRemoval.DocumentNumber ?? "");

            this.MapProperty(x => x.ActRemoval.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.ActRemoval.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.ProvidedDoc.Name, "Name", "Наименование");
            this.MapProperty(x => x.ProvidedDoc.Code, "Code", "Код");
            this.MapProperty(x => x.DateProvided, "TimeStart", "Время начала");
        }
    }
}