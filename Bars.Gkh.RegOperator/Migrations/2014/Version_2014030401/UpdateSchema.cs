namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014030401
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014030400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_RO_CHARGE_ACCOUNT", new Column("CHARGE_TOTAL", DbType.Decimal, ColumnProperty.NotNull, "0"));
            Database.AddColumn("REGOP_RO_CHARGE_ACCOUNT", new Column("PAID_TOTAL", DbType.Decimal, ColumnProperty.NotNull, "0"));

            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("DEBT_TOTAL", DbType.Decimal, ColumnProperty.NotNull, "0"));
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("CREDIT_TOTAL", DbType.Decimal, ColumnProperty.NotNull, "0"));

            Database.AddColumn("REGOP_RO_SUPP_ACC", new Column("SALDO", DbType.Decimal, ColumnProperty.NotNull, "0"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_CHARGE_ACCOUNT", "CHARGE_TOTAL");
            Database.RemoveColumn("REGOP_RO_CHARGE_ACCOUNT", "PAID_TOTAL");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "DEBT_TOTAL");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "CREDIT_TOTAL");
            Database.RemoveColumn("REGOP_RO_SUPP_ACC", "SALDO");
        }
    }
}
