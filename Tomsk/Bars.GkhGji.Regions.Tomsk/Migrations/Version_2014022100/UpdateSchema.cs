namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014022100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_REQUIREMENT_ARTICLE",
                new RefColumn("REQ_ID", ColumnProperty.NotNull, "REQUIR_ARTICLE_REQ", "GJI_REQUIREMENT", "ID"),
                new RefColumn("ARTICLE_ID", ColumnProperty.NotNull, "REQUIR_ARTICLE_ART", "GJI_DICT_ARTICLELAW", "ID"));

            Database.RemoveColumn("GJI_REQUIREMENT", "ARTICLELAW_ID");
        }

        public override void Down()
        {
            Database.AddRefColumn("GJI_REQUIREMENT", new RefColumn("ARTICLELAW_ID", "GJI_REQUIREMENT_ARTLAW", "GJI_DICT_ARTICLELAW", "ID"));

            Database.RemoveTable("GJI_REQUIREMENT_ARTICLE");
        }
    }
}