namespace Bars.Gkh.Migrations._2016.Version_2016092700
{
	using System.Data;

	using Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2016092700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2016.Version_2016092200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
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
		}
    }
}