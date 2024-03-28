/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.Gkh.Map;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Приложения протокола МВД"
///     /// </summary>
///     public class ProtocolMvdAnnexMap : BaseGkhEntityMap<ProtocolMvdAnnex>
///     {
///         public ProtocolMvdAnnexMap()
///             : base("GJI_PROT_MVD_ANNEX")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             References(x => x.ProtocolMvd, "PROT_MVD_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Приложения в протоколе МВД"</summary>
    public class ProtocolMvdAnnexMap : BaseEntityMap<ProtocolMvdAnnex>
    {
        
        public ProtocolMvdAnnexMap() : 
                base("Приложения в протоколе МВД", "GJI_PROT_MVD_ANNEX")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Reference(x => x.ProtocolMvd, "Протокол МВД").Column("PROT_MVD_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Property(x => x.MessageCheck, "Статус файла").Column("MESSAGE_CHECK").NotNull();
        }
    }
}
