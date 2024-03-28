/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Entities;
///     using Gkh.Map;
/// 
///     /// <summary>
///     /// Маппинг сущности "Подтематика тематики обращения"
///     /// </summary>
///     public class StatSubjectSubsubjectGjiMap : BaseGkhEntityMap<StatSubjectSubsubjectGji>
///     {
///         public StatSubjectSubsubjectGjiMap() : base("GJI_DICT_STATSUBJ_SUBSUBJ")
///         {
///             References(x => x.Subject, "SUBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Subsubject, "SUBSUBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Подтематика тематики"</summary>
    public class StatSubjectSubsubjectGjiMap : BaseEntityMap<StatSubjectSubsubjectGji>
    {
        
        public StatSubjectSubsubjectGjiMap() : 
                base("Подтематика тематики", "GJI_DICT_STATSUBJ_SUBSUBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.Subject, "Тематика").Column("SUBJECT_ID").NotNull().Fetch();
            Reference(x => x.Subsubject, "Подтематика").Column("SUBSUBJECT_ID").NotNull().Fetch();
        }
    }
}
