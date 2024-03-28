namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014062902
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062902")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014062901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.TableExists("REGOP_CREDITORG_SERVICE_CONDITION"))
            {
                Database.RenameTable("REGOP_CREDITORG_SERVICE_CONDITION", "REGOP_CREDITORG_SERVICE_COND");
            }

            Database.AddColumn("REGOP_CREDITORG_SERVICE_COND", new Column("PENALTY_CONTRACT_ID", DbType.String, 10));
            Database.AddColumn("REGOP_CREDITORG_SERVICE_COND", new Column("SUM_CONTRACT_ID", DbType.String, 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_CREDITORG_SERVICE_COND", "PENALTY_CONTRACT_ID");
            Database.RemoveColumn("REGOP_CREDITORG_SERVICE_COND", "SUM_CONTRACT_ID");
        }
    }
}
