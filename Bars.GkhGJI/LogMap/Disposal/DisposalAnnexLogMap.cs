namespace Bars.GkhGji.LogMap.Disposal
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class DisposalAnnexLogMap : AuditLogMap<DisposalAnnex>
    {
        public DisposalAnnexLogMap()
        {
            this.Name("Распоряжение – Приложения");
            this.Description(x => x.Disposal.DocumentNumber ?? "");

            this.MapProperty(x => x.Name, "Name", "Наименование");
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Description, "Description", "Описание");
            this.MapProperty(x => x.File.FullName, "File", "Файл");
        }
    }
}