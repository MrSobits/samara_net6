namespace Bars.GkhGji.Migrations._2020.Version_2020101900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020101900")]
    [MigrationDependsOn(typeof(Version_2020100700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_NSO_DISPOSAL_SURVEY_OBJ", new Column("DESCRIPTION", DbType.String, 1500));
            this.Database.ExecuteNonQuery(@"update GJI_NSO_DISPOSAL_SURVEY_OBJ so set DESCRIPTION = dso.name from GJI_DICT_SURVEY_OBJ dso where so.SURVEY_OBJ_ID = dso.id");

            Database.AddColumn("GJI_NSO_DISPOSAL_SURVEY_PURP", new Column("DESCRIPTION", DbType.String, 1500));
            this.Database.ExecuteNonQuery(@"update GJI_NSO_DISPOSAL_SURVEY_PURP sp set DESCRIPTION = dsp.name from GJI_DICT_SURVEY_PURP dsp where sp.SURVEY_PURP_ID = dsp.id");
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_NSO_DISPOSAL_SURVEY_PURP", "DESCRIPTION");
            Database.RemoveColumn("GJI_NSO_DISPOSAL_SURVEY_OBJ", "DESCRIPTION");
        }
    }
}