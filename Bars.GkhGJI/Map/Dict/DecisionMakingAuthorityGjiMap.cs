/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.Dict
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities.Dict;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Органы, принимающие решение по предписанию"
///     /// </summary>
///     public class DecisionMakingAuthorityGjiMap : BaseEntityMap<DecisionMakingAuthorityGji>
///     {
///         public DecisionMakingAuthorityGjiMap()
///             : base("GJI_DICT_DECISMAKEAUTH")
///         {
///             Map(x => x.Code, "CODE").Length(300);
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    
    
    /// <summary>Маппинг для "Органы, принимающие решение по предписанию"</summary>
    public class DecisionMakingAuthorityGjiMap : BaseEntityMap<DecisionMakingAuthorityGji>
    {
        
        public DecisionMakingAuthorityGjiMap() : 
                base("Органы, принимающие решение по предписанию", "GJI_DICT_DECISMAKEAUTH")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE").Length(300);
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
        }
    }
}
