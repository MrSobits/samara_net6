namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014120200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014120100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_TRANSFER_CTR_DETAIL",
                new RefColumn("TRANSFER_CTR_ID", ColumnProperty.NotNull, "REGOP_TRANSFER_CTR_DCTR", "RF_TRANSFER_CTR", "ID"),
                new RefColumn("WALLET_ID", ColumnProperty.NotNull, "REGOP_TRANSFER_CTR_WLT", "REGOP_WALLET", "ID"),
                new Column("PAID_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("AMOUNT", DbType.Decimal, ColumnProperty.NotNull, 0m));

            Database.RemoveColumn("RF_TRANSFER_CTR", "BUDGET_MU");
            Database.RemoveColumn("RF_TRANSFER_CTR", "BUDGET_SUB");
            Database.RemoveColumn("RF_TRANSFER_CTR", "OWNER_RES");
            Database.RemoveColumn("RF_TRANSFER_CTR", "FUND_RES");
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_TRANSFER_CTR_DETAIL");

            Database.AddColumn("RF_TRANSFER_CTR", new Column("BUDGET_MU", DbType.Decimal));
            Database.AddColumn("RF_TRANSFER_CTR", new Column("BUDGET_SUB", DbType.Decimal));
            Database.AddColumn("RF_TRANSFER_CTR", new Column("OWNER_RES", DbType.Decimal));
            Database.AddColumn("RF_TRANSFER_CTR", new Column("FUND_RES", DbType.Decimal));
        }
    }
}
