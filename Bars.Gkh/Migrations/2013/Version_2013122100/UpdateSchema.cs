namespace Bars.Gkh.Migrations.Version_2013122100
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013122000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("PAYMENT_SIZE_CR", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("PAYMENT_CHARGE_ALL", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("PAYMENT_PAID_ALL", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("PAYMENT_DEBT_ALL", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("PAYMENT_CHARGE_MONTH", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("PAYMENT_PAID_MONTH", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("PAYMENT_DEBT_MONTH", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("PENALTIES_CHARGE_ALL", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("PENALTIES_PAID_ALL", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("PENALTIES_DEBT_ALL", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("PENALTIES_CHARGE_MONTH", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("PENALTIES_PAID_MONTH", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", new Column("PENALTIES_DEBT_MONTH", DbType.Decimal));
        }
        
        public override void Down()
        {
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "PAYMENT_SIZE_CR");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "PAYMENT_CHARGE_ALL");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "PAYMENT_PAID_ALL");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "PAYMENT_DEBT_ALL");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "PAYMENT_CHARGE_MONTH");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "PAYMENT_PAID_MONTH");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "PAYMENT_DEBT_MONTH");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "PENALTIES_CHARGE_ALL");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "PENALTIES_PAID_ALL");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "PENALTIES_DEBT_ALL");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "PENALTIES_CHARGE_MONTH");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "PENALTIES_PAID_MONTH");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "PENALTIES_DEBT_MONTH");
        }
    }
}