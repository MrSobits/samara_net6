/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Saha.Map.AppealCits
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Saha.Entities.AppealCits;
/// 
///     public class CheckTimeChangeMap : BaseEntityMap<CheckTimeChange>
///     {
///         public CheckTimeChangeMap()
///             : base("GJI_CHECK_TIME_CH")
///         {
///             Map(x => x.NewValue, "NEW_VALUE", false);
///             Map(x => x.OldValue, "OLD_VALUE", false);
/// 
///             References(x => x.User, "USER_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.AppealCits, "APPEAL_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Saha.Map.AppealCits
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Saha.Entities.AppealCits;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Saha.Entities.AppealCits.CheckTimeChange"</summary>
    public class CheckTimeChangeMap : BaseEntityMap<CheckTimeChange>
    {
        
        public CheckTimeChangeMap() : 
                base("Bars.GkhGji.Regions.Saha.Entities.AppealCits.CheckTimeChange", "GJI_CHECK_TIME_CH")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.AppealCits, "AppealCits").Column("APPEAL_ID").NotNull().Fetch();
            Property(x => x.OldValue, "OldValue").Column("OLD_VALUE");
            Property(x => x.NewValue, "NewValue").Column("NEW_VALUE");
            Reference(x => x.User, "User").Column("USER_ID").NotNull().Fetch();
        }
    }
}
