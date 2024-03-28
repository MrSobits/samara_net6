namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014100801
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014100800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_RESOL_LONGDESC",
                new RefColumn("RESOLUTION_ID", ColumnProperty.NotNull, "GJI_RESOL_LONGDESC", "GJI_RESOLUTION", "ID"),
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_RESOL_LONGDESC");
        }
    }
}