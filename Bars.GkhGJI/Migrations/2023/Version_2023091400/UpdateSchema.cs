namespace Bars.GkhGji.Migrations._2023.Version_2023091400
{
    using B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023091400")]
    [MigrationDependsOn(typeof(_2023.Version_2023090400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_STATEMENT_SUBJ", new Column("TRACK_APPEAL_CITS", DbType.Boolean, false));
            Database.AddColumn("GJI_DICT_STAT_SUB_SUBJECT", new Column("TRACK_APPEAL_CITS", DbType.Boolean, false));
            Database.AddColumn("GJI_DICT_FEATUREVIOL", new Column("TRACK_APPEAL_CITS", DbType.Boolean, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_STATEMENT_SUBJ", "TRACK_APPEAL_CITS");
            Database.RemoveColumn("GJI_DICT_STAT_SUB_SUBJECT", "TRACK_APPEAL_CITS");
            Database.RemoveColumn("GJI_DICT_FEATUREVIOL", "TRACK_APPEAL_CITS");
        }
    }
}