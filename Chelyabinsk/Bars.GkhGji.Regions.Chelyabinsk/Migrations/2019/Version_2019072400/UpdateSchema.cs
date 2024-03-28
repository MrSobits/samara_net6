namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019072400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019072400")]
    [MigrationDependsOn(typeof(Version_2019072300.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_CH_ROM_VPR_PRESCRIPTION",
            new Column("STATE_NAME", DbType.String, ColumnProperty.None),
            new RefColumn("PRESCRIPTION_ID", ColumnProperty.None, "ROM_CATEGORY_VPR_PRESCRIPTION_ID", "GJI_PRESCRIPTION", "ID"),
            new RefColumn("ROM_CATEGORY_ID", ColumnProperty.None, "ROM_CATEGORY_VPR_PRESCR_CATEGORY_ID", "GJI_CH_ROM_CATEGORY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_ROM_VPR_PRESCRIPTION");
        }
    }
}


