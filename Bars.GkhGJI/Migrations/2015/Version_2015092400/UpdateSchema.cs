namespace Bars.GkhGji.Migrations._2015.Version_2015092400
{
	using System.Data;
	using global::Bars.B4.Modules.Ecm7.Framework;
	using Bars.GkhGji.Enums;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015092400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015092102.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        Database.AddColumn("GJI_DICT_SURVEY_SUBJ", "RELEVANCE", DbType.Int32, 4, ColumnProperty.NotNull, (int) SurveySubjectRelevance.NotActual);
        }

        public override void Down()
        {
			Database.RemoveColumn("GJI_DICT_SURVEY_SUBJ", "RELEVANCE");
        }
    }
}