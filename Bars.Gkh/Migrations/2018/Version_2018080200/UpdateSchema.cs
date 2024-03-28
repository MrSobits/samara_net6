namespace Bars.Gkh.Migrations._2018.Version_2018080200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018080200")]
    [MigrationDependsOn(typeof(Version_2018072300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("CLW_LAWSUIT_DOCUMENTATION", new RefColumn("ROSP_ID", "CLW_LAWSUIT_DOCUMENTATION_JINST_ID", "CLW_JUR_INSTITUTION", "ID"));
            Database.AddColumn("CLW_LAWSUIT_DOCUMENTATION", new Column("COLLECT_DEBT_FROM", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT_DOCUMENTATION", "ROSP_ID");
            Database.RemoveColumn("CLW_LAWSUIT_DOCUMENTATION", "COLLECT_DEBT_FROM");
        }
    }
}