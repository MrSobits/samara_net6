namespace Bars.Gkh.Migrations._2018.Version_2018072300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018072300")]
    [MigrationDependsOn(typeof(Version_2018061400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_JUR_INSTITUTION", new Column("JUDGE_POSITION", DbType.String, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_JUR_INSTITUTION", "JUDGE_POSITION");
        }
    }
}