using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.Migrations._2018.Version_2018120600
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018120600")]
    [MigrationDependsOn(typeof(Version_2018120400.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
       
        public override void Up()
        {
            this.Database.ChangeColumn("GKH_DICT_QTEST_QUANSWERS",
                new Column("ANSWER", DbType.String, 5000));

            this.Database.ChangeColumn("GKH_DICT_QTEST_QUESTIONS",
                new Column("QUESTION_TEXT", DbType.String, 5000));

            this.Database.ChangeColumn("GKH_QEXAM_QUESTION",
                new Column("QUESTION_TEXT", DbType.String, 5000));

            this.Database.ChangeColumn("GKH_QEXAM_ANSWER",
                new Column("ANSWER_TEXT", DbType.String, 5000));
        }

        public override void Down()
        {
           
        }
    }
}