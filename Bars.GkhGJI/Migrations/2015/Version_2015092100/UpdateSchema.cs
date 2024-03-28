namespace Bars.GkhGji.Migrations._2015.Version_2015092100
{
	using System.Data;
	using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015092100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2015091000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			Database.AddColumn("GJI_DICT_SURVEY_SUBJ", "INSP_PLAN_FORMULATION", DbType.String, 500, ColumnProperty.Null);
        }

        public override void Down()
        {
			Database.RemoveColumn("GJI_DICT_SURVEY_SUBJ", "INSP_PLAN_FORMULATION");
        }
    }
}