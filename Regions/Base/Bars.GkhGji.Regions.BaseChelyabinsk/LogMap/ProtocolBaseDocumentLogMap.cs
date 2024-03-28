namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
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

            this.MapProperty(x => x.Protocol.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Protocol.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.Protocol.FormatPlace, "Executant", "Тип исполнителя документа");
            this.MapProperty(x => x.Protocol.TypeDocumentGji, "Description", "Тип документа ГЖИ", x => x.Return(y => y.GetDescriptionName()));
            this.MapProperty(x => x.Protocol.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Protocol.DocumentNumber, "DocumentNumber", "Номер документа");
        }
    }
}
