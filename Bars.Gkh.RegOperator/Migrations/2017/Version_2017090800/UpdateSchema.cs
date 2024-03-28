namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017090800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017090800")]
    [MigrationDependsOn(typeof(Version_2017082300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_DEBTOR", "DEBT_BASE_TARIFF_SUM", DbType.Decimal, ColumnProperty.NotNull, 0);
            this.Database.AddColumn("REGOP_DEBTOR", "DEBT_DECISION_TARIFF_SUM", DbType.Decimal, ColumnProperty.NotNull, 0);

            this.Database.AddColumn("CLW_CLAIM_WORK_ACC_DETAIL", "CUR_CHARGE_BASE_TARIFF_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0);
            this.Database.AddColumn("CLW_CLAIM_WORK_ACC_DETAIL", "CUR_CHARGE_DECISION_TARIFF_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0);

            this.Database.AddColumn("CLW_CLAIM_WORK_ACC_DETAIL", "ORIG_CHARGE_BASE_TARIFF_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0);
            this.Database.AddColumn("CLW_CLAIM_WORK_ACC_DETAIL", "ORIG_CHARGE_DECISION_TARIFF_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0);
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_DEBTOR", "DEBT_BASE_TARIFF_SUM");
            this.Database.RemoveColumn("REGOP_DEBTOR", "DEBT_DECISION_TARIFF_SUM");

            this.Database.RemoveColumn("CLW_CLAIM_WORK_ACC_DETAIL", "CUR_CHARGE_BASE_TARIFF_DEBT");
            this.Database.RemoveColumn("CLW_CLAIM_WORK_ACC_DETAIL", "CUR_CHARGE_DECISION_TARIFF_DEBT");

            this.Database.RemoveColumn("CLW_CLAIM_WORK_ACC_DETAIL", "ORIG_CHARGE_BASE_TARIFF_DEBT");
            this.Database.RemoveColumn("CLW_CLAIM_WORK_ACC_DETAIL", "ORIG_CHARGE_DECISION_TARIFF_DEBT");
        }
    }
}