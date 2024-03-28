namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014021300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014021201.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("REGOP_RO_CHARGE_ACCOUNT", "CCURR_BALANCE");
            Database.RemoveColumn("REGOP_RO_CHARGE_ACCOUNT", "CDATE_OPEN");
            Database.RemoveColumn("REGOP_RO_CHARGE_ACCOUNT", "CDATE_CLOSE");

            Database.AddColumn("REGOP_RO_CHARGE_ACCOUNT", new Column("ACC_NUM", DbType.String, 20, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.AddColumn("REGOP_RO_CHARGE_ACCOUNT", new Column("CCURR_BALANCE", DbType.Decimal, ColumnProperty.NotNull));
            Database.AddColumn("REGOP_RO_CHARGE_ACCOUNT", new Column("CDATE_OPEN", DbType.AnsiString));
            Database.AddColumn("REGOP_RO_CHARGE_ACCOUNT", new Column("CDATE_CLOSE", DbType.AnsiString));
        }
    }
}
