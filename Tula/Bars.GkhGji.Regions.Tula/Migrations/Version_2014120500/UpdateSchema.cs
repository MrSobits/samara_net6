namespace Bars.GkhGji.Regions.Tula.Migrations.Version_2014120500
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tula.Migrations.Version_2014112000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_RESOLUTION_LONGDESC",
                new RefColumn("RESOLUTION_ID", ColumnProperty.NotNull, "GJI_RESOLUTION_LONGDESC", "GJI_RESOLUTION", "ID"),
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_RESOLUTION_LONGDESC");
        }
    }
}
