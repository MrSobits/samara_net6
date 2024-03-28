namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2018090700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018090700")]
    [MigrationDependsOn(typeof(Version_2018070500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_WARNING_DOC_ROBJECT",
                new RefColumn("WARNING_DOC_ID", ColumnProperty.NotNull, "GJI_WARNING_DOC_ROBJECT_ACTISOLATED_ID", "GJI_WARNING_DOC", "ID"),
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "GJI_WARNING_DOC_ROBJECT_REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID"));
        }
        public override void Down()
        {
            this.Database.RemoveTable("GJI_WARNING_DOC_ROBJECT");
        }
    }
}