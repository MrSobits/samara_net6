namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092901
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PAYMENT_ORDER_DETAIL",
                new Column("PAID_SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("AMOUNT", DbType.Decimal, ColumnProperty.NotNull, 0),
                new RefColumn("PAYMENT_ORDER_ID", ColumnProperty.NotNull, "DETAIL_PAYMENT_ORDER", "CR_OBJ_PER_ACT_PAYMENT", "ID"),
                new RefColumn("WALLET_ID", ColumnProperty.NotNull, "DETAIL_WALLET", "REGOP_WALLET", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PAYMENT_ORDER_DETAIL");
        }
    }
}
