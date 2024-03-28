/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map.RealityObject
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     public class RealityObjectStructuralElementInProgrammMap : BaseEntityMap<RealityObjectStructuralElementInProgramm>
///     {
///         public RealityObjectStructuralElementInProgrammMap() : base("OVRHL_RO_STRUCT_EL_IN_PRG")
///         {
///             References(x => x.StructuralElement, "RO_SE_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Stage2, "STAGE2_ID", ReferenceMapConfig.Fetch);
///             Map(x => x.Year, "YEAR", true, 0);
///             Map(x => x.Sum, "SUM", true, 0);
///             Map(x => x.ServiceCost, "SERVICE_COST", true, 0);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.RealityObjectStructuralElementInProgramm"</summary>
    public class RealityObjectStructuralElementInProgrammMap : BaseEntityMap<RealityObjectStructuralElementInProgramm>
    {
        
        public RealityObjectStructuralElementInProgrammMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.RealityObjectStructuralElementInProgramm", "OVRHL_RO_STRUCT_EL_IN_PRG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.StructuralElement, "StructuralElement").Column("RO_SE_ID").NotNull().Fetch();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            Property(x => x.ServiceCost, "ServiceCost").Column("SERVICE_COST").NotNull();
            Reference(x => x.Stage2, "Stage2").Column("STAGE2_ID").Fetch();
        }
    }
}
