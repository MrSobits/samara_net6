namespace Bars.GkhGji.Migrations._2017.Version_2017112500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017112500")]
    [MigrationDependsOn(typeof(Version_2017090900.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017110200.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017111400.UpdateSchema))]
    public class UpdateSchema : Migration
    {

        public override void Up()
        {
            this.Database.AddColumn("GJI_DICT_STATEMENT_SUBJ",
                new Column("QUESTION_CODE", DbType.String, 20));

            this.Database.AddColumn("GJI_APPEAL_CITIZEN_ANSWER_FILES",
                new Column("UNIQUE_NAME", DbType.String, 300));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_DICT_STATEMENT_SUBJ", "QUESTION_CODE");

            this.Database.RemoveColumn("GJI_APPEAL_CITIZEN_ANSWER_FILES", "UNIQUE_NAME");
        }
    }
}
