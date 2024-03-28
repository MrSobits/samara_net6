namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017100400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017100400")]
    [MigrationDependsOn(typeof(Version_2017082100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("REGOP_IMPORTED_PAYMENT", "PAND_STATE_REASON", DbType.Int16, ColumnProperty.Null);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "PAND_STATE_REASON");
        }
    }
}