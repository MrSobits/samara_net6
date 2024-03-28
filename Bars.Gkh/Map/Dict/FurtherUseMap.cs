/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Dict
/// {
///     using Bars.Gkh.Entities.Dicts;
/// 
///     /// <summary>
///     /// Маппинг сущности "Дальнейшее использование"
///     /// </summary>
///     public class FurtherUseMap : BaseGkhEntityMap<FurtherUse>
///     {
///         public FurtherUseMap() : base("GKH_DICT_FURTHER_USE")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(300).Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Dicts
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;
    
    
    /// <summary>Маппинг для "Дальнейшее использование"</summary>
    public class FurtherUseMap : BaseImportableEntityMap<FurtherUse>
    {
        
        public FurtherUseMap() : 
                base("Дальнейшее использование", "GKH_DICT_FURTHER_USE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300).NotNull();
        }
    }
}
