/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Документ отопительного сезона"
///     /// </summary>
///     public class HeatSeasonDocMap : BaseGkhEntityMap<HeatSeasonDoc>
///     {
///         public HeatSeasonDocMap() : base("GJI_HEATSEASON_DOCUMENT")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER").Length(50);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.TypeDocument, "TYPE_DOCUMENT").Not.Nullable().CustomType<HeatSeasonDocType>();
/// 
///             References(x => x.HeatingSeason, "HEATSEASON_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Документ подготовки к отопительному сезону"</summary>
    public class HeatSeasonDocMap : BaseEntityMap<HeatSeasonDoc>
    {
        
        public HeatSeasonDocMap() : 
                base("Документ подготовки к отопительному сезону", "GJI_HEATSEASON_DOCUMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER").Length(50);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.TypeDocument, "Тип документа").Column("TYPE_DOCUMENT").NotNull();
            Reference(x => x.HeatingSeason, "Отопительный сезон").Column("HEATSEASON_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
