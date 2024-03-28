namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014091500
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014091500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014091200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_PRESENT_DESCR",
                new RefColumn("PRESEN_ID", ColumnProperty.NotNull, "GJI_PRES_DESCR", "GJI_PRESENTATION", "ID"),
                new Column("DESCRIPTION_SET", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_PRESENT_DESCR");
        }
    }
}