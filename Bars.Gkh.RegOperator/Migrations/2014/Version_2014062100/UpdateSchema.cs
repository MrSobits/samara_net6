using System.Data;

namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014062100
{
    using global::Bars.B4.Modules.Ecm7.Framework;


    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014062001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("PAYMENT_AGENT_CODE", DbType.String, 50));
            Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("PAYMENT_AGENT_NAME", DbType.String, 300));

            Database.AddColumn("REGOP_IMPORTED_PAYMENT", new Column("PAYMENT_NUM_US", DbType.String, 50));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_BANK_DOC_IMPORT", "PAYMENT_AGENT_CODE");
            Database.RemoveColumn("REGOP_BANK_DOC_IMPORT", "PAYMENT_AGENT_NAME");

            Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "PAYMENT_NUM_US");
        }
    }
}
