namespace Bars.Gkh.Migrations._2018.Version_2018041500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018041500")]
    [MigrationDependsOn(typeof(Version_2018040700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT", new Column("JUDGE_NAME", DbType.String, ColumnProperty.None));
            Database.AddColumn("CLW_LAWSUIT", new Column("COURT_BUISNESS_NUMBER", DbType.String, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT", "COURT_BUISNESS_NUMBER");
            Database.RemoveColumn("CLW_LAWSUIT", "JUDGE_NAME");
        }
    }
}