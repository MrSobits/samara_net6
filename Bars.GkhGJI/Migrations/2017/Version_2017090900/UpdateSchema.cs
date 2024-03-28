namespace Bars.GkhGji.Migrations._2017.Version_2017090900
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    using Bars.Gkh.Utils;

    [Migration("2017090900")]
    [MigrationDependsOn(typeof(Version_2017050300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017090400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddIndex(
                "IND_GJI_APPEAL_DECLARANT_CATEGORY_PAIR",
                true,
                "GJI_APPEAL_DECLARANT_CATEGORY",
                "GJI_APPEAL_CITIZENS_ID",
                "GJI_DICT_APPLICANT_CATEGORY_ID");

            this.Database.AddEntityTable("GJI_APPEAL_CITIZEN_ANSWER_FILES",
                new Column("NAME", DbType.String, 100, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new RefColumn("ANSWER_ID", "APPCIT_ANSWER_FILE_ANSWER_ID", "GJI_APPCIT_ANSWER", "ID"),
                new FileColumn("FILE_INFO_ID", "APPCIT_ANSWER_FILE_FILEINFO_ID"));

            this.Database.AddEntityTable("GJI_APPCIT_HEADINSP",
                new RefColumn("APPCIT_ID", "APPCIT_HEADINSP_APPCIT_ID", "GJI_APPEAL_CITIZENS", "ID"),
                new RefColumn("INSPECTOR_ID", "APPCIT_HEADINSP_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));

            this.Database.AddIndex("IND_GJI_APPCIT_HEADINSP_PAIR", true, "GJI_APPCIT_HEADINSP", "APPCIT_ID", "INSPECTOR_ID");

            this.Database.AddEntityTable("GJI_DICT_FACT_CHECKING_TYPE",
                new Column("CODE", DbType.String, 3, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("NAME", DbType.String, 100, ColumnProperty.NotNull | ColumnProperty.Unique));

            this.FillData("GJI_DICT_FACT_CHECKING_TYPE", new Dictionary<string, string>
            {
                { "1", "Документальная проверка" },
                { "2", "Коллегиальная проверка" },
                { "3", "Коллегиальная проверка с выездом на место" },
                { "4", "Коллегиальная проверка с выездом на место с участием заявителя" },
                { "5", "Проверка с выездом на место" },
                { "6", "Проверка с выездом на место с участием заявителя" },
                { "7", "Проверка с участием заявителя" }
                
            });

            this.Database.AddEntityTable("GJI_DICT_CONCEDERATION_RESULT",
                new Column("CODE", DbType.String, 3, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("NAME", DbType.String, 100, ColumnProperty.NotNull | ColumnProperty.Unique));

            this.FillData("GJI_DICT_CONCEDERATION_RESULT", new Dictionary<string, string>
            {
                { "1", "Поддержано" },
                { "2", "Разъяснено" },
                { "3", "Не поддержано" },
                { "4", "Дан ответ" },
                { "5", "Оставлено без ответа" },
                { "6", "Направлено по компетенции" },
                { "7", "Поддержано, в том числе меры приняты" },
                { "8", "Срок продлен" }
            });

            this.Database.AddEntityTable("GJI_APPCIT_ANSWER_STATSUBJ",
                new RefColumn("ANSWER_ID", "APPCIT_ANSWER_STATSUBJ_ANSWER_ID", "GJI_APPCIT_ANSWER", "ID"),
                new RefColumn("APPCIT_STAT_SUBJ_ID", "APPCIT_ANSWER_APPCIT_STAT_SUBJ_ID", "GJI_APPCIT_STATSUBJ", "ID"));

            this.Database.AddIndex("IND_GJI_APPCIT_ANSWER_STATSUBJ_PAIR", true, "GJI_APPCIT_ANSWER_STATSUBJ", "ANSWER_ID", "APPCIT_STAT_SUBJ_ID");

            this.Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("EXEC_TAKE_DATE", DbType.DateTime));

            this.Database.AddColumn("GJI_APPCIT_ANSWER", new Column("EXEC_DATE", DbType.DateTime));
            this.Database.AddColumn("GJI_APPCIT_ANSWER", new Column("EXTEND_DATE", DbType.DateTime));
            this.Database.AddRefColumn("GJI_APPCIT_ANSWER", new RefColumn("CONCED_RESULT_ID", "APPCIT_ANSWER_CONCEDRESULT_ID", "GJI_DICT_CONCEDERATION_RESULT", "ID"));
            this.Database.AddRefColumn("GJI_APPCIT_ANSWER", new RefColumn("FACT_CHECK_TYPE_ID", "APPCIT_ANSWER_FACTCHECKTYPE_ID", "GJI_DICT_FACT_CHECKING_TYPE", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_APPEAL_CITIZENS", "EXEC_TAKE_DATE");
            this.Database.RemoveColumn("GJI_APPCIT_ANSWER", "EXEC_DATE");
            this.Database.RemoveColumn("GJI_APPCIT_ANSWER", "EXTEND_DATE");
            this.Database.RemoveColumn("GJI_APPCIT_ANSWER", "CONCED_RESULT_ID");
            this.Database.RemoveColumn("GJI_APPCIT_ANSWER", "FACT_CHECK_TYPE_ID");

            this.Database.RemoveTable("GJI_APPCIT_ANSWER_STATSUBJ");
            this.Database.RemoveTable("GJI_DICT_CONCEDERATION_RESULT");
            this.Database.RemoveTable("GJI_DICT_FACT_CHECKING_TYPE");
            this.Database.RemoveTable("GJI_APPCIT_HEADINSP");
            this.Database.RemoveTable("GJI_APPEAL_CITIZEN_ANSWER_FILES");

        }

        private void FillData(string tableName, IDictionary<string, string> codeNameDictionary)
        {
            var values = codeNameDictionary.Select(x => $"(0, now(), now(), '{x.Key}', '{x.Value}')");
            this.Database.ExecuteNonQuery(
                $@"INSERT INTO {tableName} 
                    (OBJECT_VERSION, OBJECT_CREATE_DATE, OBJECT_EDIT_DATE, CODE, NAME) 
                    VALUES {values.AggregateWithSeparator(",")}");
        }
    }
}
