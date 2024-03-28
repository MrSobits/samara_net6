/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.Dict
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities.Dict;
/// 
///     public class HeatingSeasonResolutionMap : BaseEntityMap<HeatingSeasonResolution>
///     {
///         public HeatingSeasonResolutionMap()
///             : base("GJI_DICT_HEAT_SEAS_RESOL")
///         {
///             Map(x => x.AcceptDate, "ACCEPT_DATE");
///             References(x => x.Doc, "DOC_ID");
///             References(x => x.HeatSeasonPeriodGji, "PERIOD_ID");
///             References(x => x.Municipality, "MUNICIPALITY_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    
    
    /// <summary>Маппинг для "Постановления о пуске тепла"</summary>
    public class HeatingSeasonResolutionMap : BaseEntityMap<HeatingSeasonResolution>
    {
        
        public HeatingSeasonResolutionMap() : 
                base("Постановления о пуске тепла", "GJI_DICT_HEAT_SEAS_RESOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.AcceptDate, "Дата принятия постановления о пуске тепла").Column("ACCEPT_DATE");
            Reference(x => x.Doc, "Документ").Column("DOC_ID");
            Reference(x => x.HeatSeasonPeriodGji, "Период отопительного сезона").Column("PERIOD_ID");
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID");
        }
    }
}
