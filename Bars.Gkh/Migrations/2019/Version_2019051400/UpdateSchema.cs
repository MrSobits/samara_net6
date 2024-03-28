namespace Bars.Gkh.Migrations._2019.Version_2019051400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2019051400")]
    [MigrationDependsOn(typeof(Version_2019020100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("ZVSPCourtDecision", DbType.Int32, 4, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "ZVSPCourtDecision");
        }
    }
}