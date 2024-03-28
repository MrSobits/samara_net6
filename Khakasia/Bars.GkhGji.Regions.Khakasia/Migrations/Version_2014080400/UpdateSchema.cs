namespace Bars.GkhGji.Regions.Khakasia.Migrations.Version_2014080400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Khakasia.Migrations.Version_2014073101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_DICT_SURVEY_SUBJ", new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_SURV_SUBJ_NAME", false, "GJI_DICT_SURVEY_SUBJ", "NAME");
            Database.AddIndex("IND_GJI_SURV_SUBJ_CODE", false, "GJI_DICT_SURVEY_SUBJ", "CODE");

            Database.RemoveColumn("GJI_DISP_SURVSUBJ", "TYPE_SURVEY_SUBJ");
            Database.AddColumn("GJI_DISP_SURVSUBJ", new RefColumn("SURVEY_SUBJ_ID", ColumnProperty.Null, "DISP_SURV_SUBJ", "GJI_DICT_SURVEY_SUBJ", "ID"));
        }

        public override void Down()
        {
            Database.FormatSql("UPDATE GJI_DISP_SURVSUBJ set SURVEY_SUBJ_ID = null");
            Database.RemoveColumn("GJI_DISP_SURVSUBJ", "SURVEY_SUBJ_ID");
            Database.AddColumn("GJI_DISP_SURVSUBJ", new Column("TYPE_SURVEY_SUBJ", DbType.Int32));
        }
    }
}