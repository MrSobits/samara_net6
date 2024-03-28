namespace Bars.Gkh.Regions.Tatarstan.Migrations._2021.Version_2021033000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [MigrationDependsOn(typeof(_2021.Version_2021031100.UpdateSchema))]
    [Migration("2021033000")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_OBJ_INTERCOM", new Column("INSTALLATION_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_OBJ_INTERCOM", "INSTALLATION_DATE");
        }
    }
}
