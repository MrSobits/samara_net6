namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_PRESCRIPTION_REALOBJ",
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "GJI_PRESCRIPTION_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("PRESCRIPTION_ID", ColumnProperty.NotNull, "GJI_PRESCRIPTION_DOC", "GJI_PRESCRIPTION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_PRESCRIPTION_REALOBJ");
        }
    }
}
