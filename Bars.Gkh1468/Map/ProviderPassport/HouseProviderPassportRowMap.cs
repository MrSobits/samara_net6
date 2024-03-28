/// <mapping-converter-backup>
/// using Bars.B4.DataAccess.ByCode;
/// 
/// namespace Bars.Gkh1468.Map.ProviderPassport
/// {
///     using Bars.Gkh1468.Entities;
/// 
///     public class HouseProviderPassportRowMap : BaseEntityMap<HouseProviderPassportRow>
///     {
///         public HouseProviderPassportRowMap()
///             : base("GKH_HOUSE_PROV_PASS_ROW")
///         {
///             Map(x => x.Value, "VALUE");
///             Map(x => x.GroupKey, "GROUP_KEY");
///             Map(x => x.ParentValue, "PARENT_VALUE");
///             References(x => x.MetaAttribute, "META_ATTRIBUTE_ID");
///             References(x => x.ProviderPassport, "HOUSE_PROV_PASSPORT_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh1468.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh1468.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh1468.Entities.HouseProviderPassportRow"</summary>
    public class HouseProviderPassportRowMap : BaseEntityMap<HouseProviderPassportRow>
    {
        
        public HouseProviderPassportRowMap() : 
                base("Bars.Gkh1468.Entities.HouseProviderPassportRow", "GKH_HOUSE_PROV_PASS_ROW")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Value, "Value").Column("VALUE").Length(250);
            Property(x => x.GroupKey, "GroupKey").Column("GROUP_KEY");
            Property(x => x.ParentValue, "ParentValue").Column("PARENT_VALUE");
            Reference(x => x.MetaAttribute, "MetaAttribute").Column("META_ATTRIBUTE_ID");
            Reference(x => x.ProviderPassport, "ProviderPassport").Column("HOUSE_PROV_PASSPORT_ID");
        }
    }
}
