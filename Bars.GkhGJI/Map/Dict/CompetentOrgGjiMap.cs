/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Компетентная организация"
///     /// </summary>
///     public class CompetentOrgGjiMap : BaseGkhEntityMap<CompetentOrgGji>
///     {
///         public CompetentOrgGjiMap() : base("GJI_DICT_COMPETENT_ORG")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Компетентная организация"</summary>
    public class CompetentOrgGjiMap : BaseEntityMap<CompetentOrgGji>
    {
        
        public CompetentOrgGjiMap() : 
                base("Компетентная организация", "GJI_DICT_COMPETENT_ORG")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
