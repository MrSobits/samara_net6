namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014070100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014070100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014063000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("REGOP_CREDITORG_SERVICE_COND", "PENALTY_CONTRACT_ID");
            Database.RemoveColumn("REGOP_CREDITORG_SERVICE_COND", "SUM_CONTRACT_ID");

	        if (!Database.ColumnExists("GKH_PAYMENT_AGENT", "PENALTY_CONTRACT_ID"))
	        {
		        Database.AddColumn("GKH_PAYMENT_AGENT", new Column("PENALTY_CONTRACT_ID", DbType.String, 10, ColumnProperty.Null));
	        }
	        if (!Database.ColumnExists("GKH_PAYMENT_AGENT", "SUM_CONTRACT_ID"))
	        {
		        Database.AddColumn("GKH_PAYMENT_AGENT", new Column("SUM_CONTRACT_ID", DbType.String, 10, ColumnProperty.Null));
	        }
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_PAYMENT_AGENT", "PENALTY_CONTRACT_ID");
            Database.RemoveColumn("GKH_PAYMENT_AGENT", "SUM_CONTRACT_ID");

            Database.AddColumn("REGOP_CREDITORG_SERVICE_COND", new Column("PENALTY_CONTRACT_ID", DbType.String, 10, ColumnProperty.Null));
            Database.AddColumn("REGOP_CREDITORG_SERVICE_COND", new Column("SUM_CONTRACT_ID", DbType.String, 10, ColumnProperty.Null));
        }
    }
}
