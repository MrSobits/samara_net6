namespace Bars.Gkh.Migrations.Version_2013102902
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102902")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013102901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("HCS_HOUSE_ACCOUNT", new Column("PAYMENT_CODE", DbType.Int64, 0));

            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("COMPOSITE_KEY", DbType.String, 100));

            Database.AddColumn("HCS_HOUSE_OVERALL_BALANCE", new Column("DATE_CHARGING", DbType.Date));
            Database.AddColumn("HCS_HOUSE_OVERALL_BALANCE", new Column("COMPOSITE_KEY", DbType.String, 100));

            Database.AddColumn("HCS_METER_READING", new Column("COMPOSITE_KEY", DbType.String, 100));

            Database.AddColumn("HCS_HOUSE_METER_READING", new Column("COMPOSITE_KEY", DbType.String, 100));
        }
        
        public override void Down()
        {
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "COMPOSITE_KEY");

            Database.RemoveColumn("HCS_HOUSE_OVERALL_BALANCE", "DATE_CHARGING");
            Database.RemoveColumn("HCS_HOUSE_OVERALL_BALANCE", "COMPOSITE_KEY");

            Database.RemoveColumn("HCS_METER_READING", "COMPOSITE_KEY");

            Database.RemoveColumn("HCS_HOUSE_METER_READING", "COMPOSITE_KEY");
        }
    }
}