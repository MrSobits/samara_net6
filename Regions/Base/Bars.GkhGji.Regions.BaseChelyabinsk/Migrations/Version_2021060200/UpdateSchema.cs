namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021060200
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021060200")]
    [MigrationDependsOn(typeof(Version_2021052600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_LICENSE_ACTION_FILE", new Column("SERT_INFO", DbType.String, 1500, ColumnProperty.None));
            Database.AddColumn("GKH_MANORG_REQ_PROVDOC", new Column("SERT_INFO", DbType.String, 1500, ColumnProperty.None));
            Database.AddColumn("GJI_CH_LICENSE_REISSUANCE_PROVDOC", new Column("SERT_INFO", DbType.String, 1500, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_LICENSE_REISSUANCE_PROVDOC", "SERT_INFO");
            Database.RemoveColumn("GKH_MANORG_REQ_PROVDOC", "SERT_INFO");
            Database.RemoveColumn("GJI_LICENSE_ACTION_FILE", "SERT_INFO");
        }

    }
}