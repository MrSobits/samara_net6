namespace Bars.Gkh.Migrations.Version_2013041000
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013041000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013040200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GKH_OBJ_HOUSE_INFO", new Column("NUM", DbType.String, 300));
            Database.ChangeColumn("GKH_OBJ_HOUSE_INFO", new Column("NUM_REG_RIGHT", DbType.String, 300));
            Database.ChangeColumn("GKH_OBJ_HOUSE_INFO", new Column("OWNER", DbType.String, 300));

            Database.ChangeColumn("GKH_OBJ_APARTMENT_INFO", new Column("PHONE", DbType.String, 300));
            Database.ChangeColumn("GKH_OBJ_APARTMENT_INFO", new Column("NUM_APARTMENT", DbType.String, 300));

            Database.ChangeColumn("GKH_OBJ_IMAGE", new Column("DESCRIPTION", DbType.String, 2000));
            Database.ChangeColumn("GKH_OBJ_IMAGE", new Column("NAME", DbType.String, 300));

            Database.ChangeColumn("GKH_EMERGENCY_OBJECT", new Column("DESCRIPTION", DbType.String, 2000));

            Database.ChangeColumn("GKH_OBJ_METERING_DEVICE", new Column("DESCRIPTION", DbType.String, 2000));

            Database.ChangeColumn("GKH_OBJ_PROTOCOL_MT", new Column("DOCUMENT_NUM", DbType.String, 300));

            Database.ChangeColumn("GKH_MANAGING_ORGANIZATION", new Column("OFFICIAL_SITE", DbType.String, 300));

            Database.ChangeColumn("GKH_CONTRAGENT", new Column("INN", DbType.String, 20));
            Database.ChangeColumn("GKH_CONTRAGENT", new Column("KPP", DbType.String, 20));
            Database.ChangeColumn("GKH_CONTRAGENT", new Column("DESCRIPTION", DbType.String, 2000));
            Database.ChangeColumn("GKH_CONTRAGENT", new Column("PHONE", DbType.String, 2000));
            Database.ChangeColumn("GKH_CONTRAGENT", new Column("EMAIL", DbType.String, 200));
            Database.ChangeColumn("GKH_CONTRAGENT", new Column("OGRN", DbType.String, 250));
            Database.ChangeColumn("GKH_CONTRAGENT", new Column("OFFICIAL_WEBSITE", DbType.String, 250));
            Database.ChangeColumn("GKH_CONTRAGENT", new Column("PHONE_DISPATCH_SERVICE", DbType.String, 100));
        }

        public override void Down()
        {
        }
    }
}