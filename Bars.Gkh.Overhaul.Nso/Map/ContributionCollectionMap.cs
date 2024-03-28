/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class ContributionCollectionMap : BaseEntityMap<ContributionCollection>
///     {
///         public ContributionCollectionMap(): base("OVRHL_CONTRIBUT_COLLECT")
///         {
///             References(x => x.LongTermPrObject, "LONG_TERM_OBJ_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Date, "COL_DATE");
///             Map(x => x.PersonalAccount, "PERSONAL_ACCOUNT");
///             Map(x => x.AreaOwnerAccount, "AREA_OWNER_ACCOUNT");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.ContributionCollection"</summary>
    public class ContributionCollectionMap : BaseEntityMap<ContributionCollection>
    {
        
        public ContributionCollectionMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.ContributionCollection", "OVRHL_CONTRIBUT_COLLECT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.LongTermPrObject, "LongTermPrObject").Column("LONG_TERM_OBJ_ID").NotNull().Fetch();
            Property(x => x.Date, "Date").Column("COL_DATE");
            Property(x => x.PersonalAccount, "PersonalAccount").Column("PERSONAL_ACCOUNT").Length(250);
            Property(x => x.AreaOwnerAccount, "AreaOwnerAccount").Column("AREA_OWNER_ACCOUNT");
        }
    }
}
