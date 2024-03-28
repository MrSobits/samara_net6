namespace Bars.GkhGji.Migrations.Version_2013061100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013061100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013053000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Проверка ГЖИ по обращениям граждан - Ответ
            Database.AddEntityTable(
                "GJI_APPCIT_ANSWER",
                new RefColumn("APPCIT_ID", "GJI_ANSW_APPCIT", "GJI_APPEAL_CITIZENS", "ID"),
                new RefColumn("ANSWER_CONTENT_ID", "GJI_ANSW_CONT", "GJI_DICT_ANSWER_CONTENT", "ID"), // Содержание ответа
                new RefColumn("REVENUE_SOURCE_ID", "GJI_ANSW_REV", "GJI_DICT_REVENUESOURCE", "ID"), // Адресат
                new RefColumn("INSPECTOR_ID", "GJI_ANSW_INSP", "GKH_DICT_INSPECTOR", "ID"), // Исполнитель
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 300),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("DESCRIPTION", DbType.String, 2000),
                new RefColumn("FILE_INFO_ID", "GJI_ANSW_FI", "B4_FILE_INFO", "ID"),
                new Column("EXTERNAL_ID", DbType.String, 36));

            Database.AddEntityTable(
                "GJI_APPCIT_REQUEST",
                new RefColumn("APPCIT_ID", "GJI_REQ_APPCIT", "GJI_APPEAL_CITIZENS", "ID"),
                new Column("COMPETENT_ORG_ID", DbType.Int64, 22),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 300),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("DESCRIPTION", DbType.String, 2000),
                new Column("PERFORMANCE_DATE", DbType.Date),
                new Column("PERFORMANCE_FACT_DATE", DbType.Date),
                new RefColumn("FILE_INFO_ID", "GJI_REQUEST_FI", "B4_FILE_INFO", "ID"),
                new Column("EXTERNAL_ID", DbType.String, 36));

            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("SURETY_DATE", DbType.DateTime));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("EXECUTE_DATE", DbType.DateTime));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("SURETY_ID", DbType.Int64, 22));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("SURETY_RESOLVE_ID", DbType.Int64, 22));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("EXECUTANT_ID", DbType.Int64, 22));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("TESTER_ID", DbType.Int64, 22));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("ZONAINSP_ID", DbType.Int64, 22));

            Database.AddIndex("IND_GJI_APPCIT_REQ_CORG", false, "GJI_APPCIT_REQUEST", "COMPETENT_ORG_ID");
            Database.AddIndex("IND_GJI_APPCIT_SUR", false, "GJI_APPEAL_CITIZENS", "SURETY_ID");
            Database.AddIndex("IND_GJI_APPCIT_SURR", false, "GJI_APPEAL_CITIZENS", "SURETY_RESOLVE_ID");
            Database.AddIndex("IND_GJI_APPCIT_EXE", false, "GJI_APPEAL_CITIZENS", "EXECUTANT_ID");
            Database.AddIndex("IND_GJI_APPCIT_TES", false, "GJI_APPEAL_CITIZENS", "TESTER_ID");
            Database.AddIndex("IND_GJI_APPCIT_ZON", false, "GJI_APPEAL_CITIZENS", "ZONAINSP_ID");

            Database.AddForeignKey("FK_GJI_APPCIT_REQ_CORG", "GJI_APPCIT_REQUEST", "COMPETENT_ORG_ID", "GJI_DICT_COMPETENT_ORG", "ID");
            Database.AddForeignKey("FK_GJI_APPCIT_SUR", "GJI_APPEAL_CITIZENS", "SURETY_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_APPCIT_SURR", "GJI_APPEAL_CITIZENS", "SURETY_RESOLVE_ID", "GJI_DICT_RESOLVE", "ID");
            Database.AddForeignKey("FK_GJI_APPCIT_EXE", "GJI_APPEAL_CITIZENS", "EXECUTANT_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_APPCIT_TES", "GJI_APPEAL_CITIZENS", "TESTER_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_APPCIT_ZON", "GJI_APPEAL_CITIZENS", "ZONAINSP_ID", "GKH_DICT_ZONAINSP", "ID");

            Database.AddColumn("GJI_INSPECTION", new Column("PERSON_INSPECTION", DbType.Int32, 4, ColumnProperty.NotNull, 20));
            Database.ExecuteNonQuery(@"
update gji_inspection ii 
set person_inspection = (
    select 
        ss.person_inspection 
    from gji_inspection_statement ss 
    where ii.id = ss.id
) 
where id in (select id from gji_inspection_statement)");
            Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "PERSON_INSPECTION");
        }

        public override void Down()
        {
            Database.AddColumn("GJI_INSPECTION_STATEMENT", new Column("PERSON_INSPECTION", DbType.Int32, 4, ColumnProperty.NotNull, 20));
            Database.ExecuteNonQuery(@"
                update gji_inspection_statement ii 
                set person_inspection = (
                    select 
                        ss.person_inspection 
                    from gji_inspection ss 
                    where ii.id = ss.id)");
            Database.RemoveColumn("GJI_INSPECTION", "PERSON_INSPECTION");

            Database.RemoveTable("GJI_APPCIT_ANSWER");
            Database.RemoveTable("GJI_APPCIT_REQUEST");

            Database.RemoveIndex("IND_GJI_APPCIT_SUR", "GJI_APPEAL_CITIZENS");
            Database.RemoveIndex("IND_GJI_APPCIT_SURR", "GJI_APPEAL_CITIZENS");
            Database.RemoveIndex("IND_GJI_APPCIT_EXE", "GJI_APPEAL_CITIZENS");
            Database.RemoveIndex("IND_GJI_APPCIT_TES", "GJI_APPEAL_CITIZENS");
            Database.RemoveIndex("IND_GJI_APPCIT_ZON", "GJI_APPEAL_CITIZENS");
            Database.RemoveIndex("IND_GJI_APPCIT_REQ_CORG", "GJI_APPCIT_REQUEST");

            Database.RemoveConstraint("GJI_APPEAL_CITIZENS", "FK_GJI_APPCIT_SUR");
            Database.RemoveConstraint("GJI_APPEAL_CITIZENS", "FK_GJI_APPCIT_SURR");
            Database.RemoveConstraint("GJI_APPEAL_CITIZENS", "FK_GJI_APPCIT_EXE");
            Database.RemoveConstraint("GJI_APPEAL_CITIZENS", "FK_GJI_APPCIT_TES");
            Database.RemoveConstraint("GJI_APPEAL_CITIZENS", "FK_GJI_APPCIT_SUR");
            Database.RemoveConstraint("GJI_APPCIT_REQUEST", "FK_GJI_APPCIT_REQ_CORG");

            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "SURETY_DATE");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "EXECUTE_DATE");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "SURETY_ID");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "SURETY_RESOLVE_ID");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "EXECUTANT_ID");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "TESTER_ID");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "ZONAINSP_ID");
        }
    }
}