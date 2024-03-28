/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Социальный статус"
///     /// </summary>
///     public class SocialStatusMap : BaseEntityMap<SocialStatus>
///     {
///         public SocialStatusMap()
///             : base("GJI_DICT_SOC_ST")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// 
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Спарвочник "социальный статус""</summary>
    public class SocialStatusMap : BaseEntityMap<SocialStatus>
    {
        
        public SocialStatusMap() : 
                base("Спарвочник \"социальный статус\"", "GJI_DICT_SOC_ST")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
