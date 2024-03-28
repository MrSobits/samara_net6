/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Entities;
///     using Bars.B4.DataAccess;
/// 
///     public class FieldRequirementMap : BaseEntityMap<FieldRequirement>
///     {
///         public FieldRequirementMap()
///             : base("GKH_FIELD_REQUIREMENT")
///         {
///             Map(x => x.RequirementId, "REQUIREMENTID").Length(512).Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.FieldRequirement"</summary>
    public class FieldRequirementMap : BaseEntityMap<FieldRequirement>
    {
        
        public FieldRequirementMap() : 
                base("Bars.Gkh.Entities.FieldRequirement", "GKH_FIELD_REQUIREMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.RequirementId, "RequirementId").Column("REQUIREMENTID").Length(512).NotNull();
        }
    }
}
