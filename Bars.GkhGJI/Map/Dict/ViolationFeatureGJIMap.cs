/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Сущность связи между Нарушением и Характеристикой нарушения"
///     /// </summary>
///     public class ViolationFeatureGjiMap : BaseGkhEntityMap<ViolationFeatureGji>
///     {
///         public ViolationFeatureGjiMap() : base("Gji_DICT_VIOLATIONFEATURE")
///         {
///             References(x => x.ViolationGji, "VIOLATION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.FeatureViolGji, "FEATUREVIOL_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Сущность связи между Нарушением и Характеристикой нарушения"</summary>
    public class ViolationFeatureGjiMap : BaseEntityMap<ViolationFeatureGji>
    {
        
        public ViolationFeatureGjiMap() : 
                base("Сущность связи между Нарушением и Характеристикой нарушения", "GJI_DICT_VIOLATIONFEATURE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ViolationGji, "Нарушение").Column("VIOLATION_ID").NotNull().Fetch();
            Reference(x => x.FeatureViolGji, "Характеристика нарушения").Column("FEATUREVIOL_ID").NotNull().Fetch();
        }
    }
}
