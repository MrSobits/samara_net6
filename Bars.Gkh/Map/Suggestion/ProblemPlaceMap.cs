/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Suggestion
/// {
///     using Entities.Suggestion;
/// 
///     public class ProblemPlaceMap : BaseGkhEntityMap<ProblemPlace>
///     {
///         public ProblemPlaceMap() : base("GKH_DICT_PROBLEM_PLACE")
///         {
///             Map(x => x.Name, "NAME");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Suggestion
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Suggestion;
    
    
    /// <summary>Маппинг для "Место проблемы"</summary>
    public class ProblemPlaceMap : BaseImportableEntityMap<ProblemPlace>
    {
        
        public ProblemPlaceMap() : 
                base("Место проблемы", "GKH_DICT_PROBLEM_PLACE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Name").Column("NAME");
        }
    }
}
