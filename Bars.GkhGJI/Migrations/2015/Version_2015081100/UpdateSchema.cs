namespace Bars.GkhGji.Migrations._2015.Version_2015081100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015081100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015081000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_LEGFOUND_INSPECT", "NORM_DOC_ID");
            Database.RemoveColumn("GJI_DICT_TASKS_INSPECTION", "SURVEY_OBJECTIVE_ID");
            Database.RemoveColumn("GJI_DICT_GOALS_INSPECTION", "SURVEY_PURPOSE_ID");
            Database.RemoveTable("GJI_DICT_ADM_REG");
        }

        public override void Up()
        {
            Database.ChangeColumn("GJI_DICT_SURVEY_PURP", new Column("NAME", DbType.String, 2000));
            Database.ChangeColumn("GJI_DICT_SURVEY_OBJ", new Column("NAME", DbType.String, 2000));

            Database.AddEntityTable(
                "GJI_DICT_ADM_REG",
                new RefColumn(
                    "TYPE_SURVEY_GJI_ID",
                    ColumnProperty.NotNull,
                    "TYPE_SUR_ADM_REG",
                    "GJI_DICT_TYPESURVEY",
                    "ID"),
                new RefColumn("NORM_DOC_ID", ColumnProperty.NotNull, "TYPE_SUR_NORM_DOC", "GKH_DICT_NORMATIVE_DOC", "ID"));

            Database.AddRefColumn(
                "GJI_DICT_GOALS_INSPECTION",
                new RefColumn(
                    "SURVEY_PURPOSE_ID",
                    ColumnProperty.Null,
                    "TYP_SUR_PURP",
                    "GJI_DICT_SURVEY_PURP",
                    "ID"));

            Database.AddRefColumn(
                "GJI_DICT_TASKS_INSPECTION",
                new RefColumn(
                    "SURVEY_OBJECTIVE_ID",
                    ColumnProperty.Null,
                    "TYP_SUR_OBJ",
                    "GJI_DICT_SURVEY_OBJ",
                    "ID"));

            Database.AddRefColumn(
                "GJI_DICT_LEGFOUND_INSPECT",
                new RefColumn(
                    "NORM_DOC_ID",
                    ColumnProperty.Null,
                    "TYPE_SUR_LEGF_NORM_DOC",
                    "GKH_DICT_NORMATIVE_DOC",
                    "ID"));
        }
    }
}