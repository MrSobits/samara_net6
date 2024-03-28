namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015072101
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015072101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015072100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			this.Database.AddEntityTable("GJI_REALITY_OBJECT_FANTOM",
				new RefColumn("REALITY_OBJECT_ID", "REALITY_OBJECT_FANTOM_RO", "GKH_REALITY_OBJECT", "ID"),
                new Column("FANTOM", DbType.String, 1000, ColumnProperty.Null));

	        this.Database.AddEntityTable("GJI_MKD_CHANGE_NOTIFICATION",
		        new Column("REGISTRATION_NUMBER", DbType.Int16, ColumnProperty.NotNull),
		        new Column("DATE", DbType.DateTime, ColumnProperty.NotNull),
		        new Column("INBOUND_NUMBER", DbType.String, 50, ColumnProperty.NotNull),
		        new Column("REGISTRATION_DATE", DbType.DateTime, ColumnProperty.NotNull),
		        new Column("OLD_INN", DbType.String, 20, ColumnProperty.Null),
		        new Column("OLD_OGRN", DbType.String, 250, ColumnProperty.Null),
		        new Column("NEW_INN", DbType.String, 20, ColumnProperty.Null),
		        new Column("NEW_OGRN", DbType.String, 250, ColumnProperty.Null),
		        new Column("NEW_JURIDICAL_ADDRESS", DbType.String, 500, ColumnProperty.Null),
		        new Column("NEW_MANAGER", DbType.String, 100, ColumnProperty.Null),
		        new Column("NEW_PHONE", DbType.String, 2000, ColumnProperty.Null),
		        new Column("NEW_EMAIL", DbType.String, 200, ColumnProperty.Null),
		        new Column("NEW_OFFICIAL_SITE", DbType.String, 250, ColumnProperty.Null),
		        new Column("NEW_ACT_COPY_DATE", DbType.DateTime, ColumnProperty.Null),
				new RefColumn("REALITY_OBJECT_FANTOM_ID", "MKD_CHANGE_NOTIFICATION_ROF", "GJI_REALITY_OBJECT_FANTOM", "ID"),
				new RefColumn("NOTIFICATION_CAUSE_ID", "MKD_CHANGE_NOTIFICATION_NC", "GJI_NOTIFICATION_CAUSE", "ID"),
				new RefColumn("MKD_MANAGEMENT_METHOD_OLD_ID", "MKD_CHANGE_NOTIFICATION_OMM", "GJI_MKD_MANAGEMENT_METHOD", "ID"),
				new RefColumn("MANAGING_ORGANIZATION_OLD_ID", "MKD_CHANGE_NOTIFICATION_OMOR", "GKH_MANAGING_ORGANIZATION", "ID"),
				new RefColumn("MKD_MANAGEMENT_METHOD_NEW_ID", "MKD_CHANGE_NOTIFICATION_NMM", "GJI_MKD_MANAGEMENT_METHOD", "ID"),
				new RefColumn("MANAGING_ORGANIZATION_NEW_ID", "MKD_CHANGE_NOTIFICATION_NMOR", "GKH_MANAGING_ORGANIZATION", "ID"),
		        new RefColumn("STATE_ID", "MKD_CHANGE_NOTIFICATION_ST", "B4_STATE", "ID")
		   );
        }

        public override void Down()
        {
			this.Database.RemoveTable("GJI_REALITY_OBJECT_FANTOM");
			this.Database.RemoveTable("GJI_MKD_CHANGE_NOTIFICATION");
        }
    }
}