namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class PrescriptionBaseDocumentLogMap : AuditLogMap<PrescriptionBaseDocument>
    {
        public PrescriptionBaseDocumentLogMap()
        {
            this.Name("Предписание - Деятельность");
            this.Description(x => x.Prescription.DocumentNumber ?? "");

            this.MapProperty(x => x.Prescription.Executant.Name, "Executant", "Тип исполнителя документа");
            this.MapProperty(x => x.Prescription.TypeDocumentGji, "Description", "Тип документа ГЖИ", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.Prescription.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Prescription.DocumentNumber, "DocumentNumber", "Номер документа");
        }
    }
}