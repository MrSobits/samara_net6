namespace Bars.Gkh.Migrations._2015.Version_2015112400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015112400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015111700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CIT_SUG_COMMENT", new Column("IS_FIRST", DbType.Boolean, false));
            Database.AddColumn("GKH_CIT_SUG_COMMENT", new Column("DESCRIPTION", DbType.String, 1000));
            Database.AddRefColumn("GKH_CIT_SUG_COMMENT", new RefColumn("PROBLEM_PLACE_ID", "GKH_COMM_PROBLEM_PLACE", "GKH_DICT_PROBLEM_PLACE", "ID"));
            Database.AddRefColumn("GKH_CIT_SUG_COMMENT", new RefColumn("EXECUTOR_MANORG_ID", "GKH_CIT_SUG_COMM_MANORG", "GKH_MANAGING_ORGANIZATION", "ID"));
            Database.AddRefColumn("GKH_CIT_SUG_COMMENT", new RefColumn("EXECUTOR_MUNICIPALITY_ID", "GKH_COMM_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"));
            Database.AddRefColumn("GKH_CIT_SUG_COMMENT", new RefColumn("EXECUTOR_ZONAL_INSP_ID", "GKH_COMM_ZONAL_INSP", "GKH_DICT_ZONAINSP", "ID"));
            Database.AddRefColumn("GKH_CIT_SUG_COMMENT", new RefColumn("EXECUTOR_CR_FUND_ID", "GKH_CIT_SUG_COMM_CR_FUND", "GKH_CONTRAGENT_CONTACT", "ID"));

            Database.AddRefColumn("GKH_CIT_SUG", new RefColumn("MESSAGE_SUBJECT_ID", "GKH_SUG_MESSAGE_SUBJECT", "GKH_DICT_MESSAGE_SUBJECT", "ID"));
            Database.AddRefColumn("GKH_CIT_SUG", new RefColumn("ROOM_ID", "GKH_CIT_SUG_ROOM", "GKH_ROOM", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT", "IS_FIRST");
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT", "DESCRIPTION");
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT", "PROBLEM_PLACE_ID");
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT", "EXECUTOR_MANORG_ID");
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT", "EXECUTOR_MUNICIPALITY_ID");
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT", "EXECUTOR_ZONAL_INSP_ID");
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT", "EXECUTOR_CR_FUND_ID");

            Database.RemoveColumn("GKH_CIT_SUG", "MESSAGE_SUBJECT_ID");
            Database.RemoveColumn("GKH_CIT_SUG", "ROOM_ID");

        }
    }
}
