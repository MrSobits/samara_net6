namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2023082800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023082800")]
    [MigrationDependsOn(typeof(Version_2023082300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_KPKR_COST_LIMITS", new Column("YEAR", DbType.Int32));
            Database.AddColumn("OVRHL_KPKR_COST_LIMITS", new Column("RATE", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_KPKR_COST_LIMITS", "RATE");
            Database.RemoveColumn("OVRHL_KPKR_COST_LIMITS", "YEAR");
        }
    }
}