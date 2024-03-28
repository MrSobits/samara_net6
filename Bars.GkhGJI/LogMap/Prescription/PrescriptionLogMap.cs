namespace Bars.GkhGji.LogMap.Prescription
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class PrescriptionLogMap : AuditLogMap<Prescription>
    {
        public PrescriptionLogMap()
        {
            this.Name("Документы ГЖИ");
            this.Description(x => x.DocumentNumber ?? "");
            
            this.MapProperty(x => x.TypeDocumentGji, "TypeDocumentGji", "Тип документа");
            this.MapProperty(x => x.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Description, "DocumentPlace", "Примечание");
            this.MapProperty(x => x.PhysicalPerson, "PhysicalPerson", "Физическое лицо");
            this.MapProperty(x => x.Contragent.Name, "Name", "Контрагент");
            this.MapProperty(x => x.PhysicalPersonInfo, "PhysicalPersonInfo", "Реквизиты физ. лица");
        }
    }
}
