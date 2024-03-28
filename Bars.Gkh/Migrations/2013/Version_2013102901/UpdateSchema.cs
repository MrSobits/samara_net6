namespace Bars.Gkh.Migrations.Version_2013102901
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013102900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("HCS_METER_READING", new RefColumn("ACCOUNT_ID", "HCS_METER_READ_ACC", "HCS_HOUSE_ACCOUNT", "ID"));
            Database.AddRefColumn("HCS_HOUSE_ACCOUNT_CHARGE", new RefColumn("ACCOUNT_ID", "HCS_HOUSE_ACC_CHARGE_ACC", "HCS_HOUSE_ACCOUNT", "ID"));

            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("DATE_CHARGING", DbType.DateTime));
            Database.AddColumn("HCS_HOUSE_METER_READING", new Column("METER_SERIAL", DbType.String));
        }
        
        public override void Down()
        {
            Database.RemoveColumn("HCS_METER_READING", "ACCOUNT_ID");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "ACCOUNT_ID");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "DATE_CHARGING");
            Database.RemoveColumn("HCS_HOUSE_METER_READING", "METER_SERIAL");
        }
    }
}