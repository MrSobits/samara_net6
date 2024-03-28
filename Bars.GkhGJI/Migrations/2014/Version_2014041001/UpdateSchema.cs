namespace Bars.GkhGji.Migrations.Version_2014041001
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014041000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_DICT_TYPE_SURVEY_DOC",
                new RefColumn("TYPE_SURVEY_GJI_ID", ColumnProperty.NotNull, "GJI_TYPESURVEYDOC_TS", "GJI_DICT_TYPESURVEY", "ID"),
                new RefColumn("PROVIDED_DOC_ID", ColumnProperty.NotNull, "GJI_TYPESURVEYDOC_DOC", "GJI_DICT_PROVIDEDDOCUMENT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_TYPE_SURVEY_DOC");
        }
    }
}