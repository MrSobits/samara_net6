namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities.Protocol;

    public class ProtocolBaseDocumentLogMap : AuditLogMap<ProtocolBaseDocument>
    {
        public ProtocolBaseDocumentLogMap()
        {
            this.Name("Протокол - Деятельность");
            this.Description(x => x.Protocol.DocumentNumber ?? "");

            this.MapProperty(x => x.Protocol.FormatPlace, "FormatPlace", "Тип исполнителя документа");
            this.MapProperty(x => x.Protocol.TypeDocumentGji, "TypeDocumentGji", "Тип документа ГЖИ", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.Protocol.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Protocol.DocumentNumber, "DocumentNumber", "Номер документа");
        }
    }
}