namespace Bars.Gkh.Regions.Tatarstan.Migrations._2021.Version_2021040100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [MigrationDependsOn(typeof(_2021.Version_2021033000.UpdateSchema))]
    [Migration("2021040100")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_OBJ_INTERCOM", new Column("UNIT_MEASURE", DbType.Int32));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_OBJ_INTERCOM", "UNIT_MEASURE");
        }
    }
}
