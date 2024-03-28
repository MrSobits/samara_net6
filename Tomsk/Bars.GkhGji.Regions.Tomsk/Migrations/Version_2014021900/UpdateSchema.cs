namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021900
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Приложения адм дела
            Database.AddEntityTable(
                "GJI_REQUIREMENT_DOC",
                new RefColumn("DOC_ID", ColumnProperty.NotNull, "GJI_REQUIREMENT_DOC_D", "GJI_DOCUMENT", "ID"),
                new RefColumn("REQ_ID", ColumnProperty.NotNull, "GJI_REQUIREMENT_DOC_R", "GJI_REQUIREMENT", "ID"));
            //-----
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_REQUIREMENT_DOC");
        }
    }
}
