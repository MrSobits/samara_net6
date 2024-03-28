/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map.Version
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class VersionParamMap : BaseEntityMap<VersionParam>
///     {
///         public VersionParamMap()
///             : base("OVRHL_VERSION_PRM")
///         {
///             References(x => x.ProgramVersion, "VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Code, "CODE", true, 300);
///             Map(x => x.Weight, "WEIGHT", true);
///             References(x => x.Municipality, "MU_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.VersionParam"</summary>
    public class VersionParamMap : BaseEntityMap<VersionParam>
    {
        
        public VersionParamMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.VersionParam", "OVRHL_VERSION_PRM")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ProgramVersion, "ProgramVersion").Column("VERSION_ID").NotNull().Fetch();
            Property(x => x.Code, "Code").Column("CODE").Length(300).NotNull();
            Property(x => x.Weight, "Weight").Column("WEIGHT").NotNull();
            Reference(x => x.Municipality, "Municipality").Column("MU_ID").Fetch();
        }
    }
}
