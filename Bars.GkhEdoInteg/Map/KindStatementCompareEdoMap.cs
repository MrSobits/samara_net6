/// <mapping-converter-backup>
/// namespace Bars.GkhEdoInteg.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhEdoInteg.Entities;
/// 
///     public class KindStatementCompareEdoMap : BaseGkhEntityMap<KindStatementCompareEdo>
///     {
///         public KindStatementCompareEdoMap()
///             : base("INTGEDO_KINDSTATEM")
///         {
///             References(x => x.KindStatement, "KINDSTATEM_ID").Not.Nullable().Fetch.Join();
///             Map(x => x.CodeEdo, "CODE_EDO");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhEdoInteg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhEdoInteg.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhEdoInteg.Entities.KindStatementCompareEdo"</summary>
    public class KindStatementCompareEdoMap : BaseEntityMap<KindStatementCompareEdo>
    {
        
        public KindStatementCompareEdoMap() : 
                base("Bars.GkhEdoInteg.Entities.KindStatementCompareEdo", "INTGEDO_KINDSTATEM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.CodeEdo, "CodeEdo").Column("CODE_EDO");
            Reference(x => x.KindStatement, "KindStatement").Column("KINDSTATEM_ID").NotNull().Fetch();
        }
    }
}
