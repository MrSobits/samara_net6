/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Dict
/// {
///     using Bars.Gkh.Entities.Dicts;
/// 
///     /// <summary>
///     /// Маппинг сущности "Основание нецелесообразности"
///     /// </summary>
///     public class ReasonInexpedientMap : BaseGkhEntityMap<ReasonInexpedient>
///     {
///         public ReasonInexpedientMap() : base("GKH_DICT_REAS_INEXPEDIENT")
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
    
    
    /// <summary>Маппинг для "Основание нецелесообразности"</summary>
    public class ReasonInexpedientMap : BaseImportableEntityMap<ReasonInexpedient>
    {
        
        public ReasonInexpedientMap() : 
                base("Основание нецелесообразности", "GKH_DICT_REAS_INEXPEDIENT")
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
