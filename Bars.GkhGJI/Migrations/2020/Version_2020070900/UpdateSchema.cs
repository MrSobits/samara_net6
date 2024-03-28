namespace Bars.GkhGji.Migrations._2020.Version_2020070900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2020070900")]
    [MigrationDependsOn(typeof(Version_2020070800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_SURVEY_SUBJ", new Column("GIS_GKH_CODE", DbType.String));
            Database.AddColumn("GJI_DICT_SURVEY_SUBJ", new Column("GIS_GKH_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_SURVEY_SUBJ", "GIS_GKH_GUID");
            Database.RemoveColumn("GJI_DICT_SURVEY_SUBJ", "GIS_GKH_CODE");
        
        }
    }
}