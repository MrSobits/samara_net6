/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "период отапительного сезона"
///     /// </summary>
///     public class HeatSeasonPeriodGjiMap : BaseGkhEntityMap<HeatSeasonPeriodGji>
///     {
///         public HeatSeasonPeriodGjiMap()
///             : base("GJI_DICT_HEATSEASONPERIOD")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Период отопительного сезона"</summary>
    public class HeatSeasonPeriodGjiMap : BaseEntityMap<HeatSeasonPeriodGji>
    {
        
        public HeatSeasonPeriodGjiMap() : 
                base("Период отопительного сезона", "GJI_DICT_HEATSEASONPERIOD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
        }
    }
}
