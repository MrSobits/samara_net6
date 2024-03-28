namespace Bars.Gkh.Migrations.Version_2013102300
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013102200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CIT_SUG_FILES", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GKH_CIT_SUG", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GKH_DICT_PROBLEM_PLACE", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GKH_SUG_RUBRIC", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GKH_CIT_SUG_COMMENT_FILES", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GKH_CIT_SUG_COMMENT", new Column("EXTERNAL_ID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CIT_SUG_FILES", "EXTERNAL_ID");
            Database.RemoveColumn("GKH_CIT_SUG", "EXTERNAL_ID");
            Database.RemoveColumn("GKH_DICT_PROBLEM_PLACE", "EXTERNAL_ID");
            Database.RemoveColumn("GKH_SUG_RUBRIC", "EXTERNAL_ID");
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT_FILES", "EXTERNAL_ID");
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT", "EXTERNAL_ID");
        }
    }
}