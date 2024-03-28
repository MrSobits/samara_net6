namespace Bars.GkhDi.Migrations.Version_2015072900
{
    using System;
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015072900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015072800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("ADVANCE_PAYMENTS", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("CARRYOVER_FUNDS", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("DEBT", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("CFMAR_MAINTANCE", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("CFMAR_REPAIRS", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("RC_FOWNERS", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("RC_FOTARGETED", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("RC_GRANT", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("RC_FCOMMONPROP", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("RC_FOTHER", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("CB_ADVANCE", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("CB_CARRYOVER", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("CB_DEBT", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("ALL_CASH_BALANCE", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("ALL_RECEIVED_CASH", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "ADVANCE_PAYMENTS");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "CARRYOVER_FUNDS");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "DEBT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "CFMAR_MAINTANCE");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "CFMAR_REPAIRS");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "RC_FOWNERS");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "RC_FOTARGETED");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "RC_GRANT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "RC_FCOMMONPROP");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "RC_FOTHER");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "CB_ADVANCE");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "CB_CARRYOVER");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "CB_DEBT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "ALL_CASH_BALANCE");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "ALL_RECEIVED_CASH");
        }
    }
}
