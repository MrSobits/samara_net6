namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014101800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014101800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014101700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("MONEY_LOCKED", DbType.Decimal, ColumnProperty.NotNull, "0"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "MONEY_LOCKED");
        }
    }
}
