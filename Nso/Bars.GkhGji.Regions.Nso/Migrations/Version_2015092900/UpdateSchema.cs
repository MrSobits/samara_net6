﻿namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2015092900
{
	using global::Bars.B4.Modules.Ecm7.Framework;
	using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015092900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2015092100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
		public override void Up()
		{
			Database.AddEntityTable("GJI_NSO_DISPOSAL_SURVEY_PURP",
				new RefColumn("DISPOSAL_ID", "GJI_NSO_DISP_SURVEYP_D", "GJI_DISPOSAL", "ID"),
				new RefColumn("SURVEY_PURP_ID", "GJI_NSO_DISP_SURVEYPU", "GJI_DICT_SURVEY_PURP", "ID"));

			Database.AddEntityTable("GJI_NSO_DISPOSAL_SURVEY_OBJ",
				new RefColumn("DISPOSAL_ID", "GJI_NSO_DISP_SURVEY_OBJ_D", "GJI_DISPOSAL", "ID"),
				new RefColumn("SURVEY_OBJ_ID", "GJI_NSO_DISP_SURVEY_OBJ_O", "GJI_DICT_SURVEY_OBJ", "ID"));

			Database.AddEntityTable("GJI_NSO_DISPOSAL_INSPFOUND",
				new RefColumn("DISPOSAL_ID", "GJI_NSO_DISP_IFOUND_D", "GJI_DISPOSAL", "ID"),
				new RefColumn("INSPFOUND_ID", "GJI_NSO_DISP_IFOUND_F", "GKH_DICT_NORMATIVE_DOC", "ID"));

			Database.AddEntityTable("GJI_NSO_DISPOSAL_ADMREG",
				new RefColumn("DISPOSAL_ID", "GJI_NSO_DISP_ADMREG_D", "GJI_DISPOSAL", "ID"),
				new RefColumn("ADMREG_ID", "GJI_NSO_DISP_ADMREG_AR", "GKH_DICT_NORMATIVE_DOC", "ID"));
		}

		public override void Down()
		{
			Database.RemoveTable("GJI_NSO_DISPOSAL_SURVEY_PURP");
			Database.RemoveTable("GJI_NSO_DISPOSAL_SURVEY_OBJ");
			Database.RemoveTable("GJI_NSO_DISPOSAL_INSPFOUND");
			Database.RemoveTable("GJI_NSO_DISPOSAL_ADMREG");
		}
    }
}