/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gji.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Bars.GkhGji.Entities;
/// 
///     public class GjiParamMap : BaseEntityMap<GjiParam>
///     {
///         public GjiParamMap()
///             : base("GJI_PARAMETER")
///         {
///             Map(x => x.Key, "KEY", true);
///             Map(x => x.Value, "VALUE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.GjiParam"</summary>
    public class GjiParamMap : BaseEntityMap<GjiParam>
    {
        
        public GjiParamMap() : 
                base("Bars.GkhGji.Entities.GjiParam", "GJI_PARAMETER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Key, "Ключ").Column("KEY").Length(250).NotNull();
            Property(x => x.Value, "Значение").Column("VALUE").Length(250);
        }
    }
}
