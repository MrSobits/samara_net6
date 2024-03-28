namespace Bars.GkhDi.Migrations.Version_2015091500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015091500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015091400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_DISINFO_RO_PAY_COMMUN", new Column("TOTAL_CONSUMP", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_RO_PAY_COMMUN", new Column("ACCRUAL_BY_PROV", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_RO_PAY_COMMUN", new Column("PAYED_TO_PROV", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_RO_PAY_COMMUN", new Column("DEBT_TO_PROV", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_RO_PAY_COMMUN", new Column("RECEIVED_PENALTY", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_DISINFO_RO_PAY_COMMUN", "TOTAL_CONSUMP");
            Database.RemoveColumn("DI_DISINFO_RO_PAY_COMMUN", "ACCRUAL_BY_PROV");
            Database.RemoveColumn("DI_DISINFO_RO_PAY_COMMUN", "PAYED_TO_PROV");
            Database.RemoveColumn("DI_DISINFO_RO_PAY_COMMUN", "DEBT_TO_PROV");
            Database.RemoveColumn("DI_DISINFO_RO_PAY_COMMUN", "RECEIVED_PENALTY");
        }
    }
}