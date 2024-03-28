/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Расценка работы в Регионе расширенная полями
///     /// </summary>
///     public class NsoWorkPriceMap : SubclassMap<NsoWorkPrice>
///     {
///         public NsoWorkPriceMap()
///         {
///             Table("NSO_OVRHL_DICT_WORKPRICE");
///             KeyColumn("ID");
/// 
///             References(x => x.RealEstateType, "REAL_ESTATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.NsoWorkPrice"</summary>
    public class NsoWorkPriceMap : JoinedSubClassMap<NsoWorkPrice>
    {
        
        public NsoWorkPriceMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.NsoWorkPrice", "NSO_OVRHL_DICT_WORKPRICE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealEstateType, "RealEstateType").Column("REAL_ESTATE_ID").Fetch();
        }
    }
}
