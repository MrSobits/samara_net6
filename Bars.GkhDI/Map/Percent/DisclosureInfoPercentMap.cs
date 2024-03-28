/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.GkhDi.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class DisclosureInfoPercentMap : SubclassMap<DisclosureInfoPercent>
///     {
///         public DisclosureInfoPercentMap()
///         {
///             Table("DI_PERC_DINFO");
///             KeyColumn("ID");
///             References(x => x.DisclosureInfo, "DIS_INFO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.DisclosureInfoPercent"</summary>
    public class DisclosureInfoPercentMap : JoinedSubClassMap<DisclosureInfoPercent>
    {
        
        public DisclosureInfoPercentMap() : 
                base("Bars.GkhDi.Entities.DisclosureInfoPercent", "DI_PERC_DINFO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DIS_INFO_ID").NotNull().Fetch();
        }
    }
}
