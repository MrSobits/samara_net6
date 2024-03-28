/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Проверка ГЖИ по обращению граждан - Запрос"
///     /// </summary>
///     public class AppealCitsRequestMap : BaseGkhEntityMap<AppealCitsRequest>
///     {
///         public AppealCitsRequestMap()
///             : base("GJI_APPCIT_REQUEST")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(300);
///             Map(x => x.DocumentNumber, "DOCUMENT_NUM").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///             Map(x => x.PerfomanceDate, "PERFORMANCE_DATE");
///             Map(x => x.PerfomanceFactDate, "PERFORMANCE_FACT_DATE");
/// 
///             References(x => x.AppealCits, "APPCIT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.CompetentOrg, "COMPETENT_ORG_ID").Fetch.Join();
///             References(x => x.File, "FILE_INFO_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Обращениям граждан - Запрос"</summary>
    public class AppealCitsRequestMap : BaseEntityMap<AppealCitsRequest>
    {
        
        public AppealCitsRequestMap() : 
                base("Обращениям граждан - Запрос", "GJI_APPCIT_REQUEST")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Property(x => x.PerfomanceDate, "Дата исполнения").Column("PERFORMANCE_DATE");
            Property(x => x.PerfomanceFactDate, "Дата фактического исполнения").Column("PERFORMANCE_FACT_DATE");
            Reference(x => x.AppealCits, "Обращение граждан").Column("APPCIT_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_INFO_ID").Fetch();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Reference(x => x.SenderInspector, "Инспектор").Column("SENDER_INSPECTOR_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.Signer, "Подписант").Column("SIGNER_ID").Fetch();
        }
    }
}
