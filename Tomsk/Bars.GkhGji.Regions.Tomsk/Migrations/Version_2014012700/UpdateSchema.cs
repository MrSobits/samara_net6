namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014012700
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_PRIM_BASESTAT_APPCIT",
                new RefColumn("BASESTAT_APPCIT_ID", ColumnProperty.NotNull, "GJI_BASESTAT_APPCIT_PRIMARY", "GJI_BASESTAT_APPCIT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_PRIM_BASESTAT_APPCIT");
        }
    }
}
