namespace Bars.Gkh.Regions.Yanao.Migrations.Version_1
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GKH_YANAO_ROBJ_EXTENSION",
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "GKH_YANAO_RO_EXT_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("TECHPASSPORT_SCAN_FILE_ID", ColumnProperty.NotNull, "GKH_YANAO_RO_EXT_SCANFILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_YANAO_ROBJ_EXTENSION");
        }
    }
}