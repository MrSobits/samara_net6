/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.RealityObject
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class RealityObjectStructuralElementInProgrammStage2Map : BaseEntityMap<RealityObjectStructuralElementInProgrammStage2>
///     {
///         public RealityObjectStructuralElementInProgrammStage2Map()
///             : base("OVRHL_RO_STRUCT_EL_IN_PRG_2")
///         {
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.CommonEstateObject, "CEO_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Stage3, "STAGE3_ID", ReferenceMapConfig.Fetch);
///             Map(x => x.Year, "YEAR", true, 0);
///             Map(x => x.Sum, "SUM", true, 0);
///             Map(x => x.StructuralElements, "SE_STRING", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.RealityObjectStructuralElementInProgrammStage2"</summary>
    public class RealityObjectStructuralElementInProgrammStage2Map : BaseEntityMap<RealityObjectStructuralElementInProgrammStage2>
    {
        
        public RealityObjectStructuralElementInProgrammStage2Map() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.RealityObjectStructuralElementInProgrammStage2", "OVRHL_RO_STRUCT_EL_IN_PRG_2")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").NotNull().Fetch();
            Reference(x => x.CommonEstateObject, "CommonEstateObject").Column("CEO_ID").NotNull().Fetch();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            Property(x => x.StructuralElements, "StructuralElements").Column("SE_STRING").Length(250).NotNull();
            Reference(x => x.Stage3, "Stage3").Column("STAGE3_ID").Fetch();
        }
    }
}
