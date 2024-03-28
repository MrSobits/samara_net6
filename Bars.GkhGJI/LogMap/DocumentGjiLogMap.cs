namespace Bars.GkhGji.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Entities;
    using Bars.B4.Utils;

    public class DocumentGjiLogMap : AuditLogMap<DocumentGji>
    {
        public DocumentGjiLogMap()
        {
            this.Name("Документы ГЖИ");
            this.Description(x => x.DocumentNumber ?? "");

            this.MapProperty(x => x.TypeDocumentGji, "TypeDocumentGji", "Тип документа");
            this.MapProperty(x => x.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.Inspection.InspectionBaseType.Name, "InspectionBaseType", "Основание проверки");
            this.MapProperty(x => x.Inspection.CheckDate, "CheckDate", "Дата проверки");
            this.MapProperty(x => x.Inspection.State.Name, "State", "Статус");
            this.MapProperty(x => x.TypeDocumentGji, "TypeDocumentGji", "Тип документа ГЖИ", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.Inspection.InspectionBaseType.Name, "Name", "Основание проверки");
        }
    }
}
