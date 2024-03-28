namespace Bars.GkhGji.LogMap.Resolution
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class ResolutionAnnexLogMap : AuditLogMap<ResolutionAnnex>
    {
        public ResolutionAnnexLogMap()
        {
            this.Name("Постановление - Приложения");
            this.Description(x => x.Resolution.DocumentNumber ?? "");

            this.MapProperty(x => x.Name, "Name", "Наименование");
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Description, "Description", "Описание");
            this.MapProperty(x => x.File.FullName, "File", "Файл");
        }
    }
}