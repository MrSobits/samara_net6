namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class NsoPrescriptionLogMap : AuditLogMap<NsoPrescription>
    {
        public NsoPrescriptionLogMap()
        {
            this.Name("Предписание ГЖИ");
            this.Description(x => x.DocumentNumber ?? "");

            this.MapProperty(x => x.DocumentDate, "DateNumber", "Дата документа");
            this.MapProperty(x => x.DocumentPlace, "DocumentPlace", "Место составления");
            this.MapProperty(x => x.DocumentTime, "DocumentTime", "Время составления");
            this.MapProperty(x => x.PhysicalPerson, "PhysicalPerson", "Физическое лицо");
            this.MapProperty(x => x.Contragent.Name, "Name", "Контрагент");
            this.MapProperty(x => x.PhysicalPersonInfo, "PhysicalPersonInfo", "Реквизиты физ. лица");
        }
    }
}
