/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Расценка работы в Регионе расширенная полями
///     /// </summary>
///     public class HmaoWorkPriceMap : SubclassMap<HmaoWorkPrice>
///     {
///         public HmaoWorkPriceMap()
///         {
///             Table("HMAO_OVRHL_DICT_WORKPRICE");
///             KeyColumn("ID");
/// 
///             References(x => x.RealEstateType, "REAL_ESTATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "ущность WorkPrice расширяется новым полем ТипДома"</summary>
    public class HmaoWorkPriceMap : JoinedSubClassMap<HmaoWorkPrice>
    {
        
        public HmaoWorkPriceMap() : 
                base("ущность WorkPrice расширяется новым полем ТипДома", "HMAO_OVRHL_DICT_WORKPRICE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealEstateType, "RealEstateType").Column("REAL_ESTATE_ID").Fetch();
        }
    }
}
