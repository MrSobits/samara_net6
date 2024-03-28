namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2021121400
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2021121400")]
    [MigrationDependsOn(typeof(Version_2021102200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_ROM_CATEGORY_ROBJECT", new Column("HAS_VDGO", DbType.Int32, 4, ColumnProperty.NotNull, 30));

            Database.AddEntityTable("GJI_CH_ROM_G1_RESOLUTION",
           new Column("ART_LAW_NAMES", DbType.String, ColumnProperty.None),
           new RefColumn("RESOLUTION_ID", ColumnProperty.None, "ROM_CATEGORY_G1_RESOLUTION_ID", "GJI_RESOLUTION", "ID"),
           new RefColumn("ROM_CATEGORY_ID", ColumnProperty.None, "ROM_CATEGORY_G1_CATEGORY_ID", "GJI_CH_ROM_CATEGORY", "ID"));

            Database.AddColumn("GJI_CH_ROM_CATEGORY", new Column("PROBALITY_GROUP", DbType.Int32, 4, ColumnProperty.NotNull, 0));
            Database.AddColumn("GJI_CH_ROM_CATEGORY", new Column("SEVERITY_GROUP", DbType.Int32, 4, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_ROM_CATEGORY", "SEVERITY_GROUP");
            Database.RemoveColumn("GJI_CH_ROM_CATEGORY", "PROBALITY_GROUP");
            Database.RemoveTable("GJI_CH_ROM_G1_RESOLUTION");
            Database.RemoveColumn("GJI_CH_ROM_CATEGORY_ROBJECT", "HAS_VDGO");
        }
    }
}