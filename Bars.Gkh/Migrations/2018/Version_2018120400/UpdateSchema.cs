using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.Migrations._2018.Version_2018120400
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018120400")]
    [MigrationDependsOn(typeof(Version_2018120300.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
       
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_QEXAM_QUESTION",
                new Column("NUMBER", DbType.String, ColumnProperty.NotNull),
                new Column("QUESTION_TEXT", DbType.String, ColumnProperty.Null),
                new RefColumn("QUESTION_ID", "FK_EXAMQUEST_QTEST_QUESTION", "GKH_DICT_QTEST_QUESTIONS", "ID"),
                new RefColumn("ANSWER_ID", "FK_EXAMQUEST_QTEST_QUANSWERS", "GKH_DICT_QTEST_QUANSWERS", "ID"),
                new RefColumn("REQUEST_ID", "FK_EXAMQUEST_PERSON_REQUEST_EXAM", "GKH_PERSON_REQUEST_EXAM", "ID"));

            this.Database.AddEntityTable("GKH_QEXAM_ANSWER",
             
              new Column("ANSWER_TEXT", DbType.String, ColumnProperty.NotNull),
              new RefColumn("ANSWER_ID", "FK_QEXAM_ANSWER_QTEST_QUANSWERS", "GKH_DICT_QTEST_QUANSWERS", "ID"),
              new RefColumn("QUESTION_ID", "FK_QEXAM_ANSWER_QEXAM_QUESTION", "GKH_QEXAM_QUESTION", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_QEXAM_ANSWER");
            this.Database.RemoveTable("GKH_QEXAM_QUESTION");
        }
    }
}