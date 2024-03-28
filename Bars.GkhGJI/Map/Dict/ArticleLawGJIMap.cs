/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "статьи закона"
///     /// </summary>
///     public class ArticleLawGjiMap : BaseGkhEntityMap<ArticleLawGji>
///     {
///         public ArticleLawGjiMap()
///             : base("GJI_DICT_ARTICLELAW")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.Code, "Code").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///             Map(x => x.Part, "PART").Length(50);
///             Map(x => x.Article, "ARTICLE").Length(50);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "статья закона ГЖИ"</summary>
    public class ArticleLawGjiMap : BaseEntityMap<ArticleLawGji>
    {
        
        public ArticleLawGjiMap() : 
                base("статья закона ГЖИ", "GJI_DICT_ARTICLELAW")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Code").Column("CODE").Length(300);
            Property(x => x.KBK, "KBK").Column("KBK").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Property(x => x.Part, "Часть").Column("PART").Length(50);
            Property(x => x.Article, "Статья").Column("ARTICLE").Length(50);
            Property(x => x.GisGkhCode, "Код ГИС ЖКХ").Column("GIS_GKH_CODE");
            Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
        }
    }
}
