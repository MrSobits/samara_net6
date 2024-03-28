/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.RealityObject
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
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
///             Map(x => x.LastOverhaulYear, "LAST_YEAR", true, 0);
///             Map(x => x.Volume, "VOLUME", true, 0);
///             Map(x => x.Wearout, "WEAROUT", true, 0);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.RealityObjectStructuralElementInProgramm"</summary>
    public class RealityObjectStructuralElementInProgrammMap : BaseEntityMap<RealityObjectStructuralElementInProgramm>
    {
        
        public RealityObjectStructuralElementInProgrammMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.RealityObjectStructuralElementInProgramm", "OVRHL_RO_STRUCT_EL_IN_PRG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.StructuralElement, "StructuralElement").Column("RO_SE_ID").NotNull().Fetch();
            Reference(x => x.Stage2, "Stage2").Column("STAGE2_ID").Fetch();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            Property(x => x.ServiceCost, "ServiceCost").Column("SERVICE_COST").NotNull();
            Property(x => x.LastOverhaulYear, "LastOverhaulYear").Column("LAST_YEAR").NotNull();
            Property(x => x.Volume, "Volume").Column("VOLUME").NotNull();
            Property(x => x.Wearout, "Wearout").Column("WEAROUT").NotNull();
        }
    }
}
