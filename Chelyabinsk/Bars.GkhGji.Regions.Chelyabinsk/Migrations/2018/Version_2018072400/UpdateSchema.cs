namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2018072400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018072400")]
    [MigrationDependsOn(typeof(Version_2018060400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("REASON", DbType.String, 512));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_GMP", "REASON");
        }
    }
}