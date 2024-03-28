/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.Gkh.Map;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Приложения протокола МЖК"
///     /// </summary>
///     public class ProtocolMhcAnnexMap : BaseGkhEntityMap<ProtocolMhcAnnex>
///     {
///         public ProtocolMhcAnnexMap()
///             : base("GJI_PROTOCOLMHC_ANNEX")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             References(x => x.ProtocolMhc, "PROTOCOLMHC_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Приложения в протоколе МЖК"</summary>
    public class ProtocolMhcAnnexMap : BaseEntityMap<ProtocolMhcAnnex>
    {
        
        public ProtocolMhcAnnexMap() : 
                base("Приложения в протоколе МЖК", "GJI_PROTOCOLMHC_ANNEX")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Reference(x => x.ProtocolMhc, "Протокол МЖК").Column("PROTOCOLMHC_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Property(x => x.MessageCheck, "Статус файла").Column("MESSAGE_CHECK").NotNull();
        }
    }
}
