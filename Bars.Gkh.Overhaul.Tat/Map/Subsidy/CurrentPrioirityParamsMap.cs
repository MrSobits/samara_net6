/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map.Subsidy
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     public class CurrentPrioirityParamsMap : BaseEntityMap<CurrentPrioirityParams>
///     {
///         public CurrentPrioirityParamsMap()
///             : base("OVRHL_CURR_PRIORITY")
///         {
///             Map(x => x.Code, "CODE", true, 300);
///             Map(x => x.Order, "C_ORDER", true);
///             References(x => x.Municipality, "MU_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.CurrentPrioirityParams"</summary>
    public class CurrentPrioirityParamsMap : BaseEntityMap<CurrentPrioirityParams>
    {
        
        public CurrentPrioirityParamsMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.CurrentPrioirityParams", "OVRHL_CURR_PRIORITY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Code").Column("CODE").Length(300).NotNull();
            Property(x => x.Order, "Order").Column("C_ORDER").NotNull();
            Reference(x => x.Municipality, "Municipality").Column("MU_ID").Fetch();
        }
    }
}
