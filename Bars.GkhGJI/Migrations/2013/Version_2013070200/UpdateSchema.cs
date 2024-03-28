namespace Bars.GkhGji.Migrations.Version_2013070200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013070200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013062801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveConstraint("GJI_STATEMENT_ANSWER", "FK_GJI_STAT_ANSW_INSN");
            Database.RemoveConstraint("GJI_STATEMENT_ANSWER", "FK_GJI_STAT_ANSW_CON");
            Database.RemoveConstraint("GJI_STATEMENT_ANSWER", "FK_GJI_STAT_ANSW_REV");
            Database.RemoveConstraint("GJI_STATEMENT_ANSWER", "FK_GJI_STAT_ANSW_INSR");
            Database.RemoveConstraint("GJI_STATEMENT_ANSWER", "FK_GJI_STAT_ANSW_FILE");

            Database.RemoveTable("GJI_STATEMENT_ANSWER");


            Database.RemoveConstraint("GJI_STATEMENT_REQUEST", "FK_GJI_STAT_REQ_INS");
            Database.RemoveConstraint("GJI_STATEMENT_REQUEST", "FK_GJI_STAT_REQ_CORG");
            Database.RemoveConstraint("GJI_STATEMENT_REQUEST", "FK_GJI_STAT_REQ_FILE");
            Database.RemoveTable("GJI_STATEMENT_REQUEST");


            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_ZON");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_EXE");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_TES");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_SURR");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_SUR");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_REF");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_RES");

            Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "REVENUE_DATE");
            Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "REVENUE_SOURCE_ID");
            Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "REVENUE_FORM_ID");
            Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "SURETY_ID");
            Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "SURETY_RESOLVE_ID");
            Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "SURETY_DATE");
            Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "EXECUTANT_ID");
            Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "TESTER_ID");
            Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "EXECUTE_DATE");
            Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "ZONAINSP_ID");
        }

        public override void Down()
        {
            Database.AddColumn("GJI_INSPECTION_STATEMENT", new Column("REVENUE_DATE", DbType.DateTime));

            Database.AddColumn("GJI_INSPECTION_STATEMENT", new Column("REVENUE_SOURCE_ID", DbType.Int64, 22));
            Database.AddColumn("GJI_INSPECTION_STATEMENT", new Column("REVENUE_FORM_ID", DbType.Int64, 22));
            Database.AddColumn("GJI_INSPECTION_STATEMENT", new Column("SURETY_ID", DbType.Int64, 22));
            Database.AddColumn("GJI_INSPECTION_STATEMENT", new Column("SURETY_RESOLVE_ID", DbType.Int64, 22));
            Database.AddColumn("GJI_INSPECTION_STATEMENT", new Column("SURETY_DATE", DbType.DateTime));
            Database.AddColumn("GJI_INSPECTION_STATEMENT", new Column("EXECUTANT_ID", DbType.Int64, 22));
            Database.AddColumn("GJI_INSPECTION_STATEMENT", new Column("TESTER_ID", DbType.Int64, 22));
            Database.AddColumn("GJI_INSPECTION_STATEMENT", new Column("EXECUTE_DATE", DbType.DateTime));
            Database.AddColumn("GJI_INSPECTION_STATEMENT", new Column("ZONAINSP_ID", DbType.Int64, 22));
               
            Database.AddIndex("IND_GJI_INSPECT_STAT_RES", false, "GJI_INSPECTION_STATEMENT", "REVENUE_SOURCE_ID");
            Database.AddIndex("IND_GJI_INSPECT_STAT_REF", false, "GJI_INSPECTION_STATEMENT", "REVENUE_FORM_ID");
            Database.AddIndex("IND_GJI_INSPECT_STAT_SUR", false, "GJI_INSPECTION_STATEMENT", "SURETY_ID");
            Database.AddIndex("IND_GJI_INSPECT_STAT_SURR", false, "GJI_INSPECTION_STATEMENT", "SURETY_RESOLVE_ID");
            Database.AddIndex("IND_GJI_INSPECT_STAT_EXE", false, "GJI_INSPECTION_STATEMENT", "EXECUTANT_ID");
            Database.AddIndex("IND_GJI_INSPECT_STAT_TES", false, "GJI_INSPECTION_STATEMENT", "TESTER_ID");
            Database.AddIndex("IND_GJI_INSPECT_STAT_ZON", false, "GJI_INSPECTION_STATEMENT", "ZONAINSP_ID");

            Database.AddForeignKey("FK_GJI_INSPECT_STAT_ZON", "GJI_INSPECTION_STATEMENT", "ZONAINSP_ID", "GKH_DICT_ZONAINSP", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_EXE", "GJI_INSPECTION_STATEMENT", "EXECUTANT_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_TES", "GJI_INSPECTION_STATEMENT", "TESTER_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_SURR", "GJI_INSPECTION_STATEMENT", "SURETY_RESOLVE_ID", "GJI_DICT_RESOLVE", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_SUR", "GJI_INSPECTION_STATEMENT", "SURETY_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_REF", "GJI_INSPECTION_STATEMENT", "REVENUE_FORM_ID", "GJI_DICT_REVENUEFORM", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_RES", "GJI_INSPECTION_STATEMENT", "REVENUE_SOURCE_ID", "GJI_DICT_REVENUESOURCE", "ID");



            Database.AddEntityTable(
               "GJI_STATEMENT_ANSWER",
               new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
               new Column("ANSWER_CONTENT_ID", DbType.Int64, 22), // Содержание ответа
               new Column("REVENUE_SOURCE_ID", DbType.Int64, 22), // Адресат
               new Column("INSPECTOR_ID", DbType.Int64, 22), // Исполнитель
               new Column("DOCUMENT_NAME", DbType.String, 300),
               new Column("DOCUMENT_NUM", DbType.String, 300),
               new Column("DOCUMENT_DATE", DbType.Date),
               new Column("DESCRIPTION", DbType.String, 500),
               new Column("FILE_INFO_ID", DbType.Int64, 22),
               new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_STAT_ANSW_INSN", false, "GJI_STATEMENT_ANSWER", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_STAT_ANSW_CON", false, "GJI_STATEMENT_ANSWER", "ANSWER_CONTENT_ID");
            Database.AddIndex("IND_GJI_STAT_ANSW_REV", false, "GJI_STATEMENT_ANSWER", "REVENUE_SOURCE_ID");
            Database.AddIndex("IND_GJI_STAT_ANSW_INSR", false, "GJI_STATEMENT_ANSWER", "INSPECTOR_ID");
            Database.AddIndex("IND_GJI_STAT_ANSW_FILE", false, "GJI_STATEMENT_ANSWER", "FILE_INFO_ID");
            Database.AddForeignKey("FK_GJI_STAT_ANSW_INSN", "GJI_STATEMENT_ANSWER", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_STAT_ANSW_CON", "GJI_STATEMENT_ANSWER", "ANSWER_CONTENT_ID", "GJI_DICT_ANSWER_CONTENT", "ID");
            Database.AddForeignKey("FK_GJI_STAT_ANSW_REV", "GJI_STATEMENT_ANSWER", "REVENUE_SOURCE_ID", "GJI_DICT_REVENUESOURCE", "ID");
            Database.AddForeignKey("FK_GJI_STAT_ANSW_INSR", "GJI_STATEMENT_ANSWER", "INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_STAT_ANSW_FILE", "GJI_STATEMENT_ANSWER", "FILE_INFO_ID", "B4_FILE_INFO", "ID");

            Database.AddEntityTable(
               "GJI_STATEMENT_REQUEST",
               new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
               new Column("COMPETENT_ORG_ID", DbType.Int64, 22),
               new Column("DOCUMENT_NAME", DbType.String, 300),
               new Column("DOCUMENT_NUM", DbType.String, 300),
               new Column("DOCUMENT_DATE", DbType.Date),
               new Column("DESCRIPTION", DbType.String, 2000),
               new Column("PERFORMANCE_DATE", DbType.Date),
               new Column("PERFORMANCE_FACT_DATE", DbType.Date),
               new Column("FILE_INFO_ID", DbType.Int64, 22),
               new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_STAT_REQ_INS", false, "GJI_STATEMENT_REQUEST", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_STAT_REQ_CORG", false, "GJI_STATEMENT_REQUEST", "COMPETENT_ORG_ID");
            Database.AddIndex("IND_GJI_STAT_REQ_FILE", false, "GJI_STATEMENT_REQUEST", "FILE_INFO_ID");

            Database.AddForeignKey("FK_GJI_STAT_REQ_INS", "GJI_STATEMENT_REQUEST", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_STAT_REQ_CORG", "GJI_STATEMENT_REQUEST", "COMPETENT_ORG_ID", "GJI_DICT_COMPETENT_ORG", "ID");
            Database.AddForeignKey("FK_GJI_STAT_REQ_FILE", "GJI_STATEMENT_REQUEST", "FILE_INFO_ID", "B4_FILE_INFO", "ID");
        }
    }
}