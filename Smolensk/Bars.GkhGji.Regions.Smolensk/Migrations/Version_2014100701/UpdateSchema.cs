namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014100701
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014100700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_DICT_SURVEY_SUBJ", new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300));

            Database.AddEntityTable(
                "GJI_DISP_SURVSUBJ",
                new RefColumn("SURVEY_SUBJ_ID", ColumnProperty.Null, "DISP_SURV_SUBJ", "GJI_DICT_SURVEY_SUBJ", "ID"),
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "DISP_SURVSUBJ", "GJI_DISPOSAL", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DISP_SURVSUBJ");

            Database.RemoveTable("GJI_DICT_SURVEY_SUBJ");
        }
    }
}