/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Оспаривание постановления ГЖИ"
///     /// </summary>
///     public class ResolutionDisputeMap : BaseGkhEntityMap<ResolutionDispute>
///     {
///         public ResolutionDisputeMap()
///             : base("GJI_RESOLUTION_DISPUTE")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(50);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.ProsecutionProtest, "PROSECUTION_PROTEST").Not.Nullable();
///             Map(x => x.Appeal, "APPEAL").Not.Nullable().CustomType<ResolutionAppealed>();
/// 
///             References(x => x.Resolution, "RESOLUTION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///             References(x => x.Court, "COURT_ID").Fetch.Join();
///             References(x => x.Instance, "INSTANTION_ID").Fetch.Join();
///             References(x => x.CourtVerdict, "COURTVERDICT_ID").Fetch.Join();
///             References(x => x.Lawyer, "INSPECTOR_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Оспаривание постановления ГЖИ"</summary>
    public class ResolutionDisputeMap : BaseEntityMap<ResolutionDispute>
    {
        
        public ResolutionDisputeMap() : 
                base("Оспаривание постановления ГЖИ", "GJI_RESOLUTION_DISPUTE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNum, "номер документа").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.ProsecutionProtest, "Протест прокуратуры").Column("PROSECUTION_PROTEST").NotNull();
            Property(x => x.Appeal, "Постановление обжаловано").Column("APPEAL").NotNull();
            Reference(x => x.Resolution, "постановление").Column("RESOLUTION_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.Court, "вид суда").Column("COURT_ID").Fetch();
            Reference(x => x.Instance, "Инстанция").Column("INSTANTION_ID").Fetch();
            Reference(x => x.CourtVerdict, "Решение суда").Column("COURTVERDICT_ID").Fetch();
            Reference(x => x.Lawyer, "Юрист").Column("INSPECTOR_ID").Fetch();
        }
    }
}
