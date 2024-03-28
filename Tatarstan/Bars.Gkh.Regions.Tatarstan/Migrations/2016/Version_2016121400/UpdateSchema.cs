namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016121400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

	[Migration("2016121400")]
    [MigrationDependsOn(typeof(Version_2016121200.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("CLW_PROP_SEIZURE", new Column("OWNER_BANK_DETAILS", DbType.String, 255));
            this.Database.AddColumn("CLW_DEPARTURE_RESTRICTION", new Column("OWNER_BANK_DETAILS", DbType.String, 255));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CLW_PROP_SEIZURE", "OWNER_BANK_DETAILS");
            this.Database.RemoveColumn("CLW_DEPARTURE_RESTRICTION", "OWNER_BANK_DETAILS");
        }
    }
}