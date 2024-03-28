/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Gkh.Map;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности связи подтематики образения и характеристики нарушения
///     /// </summary>
///     public class StatSubsubjectFeatureGjiMap : BaseGkhEntityMap<StatSubsubjectFeatureGji>
///     {
///         public StatSubsubjectFeatureGjiMap() : base("GJI_DICT_STATSUBSUBJ_FEAT")
///         {
///             References(x => x.FeatureViol, "FEATURE_VIOL_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Subsubject, "SUB_SUBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "сущность связи между подтематикой обращения и характеристикой нарушения"</summary>
    public class StatSubsubjectFeatureGjiMap : BaseEntityMap<StatSubsubjectFeatureGji>
    {
        
        public StatSubsubjectFeatureGjiMap() : 
                base("сущность связи между подтематикой обращения и характеристикой нарушения", "GJI_DICT_STATSUBSUBJ_FEAT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.FeatureViol, "Характеристика нарушения").Column("FEATURE_VIOL_ID").NotNull().Fetch();
            Reference(x => x.Subsubject, "Подтематика").Column("SUB_SUBJECT_ID").NotNull().Fetch();
        }
    }
}
