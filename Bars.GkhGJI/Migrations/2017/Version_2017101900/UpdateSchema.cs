using System.Data;
using Bars.B4.Modules.Ecm7.Framework;

namespace Bars.GkhGji.Migrations._2017.Version_2017101900
{
    [Migration("2017101900")]
    [MigrationDependsOn(typeof(Version_2017101800.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            var gjiAppealCitizens = new SchemaQualifiedObjectName {Schema = "public", Name = "gji_appeal_citizens"};
            var gjiAppcitAnswer = new SchemaQualifiedObjectName {Schema = "public", Name = "gji_appcit_answer"};
            var gjiDictStatementSubj = new SchemaQualifiedObjectName {Schema = "public", Name = "gji_dict_statement_subj"};
            var gjiDictStatSubSubject = new SchemaQualifiedObjectName {Schema = "public", Name = "gji_dict_stat_sub_subject"};
            var gjiDictFeatureviol = new SchemaQualifiedObjectName {Schema = "public", Name = "gji_dict_featureviol"};
            
            this.Database.AddColumn(gjiAppealCitizens, new Column("is_request_work_send", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddColumn(gjiAppealCitizens, new Column("is_accepted_work_send", DbType.Boolean, ColumnProperty.NotNull, false));

            this.Database.AddColumn(gjiAppcitAnswer, new Column("answer_uid", DbType.String.WithSize(100)));
            this.Database.AddColumn(gjiAppcitAnswer, new Column("is_uploaded", DbType.Boolean));
            this.Database.AddColumn(gjiAppcitAnswer, new Column("additional_info", DbType.String.WithSize(2000)));

            this.Database.AddColumn(gjiDictStatementSubj, new Column("question_code", DbType.String.WithSize(5)));
            this.Database.AddColumn(gjiDictStatSubSubject, new Column("question_code", DbType.String.WithSize(5)));
            this.Database.AddColumn(gjiDictFeatureviol, new Column("question_code", DbType.String.WithSize(5)));
        }
    }
}
