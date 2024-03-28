/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Regions.Msk.Entities;
/// 
///     public class LiftInfoMap : BaseEntityMap<LiftInfo>
///     {
///         public LiftInfoMap()
///             : base("MSK_LIFT_INFO")
///         {
///             Map(x => x.Capacity, "CAPACITY");
///             Map(x => x.StopCount, "STOP_COUNT");
///             Map(x => x.LifeTime, "LIFETIME");
///             Map(x => x.InstallationYear, "INSTALL_YEAR");
///             Map(x => x.Period, "PERIOD");
///             Map(x => x.Porch, "PORCH");
/// 
///             References(x => x.RealityObjectInfo, "RO_INFO_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Regions.Msk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Msk.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Regions.Msk.Entities.LiftInfo"</summary>
    public class LiftInfoMap : BaseEntityMap<LiftInfo>
    {
        
        public LiftInfoMap() : 
                base("Bars.Gkh.Regions.Msk.Entities.LiftInfo", "MSK_LIFT_INFO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObjectInfo, "RealityObjectInfo").Column("RO_INFO_ID").NotNull().Fetch();
            Property(x => x.Porch, "Porch").Column("PORCH").Length(250);
            Property(x => x.Capacity, "Capacity").Column("CAPACITY");
            Property(x => x.StopCount, "StopCount").Column("STOP_COUNT");
            Property(x => x.InstallationYear, "InstallationYear").Column("INSTALL_YEAR");
            Property(x => x.LifeTime, "LifeTime").Column("LIFETIME").Length(250);
            Property(x => x.Period, "Period").Column("PERIOD").Length(250);
        }
    }
}
