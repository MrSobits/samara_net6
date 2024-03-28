/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Entities;
/// 
///     public class GkhParamMap : BaseEntityMap<GkhParam>
///     {
///         public GkhParamMap()
///             : base("GKH_PARAMETER")
///         {
///             Map(x => x.Prefix, "PREFIX", false, 100);
///             Map(x => x.Key, "KEY", true, 100);
///             Map(x => x.Value, "VALUE", false, 250);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.GkhParam"</summary>
    public class GkhParamMap : BaseEntityMap<GkhParam>
    {
        
        public GkhParamMap() : 
                base("Bars.Gkh.Entities.GkhParam", "GKH_PARAMETER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Prefix, "Префикс").Column("PREFIX").Length(100);
            Property(x => x.Key, "Ключ").Column("KEY").Length(100).NotNull();
            Property(x => x.Value, "Значение").Column("VALUE").Length(250);
        }
    }
}
