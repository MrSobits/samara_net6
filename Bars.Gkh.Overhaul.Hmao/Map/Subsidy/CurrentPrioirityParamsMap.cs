/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map.Subsidy
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class CurrentPrioirityParamsMap : BaseImportableEntityMap<CurrentPrioirityParams>
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

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Текущие параметры оценки"</summary>
    public class CurrentPrioirityParamsMap : BaseImportableEntityMap<CurrentPrioirityParams>
    {
        
        public CurrentPrioirityParamsMap() : 
                base("Текущие параметры оценки", "OVRHL_CURR_PRIORITY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE").Length(300).NotNull();
            Property(x => x.Order, "Порядок сортировки").Column("C_ORDER").NotNull();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MU_ID").Fetch();
        }
    }
}
