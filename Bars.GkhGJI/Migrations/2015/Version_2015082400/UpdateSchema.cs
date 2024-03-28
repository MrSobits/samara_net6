namespace Bars.GkhGji.Migrations._2015.Version_2015082400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015082400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015081400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Down()
        {
            Database.RemoveTable("GJI_SURV_PLAN_CONTR_ATT");
            Database.RemoveTable("GJI_SURV_PLAN_CONTR");
            Database.RemoveTable("GJI_SURV_PLAN_CAND");
            Database.RemoveTable("GJI_SURVEY_PLAN");
        }

        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_SURVEY_PLAN",
                new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                new RefColumn(
                    "PLAN_JUR_PESON_ID",
                    ColumnProperty.NotNull,
                    "SURPL_PLAJURPERS",
                    "GJI_DICT_PLANJURPERSON",
                    "ID"),
                new RefColumn("STATE_ID", ColumnProperty.NotNull, "SURPL_STAT", "B4_STATE", "ID"));

            Database.AddEntityTable(
                "GJI_SURV_PLAN_CAND",
                new Column("PLAN_MONTH", DbType.Int16, ColumnProperty.NotNull),
                new Column("PLAN_YEAR", DbType.Int32, ColumnProperty.NotNull),
                new Column("REASON", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("FROM_LAST_AUDIT", DbType.Boolean, ColumnProperty.NotNull, false),
                new RefColumn(
                    "AUDIT_PURPOSE_ID",
                    ColumnProperty.NotNull,
                    "SUPLCAND_AUDPURP",
                    "GJI_DICT_AUDIT_PURPOSE",
                    "ID"),
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "SUPLCAND_CONTR", "GKH_CONTRAGENT", "ID"));

            Database.AddEntityTable(
                "GJI_SURV_PLAN_CONTR",
                new Column("PLAN_MONTH", DbType.Int16, ColumnProperty.NotNull),
                new Column("PLAN_YEAR", DbType.Int32, ColumnProperty.NotNull),
                new Column("REASON", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("IS_EXCLUDED", DbType.Boolean, ColumnProperty.NotNull),
                new Column("EXCLUSION_REASON", DbType.String, 2000, ColumnProperty.Null),
                new Column("FROM_LAST_AUDIT", DbType.Boolean, ColumnProperty.NotNull, false),
                new RefColumn(
                    "AUDIT_PURPOSE_ID",
                    ColumnProperty.NotNull,
                    "SUPLCONTR_AUDPURP",
                    "GJI_DICT_AUDIT_PURPOSE",
                    "ID"),
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "SUPLCONTR_CONTR", "GKH_CONTRAGENT", "ID"),
                new RefColumn("SURVEY_PLAN_ID", ColumnProperty.NotNull, "SURPLCONTR_SURPL", "GJI_SURVEY_PLAN", "ID"),
                new RefColumn(
                    "INSPECTION_ID",
                    ColumnProperty.Null,
                    "SURPLCONTR_INSP",
                    "GJI_INSPECTION_JURPERSON",
                    "ID"));

            Database.AddEntityTable(
                "GJI_SURV_PLAN_CONTR_ATT",
                new Column("NAME", DbType.String, 250, ColumnProperty.NotNull),
                new Column("NUM", DbType.String, 250, ColumnProperty.NotNull),
                new Column("DOC_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 2000, ColumnProperty.Null),
                new RefColumn("FILE_ID", ColumnProperty.NotNull, "SURPLCOATT_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn(
                    "SURV_PLAN_CONTR_ID",
                    ColumnProperty.NotNull,
                    "SURPLCOATT_SURPLCO",
                    "GJI_SURV_PLAN_CONTR",
                    "ID"));
        }
    }
}