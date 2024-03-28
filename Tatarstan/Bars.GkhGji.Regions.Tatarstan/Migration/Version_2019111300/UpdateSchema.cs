namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019111300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Migrations;

    [Migration("2019111300")]
    [MigrationDependsOn(typeof(Version_2019111200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string TypicalAnswersTableName = "GJI_DICT_CONTROL_LIST_TYPICAL_ANSWER";
        private const string TypicalQuestionsTableName = "GJI_DICT_CONTROL_LIST_TYPICAL_QUESTION";
        private const string ControlListTableName = "GJI_CONTROL_LIST";
        private const string ControlListQuestionTableName = "GJI_CONTROL_LIST_QUESTION";

        public override void Up()
        {
            //типовых ответы на вопросы проверочного листа
            this.Database.AddEntityTable(UpdateSchema.TypicalAnswersTableName, 
                new GuidColumn("TOR_ID", ColumnProperty.Null),
                new Column("ANSWER", DbType.String, 500, ColumnProperty.NotNull)
                );

            //типовые вопросы проверочного листа
            this.Database.AddEntityTable(UpdateSchema.TypicalQuestionsTableName,
                new Column("QUESTION", DbType.String, 500, ColumnProperty.NotNull),
                new RefColumn("NORMATIVE_DOC_ID", ColumnProperty.NotNull, "GJI_DICT_CONTROL_LIST_TYPICAL_QUESTIONS_NORMATIVE_DOC_ID", "GKH_DICT_NORMATIVE_DOC", "ID"),
                new RefColumn("MANDATORY_REQ_ID", ColumnProperty.Null, "GJI_DICT_CONTROL_LIST_TYPICAL_QUESTIONS_MANDATORY_REQ_ID", "GJI_DICT_MANDATORY_REQS", "ID")
                );

            //проверочные листы
            this.Database.AddEntityTable(UpdateSchema.ControlListTableName,
                new GuidColumn("TOR_ID", ColumnProperty.Null),
                new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("END_DATE", DbType.DateTime, ColumnProperty.Null),
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "GJI_CONTROL_LIST_TAT_DISPOSAL_ID", "GJI_TAT_DISPOSAL", "ID"),
                new RefColumn("FILE_ID", ColumnProperty.Null, "GJI_CONTROL_LIST_FILE_ID", "B4_FILE_INFO", "ID"));

            //Вопросы проверочных листов
            this.Database.AddEntityTable(UpdateSchema.ControlListQuestionTableName,
                new Column("QUESTION_CONTENT", DbType.String, 500, ColumnProperty.NotNull),
                new RefColumn("TYPICAL_QUESTION_ID", ColumnProperty.Null, "GJI_CONTROL_LIST_QUESTION_TYPICAL_QUESTION_ID", UpdateSchema.TypicalQuestionsTableName, "ID"),
                new RefColumn("TYPICAL_ANSWER_ID", ColumnProperty.Null, "GJI_CONTROL_LIST_QUESTION_TYPICAL_ANSWER_ID", UpdateSchema.TypicalAnswersTableName, "ID"),
                new RefColumn("CONTROL_LIST_ID", ColumnProperty.NotNull, "GJI_CONTROL_LIST_QUESTION_CONTROL_LIST_ID", UpdateSchema.ControlListTableName, "ID")
                );
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.ControlListQuestionTableName);
            this.Database.RemoveTable(UpdateSchema.ControlListTableName);
            this.Database.RemoveTable(UpdateSchema.TypicalAnswersTableName);
            this.Database.RemoveTable(UpdateSchema.TypicalQuestionsTableName);
        }
    }
}
