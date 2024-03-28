namespace Bars.GkhGji.Regions.Zabaykalye.Migrations.Version_2014071801
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Zabaykalye.Migrations.Version_2014071800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
           
            Database.AddEntityTable(
                "GJI_ACTSURVEY_CONCLUSION",
                new RefColumn("ACT_SURVEY_ID", ColumnProperty.NotNull, "GJI_GJI_ACTSURVEY_CON_A", "GJI_ACTSURVEY", "ID"),
                new RefColumn("FILE_ID", ColumnProperty.Null, "GJI_GJI_ACTSURVEY_CON_F", "B4_FILE_INFO", "ID"),
                new RefColumn("OFFICIAL_ID", ColumnProperty.Null, "GJI_GJI_ACTSURVEY_CON_F", "GKH_DICT_INSPECTOR", "ID"),
                new Column("DOC_NUMBER", DbType.String, 50),
                new Column("DOC_DATE", DbType.Date),
                new Column("DESCRIPTION", DbType.String, 2000));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_ACTSURVEY_CONCLUSION");
        }
    }
}