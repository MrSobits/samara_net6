namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015030200
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015030200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015022700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("PAYER_NAME", DbType.String, 200));
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("RECIPIENT_NAME", DbType.String, 200));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "PAYER_NAME");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "RECIPIENT_NAME");
        }
    }
}
