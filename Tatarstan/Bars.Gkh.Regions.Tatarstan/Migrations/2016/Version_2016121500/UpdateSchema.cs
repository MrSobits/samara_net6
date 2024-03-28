namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016121500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

	[Migration("2016121500")]
    [MigrationDependsOn(typeof(Version_2016121400.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("CLW_PROP_SEIZURE", new Column("YEAR", DbType.String, 255));
            this.Database.ChangeColumn("CLW_DEPARTURE_RESTRICTION", new Column("YEAR", DbType.String, 255));
        }

        public override void Down()
        {
        }
    }
}