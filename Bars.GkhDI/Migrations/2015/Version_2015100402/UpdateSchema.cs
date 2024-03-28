namespace Bars.GkhDi.Migrations.Version_2015100402
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100402")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015100401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("COM_SRV_START_ADVANCE_PAY", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("COM_SRV_START_CARRYOV_FND", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("COM_SRV_START_DEBT", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("COM_SRV_END_ADVANCE_PAY", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("COM_SRV_END_CARRYOV_FND", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("COM_SRV_END_DEBT", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("COM_SRV_RCV_PRETEN_CNT", DbType.Int32));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("COM_SRV_APROVE_PRETEN_CNT", DbType.Int32));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("COM_SRV_NO_APR_PRETEN_CNT", DbType.Int32));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("COM_SRV_PRETEN_RECALC_SUM", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "COM_SRV_START_ADVANCE_PAY");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "COM_SRV_START_CARRYOV_FND");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "COM_SRV_START_DEBT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "COM_SRV_END_ADVANCE_PAY");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "COM_SRV_END_CARRYOV_FND");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "COM_SRV_END_DEBT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "COM_SRV_RCV_PRETEN_CNT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "COM_SRV_APROVE_PRETEN_CNT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "COM_SRV_NO_APR_PRETEN_CNT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "COM_SRV_PRETEN_RECALC_SUM");
        }
    }
}