namespace Bars.GkhGji.LogMap.Prescription
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class PrescriptionAnnexLogMap : AuditLogMap<PrescriptionAnnex>
    {
        public PrescriptionAnnexLogMap()
        {
            this.Name("Предписание - Приложения");
            this.Description(x => x.Prescription.DocumentNumber ?? "");

            this.MapProperty(x => x.Name, "Name", "Наименование");
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Description, "Description", "Описание");
            this.MapProperty(x => x.File.FullName, "File", "Файл");
        }
    }
}