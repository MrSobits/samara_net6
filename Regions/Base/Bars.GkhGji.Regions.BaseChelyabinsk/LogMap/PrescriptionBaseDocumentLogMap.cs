namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities.Prescription;

    public class PrescriptionBaseDocumentLogMap : AuditLogMap<PrescriptionBaseDocument>
    {
        public PrescriptionBaseDocumentLogMap()
        {
            this.Name("Предписание - Деятельность");
            this.Description(x => x.Prescription.DocumentNumber ?? "");

            this.MapProperty(x => x.Prescription.DocumentDate, "DocumentDate", "Дата документа", x => x.Return(y => y).ToString());
            this.MapProperty(x => x.Prescription.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.Prescription.Executant.Name, "Executant", "Тип исполнителя документа");
            this.MapProperty(x => x.Prescription.TypeDocumentGji, "Description", "Тип документа ГЖИ", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.Prescription.DocumentDate, "PrescriptionDocumentDate", "Дата документа");
            this.MapProperty(x => x.Prescription.DocumentNumber, "PrescriptionDocumentNumber", "Номер документа");
        }
    }
}
