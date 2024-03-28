/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
/// 	using Bars.B4.DataAccess;
/// 	using Bars.GkhGji.Regions.Chelyabinsk.Entities;
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

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.MkdChangeNotification"</summary>
    public class MkdChangeNotificationMap : BaseEntityMap<MkdChangeNotification>
    {
        
        public MkdChangeNotificationMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.MkdChangeNotification", "GJI_MKD_CHANGE_NOTIFICATION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.RegistrationNumber, "RegistrationNumber").Column("REGISTRATION_NUMBER");
            this.Property(x => x.Date, "Date").Column("DATE");
            this.Property(x => x.InboundNumber, "InboundNumber").Column("INBOUND_NUMBER").Length(50);
            this.Property(x => x.RegistrationDate, "RegistrationDate").Column("REGISTRATION_DATE");
            this.Property(x => x.OldInn, "OldInn").Column("OLD_INN").Length(20);
            this.Property(x => x.OldOgrn, "OldOgrn").Column("OLD_OGRN").Length(250);
            this.Property(x => x.NewInn, "NewInn").Column("NEW_INN").Length(20);
            this.Property(x => x.NewOgrn, "NewOgrn").Column("NEW_OGRN").Length(250);
            this.Property(x => x.NewJuridicalAddress, "NewJuridicalAddress").Column("NEW_JURIDICAL_ADDRESS").Length(500);
            this.Property(x => x.NewManager, "NewManager").Column("NEW_MANAGER").Length(100);
            this.Property(x => x.NewPhone, "NewPhone").Column("NEW_PHONE").Length(2000);
            this.Property(x => x.NewEmail, "NewEmail").Column("NEW_EMAIL").Length(200);
            this.Property(x => x.NewOfficialSite, "NewOfficialSite").Column("NEW_OFFICIAL_SITE").Length(250);
            this.Property(x => x.NewActCopyDate, "NewActCopyDate").Column("NEW_ACT_COPY_DATE");
            this.Reference(x => x.RealityObjectFantom, "RealityObjectFantom").Column("REALITY_OBJECT_FANTOM_ID").NotNull();
            this.Reference(x => x.NotificationCause, "NotificationCause").Column("NOTIFICATION_CAUSE_ID").NotNull();
            this.Reference(x => x.OldMkdManagementMethod, "OldMkdManagementMethod").Column("MKD_MANAGEMENT_METHOD_OLD_ID").NotNull();
            this.Reference(x => x.OldManagingOrganization, "OldManagingOrganization").Column("MANAGING_ORGANIZATION_OLD_ID");
            this.Reference(x => x.NewMkdManagementMethod, "NewMkdManagementMethod").Column("MKD_MANAGEMENT_METHOD_NEW_ID").NotNull();
            this.Reference(x => x.NewManagingOrganization, "NewManagingOrganization").Column("MANAGING_ORGANIZATION_NEW_ID");
            this.Reference(x => x.State, "State").Column("STATE_ID");
        }
    }
}
