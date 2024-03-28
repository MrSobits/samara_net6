namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014080900
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014080800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("SALDO_IN_SERV", DbType.Decimal));
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("SALDO_CHANGE_SERV", DbType.Decimal));
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("SALDO_OUT_SERV", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "SALDO_IN_SERV");
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "SALDO_CHANGE_SERV");
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "SALDO_OUT_SERV");
        }
    }
}
