namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014012900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014012800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_RO_CHARGE_ACC_CHARGE", new Column("CPAID", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("REGOP_RO_CHARGE_ACC_CHARGE", new Column("CCHARGED_PENALTY", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("REGOP_RO_CHARGE_ACC_CHARGE", new Column("CPAID_PENALTY", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("REGOP_RO_CHARGE_ACC_CHARGE", new Column("CSALDO_IN", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("REGOP_RO_CHARGE_ACC_CHARGE", new Column("CSALDO_OUT", DbType.Decimal, ColumnProperty.NotNull, 0));

            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("CDATE_OPEN", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("CDATE_CLOSE", DbType.DateTime, ColumnProperty.Null));
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("CTOTAL_DEBIT", DbType.Decimal, ColumnProperty.NotNull));
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("CTOTAL_CREDIT", DbType.DateTime, ColumnProperty.NotNull));

            Database.AddColumn("REGOP_RO_CHARGE_ACCOUNT", new Column("CDATE_OPEN", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddColumn("REGOP_RO_CHARGE_ACCOUNT", new Column("CDATE_CLOSE", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_CHARGE_ACCOUNT", "CDATE_OPEN");
            Database.RemoveColumn("REGOP_RO_CHARGE_ACCOUNT", "CDATE_CLOSE");

            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "CDATE_OPEN");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "CDATE_CLOSE");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "CTOTAL_DEBIT");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "CTOTAL_CREDIT");

            Database.RemoveColumn("REGOP_RO_CHARGE_ACC_CHARGE", "CPAID");
            Database.RemoveColumn("REGOP_RO_CHARGE_ACC_CHARGE", "CCHARGED_PENALTY");
            Database.RemoveColumn("REGOP_RO_CHARGE_ACC_CHARGE", "CPAID_PENALTY");
            Database.RemoveColumn("REGOP_RO_CHARGE_ACC_CHARGE", "CSALDO_IN");
            Database.RemoveColumn("REGOP_RO_CHARGE_ACC_CHARGE", "CSALDO_OUT");
        }
    }
}
