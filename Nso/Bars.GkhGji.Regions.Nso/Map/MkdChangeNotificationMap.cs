/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map
/// {
/// 	using Bars.B4.DataAccess;
/// 	using Bars.GkhGji.Regions.Nso.Entities;
/// 
/// 	public class MkdChangeNotificationMap : BaseEntityMap<MkdChangeNotification>
/// 	{
/// 		public MkdChangeNotificationMap()
/// 			: base("GJI_MKD_CHANGE_NOTIFICATION")
/// 		{
/// 			Map(x => x.RegistrationNumber, "REGISTRATION_NUMBER");
/// 			Map(x => x.Date, "DATE");
/// 			Map(x => x.InboundNumber, "INBOUND_NUMBER").Length(50);
/// 			Map(x => x.RegistrationDate, "REGISTRATION_DATE");
/// 			Map(x => x.OldInn, "OLD_INN").Length(20);
/// 			Map(x => x.OldOgrn, "OLD_OGRN").Length(250);
/// 			Map(x => x.NewInn, "NEW_INN").Length(20);
/// 			Map(x => x.NewOgrn, "NEW_OGRN").Length(250);
/// 			Map(x => x.NewJuridicalAddress, "NEW_JURIDICAL_ADDRESS").Length(500);
/// 			Map(x => x.NewManager, "NEW_MANAGER").Length(100);
/// 			Map(x => x.NewPhone, "NEW_PHONE").Length(2000);
/// 			Map(x => x.NewEmail, "NEW_EMAIL").Length(200);
/// 			Map(x => x.NewOfficialSite, "NEW_OFFICIAL_SITE").Length(250);
/// 			Map(x => x.NewActCopyDate, "NEW_ACT_COPY_DATE");
/// 
/// 			References(x => x.RealityObjectFantom, "REALITY_OBJECT_FANTOM_ID").Not.Nullable();
/// 			References(x => x.NotificationCause, "NOTIFICATION_CAUSE_ID").Not.Nullable();
/// 			References(x => x.OldMkdManagementMethod, "MKD_MANAGEMENT_METHOD_OLD_ID").Not.Nullable();
/// 			References(x => x.OldManagingOrganization, "MANAGING_ORGANIZATION_OLD_ID");
/// 			References(x => x.NewMkdManagementMethod, "MKD_MANAGEMENT_METHOD_NEW_ID").Not.Nullable();
/// 			References(x => x.NewManagingOrganization, "MANAGING_ORGANIZATION_NEW_ID");
/// 			References(x => x.State, "STATE_ID");
/// 		}
/// 	}
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.MkdChangeNotification"</summary>
    public class MkdChangeNotificationMap : BaseEntityMap<MkdChangeNotification>
    {
        
        public MkdChangeNotificationMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.MkdChangeNotification", "GJI_MKD_CHANGE_NOTIFICATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.RegistrationNumber, "RegistrationNumber").Column("REGISTRATION_NUMBER");
            Property(x => x.Date, "Date").Column("DATE");
            Property(x => x.InboundNumber, "InboundNumber").Column("INBOUND_NUMBER").Length(50);
            Property(x => x.RegistrationDate, "RegistrationDate").Column("REGISTRATION_DATE");
            Property(x => x.OldInn, "OldInn").Column("OLD_INN").Length(20);
            Property(x => x.OldOgrn, "OldOgrn").Column("OLD_OGRN").Length(250);
            Property(x => x.NewInn, "NewInn").Column("NEW_INN").Length(20);
            Property(x => x.NewOgrn, "NewOgrn").Column("NEW_OGRN").Length(250);
            Property(x => x.NewJuridicalAddress, "NewJuridicalAddress").Column("NEW_JURIDICAL_ADDRESS").Length(500);
            Property(x => x.NewManager, "NewManager").Column("NEW_MANAGER").Length(100);
            Property(x => x.NewPhone, "NewPhone").Column("NEW_PHONE").Length(2000);
            Property(x => x.NewEmail, "NewEmail").Column("NEW_EMAIL").Length(200);
            Property(x => x.NewOfficialSite, "NewOfficialSite").Column("NEW_OFFICIAL_SITE").Length(250);
            Property(x => x.NewActCopyDate, "NewActCopyDate").Column("NEW_ACT_COPY_DATE");
            Reference(x => x.RealityObjectFantom, "RealityObjectFantom").Column("REALITY_OBJECT_FANTOM_ID").NotNull();
            Reference(x => x.NotificationCause, "NotificationCause").Column("NOTIFICATION_CAUSE_ID").NotNull();
            Reference(x => x.OldMkdManagementMethod, "OldMkdManagementMethod").Column("MKD_MANAGEMENT_METHOD_OLD_ID").NotNull();
            Reference(x => x.OldManagingOrganization, "OldManagingOrganization").Column("MANAGING_ORGANIZATION_OLD_ID");
            Reference(x => x.NewMkdManagementMethod, "NewMkdManagementMethod").Column("MKD_MANAGEMENT_METHOD_NEW_ID").NotNull();
            Reference(x => x.NewManagingOrganization, "NewManagingOrganization").Column("MANAGING_ORGANIZATION_NEW_ID");
            Reference(x => x.State, "State").Column("STATE_ID");
        }
    }
}
