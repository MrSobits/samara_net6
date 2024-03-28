namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2019080100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019080100")]
    [MigrationDependsOn(typeof(Version_2019052700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_PRG_VERSION", new Column("GIS_GKH_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_PRG_VERSION", "GIS_GKH_GUID");
        }
    }
}