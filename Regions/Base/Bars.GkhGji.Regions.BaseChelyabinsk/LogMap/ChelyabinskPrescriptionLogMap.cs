namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities.Prescription;

    public class ChelyabinskPrescriptionLogMap : AuditLogMap<ChelyabinskPrescription>
    {
        public ChelyabinskPrescriptionLogMap()
        {
            this.Name("Предписание ГЖИ");
            this.Description(x => x.DocumentNumber ?? "");

            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Description, "DocumentPlace", "Примечание");
            this.MapProperty(x => x.PhysicalPerson, "PhysicalPerson", "Физическое лицо");
            this.MapProperty(x => x.Contragent.Name, "Name", "Контрагент");
            this.MapProperty(x => x.PhysicalPersonInfo, "PhysicalPersonInfo", "Реквизиты физ. лица");
        }
    }
}
