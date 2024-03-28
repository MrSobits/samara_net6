namespace Bars.GkhGji.Regions.Khakasia.Map.AppealCits
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities.AppealCits;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.AppealCits.CheckTimeChange"</summary>
    public class CheckTimeChangeMap : BaseEntityMap<CheckTimeChange>
    {
        
        public CheckTimeChangeMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.AppealCits.CheckTimeChange", "GJI_CHECK_TIME_CH")
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
