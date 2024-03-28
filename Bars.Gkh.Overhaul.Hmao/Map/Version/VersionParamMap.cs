/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class VersionParamMap : BaseImportableEntityMap<VersionParam>
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

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Hmao.Entities.VersionParam"</summary>
    public class VersionParamMap : BaseImportableEntityMap<VersionParam>
    {
        
        public VersionParamMap() : 
                base("Bars.Gkh.Overhaul.Hmao.Entities.VersionParam", "OVRHL_VERSION_PRM")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ProgramVersion, "ProgramVersion").Column("VERSION_ID").NotNull().Fetch();
            Property(x => x.Code, "Код").Column("CODE").Length(300).NotNull();
            Property(x => x.Weight, "Вес").Column("WEIGHT").NotNull();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MU_ID").Fetch();
        }
    }
}
