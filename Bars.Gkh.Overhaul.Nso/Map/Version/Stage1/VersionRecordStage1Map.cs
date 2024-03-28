/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class VersionRecordStage1Map : BaseEntityMap<VersionRecordStage1>
///     {
///         public VersionRecordStage1Map()
///             : base("OVRHL_STAGE1_VERSION")
///         {
///             References(x => x.RealityObject, "REALITY_OBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Stage2Version, "STAGE2_VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.StructuralElement, "STRUCT_EL_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Map(x => x.Year, "YEAR", true, 0);
///             Map(x => x.LastOverhaulYear, "LAST_YEAR", true, 0);
///             Map(x => x.Sum, "SUM", true, 0);
///             Map(x => x.SumService, "SUM_SERVICE", true, 0);
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
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.VersionRecordStage1"</summary>
    public class VersionRecordStage1Map : BaseEntityMap<VersionRecordStage1>
    {
        
        public VersionRecordStage1Map() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.VersionRecordStage1", "OVRHL_STAGE1_VERSION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Stage2Version, "Stage2Version").Column("STAGE2_VERSION_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.StructuralElement, "StructuralElement").Column("STRUCT_EL_ID").NotNull().Fetch();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.LastOverhaulYear, "LastOverhaulYear").Column("LAST_YEAR").NotNull();
            Property(x => x.Volume, "Volume").Column("VOLUME").NotNull();
            Property(x => x.Wearout, "Wearout").Column("WEAROUT").NotNull();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            Property(x => x.SumService, "SumService").Column("SUM_SERVICE").NotNull();
        }
    }
}
