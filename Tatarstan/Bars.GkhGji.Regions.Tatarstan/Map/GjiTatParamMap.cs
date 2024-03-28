/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tatarstan.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class GjiTatParamMap : BaseEntityMap<GjiTatParam>
///     {
///         public GjiTatParamMap() : base("GJI_TAT_PARAM")
///         {
///             Map(x => x.Prefix, "CPREFIX", true, 20);
///             Map(x => x.Key, "CKEY", true, 50);
///             Map(x => x.Value, "CVALUE", false, 2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tatarstan.Entities.GjiTatParam"</summary>
    public class GjiTatParamMap : BaseEntityMap<GjiTatParam>
    {
        
        public GjiTatParamMap() : 
                base("Bars.GkhGji.Regions.Tatarstan.Entities.GjiTatParam", "GJI_TAT_PARAM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Prefix, "Prefix").Column("CPREFIX").Length(20).NotNull();
            Property(x => x.Key, "Key").Column("CKEY").Length(50).NotNull();
            Property(x => x.Value, "Value").Column("CVALUE").Length(2000);
        }
    }
}
