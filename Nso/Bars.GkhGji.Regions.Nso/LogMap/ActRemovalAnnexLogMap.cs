namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class ActRemovalAnnexLogMap : AuditLogMap<ActRemovalAnnex>
    {
        public ActRemovalAnnexLogMap()
        {
            this.Name("Акт проверки предписания - Приложения");
            this.Description(x => x.ActRemoval.DocumentNumber ?? "");

            this.MapProperty(x => x.Name, "Name", "Наименование");
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Description, "Description", "Описание");
            this.MapProperty(x => x.File.FullName, "File", "Файл");
        }
    }
}