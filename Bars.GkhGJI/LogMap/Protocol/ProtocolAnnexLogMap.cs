namespace Bars.GkhGji.LogMap.Protocol
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class ProtocolAnnexLogMap : AuditLogMap<ProtocolAnnex>
    {
        public ProtocolAnnexLogMap()
        {
            this.Name("Решение об отмене в предписании ГЖИ");
            this.Description(x => x.Protocol.DocumentNumber ?? "");

            this.MapProperty(x => x.Name, "Name", "Наименование");
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Description, "Description", "Описание");
            this.MapProperty(x => x.File.FullName, "File", "Файл");
        }
    }
}