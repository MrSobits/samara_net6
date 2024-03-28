namespace Bars.Gkh.Migrations._2017.Version_2017091100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017091100")]
    [MigrationDependsOn(typeof(Version_2017082900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", "CUR_CHARGE_BASE_TARIFF_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0);
            this.Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", "CUR_CHARGE_DECISION_TARIFF_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0);

            this.Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", "ORIG_CHARGE_BASE_TARIFF_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0);
            this.Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", "ORIG_CHARGE_DECISION_TARIFF_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0);

            this.Database.AddColumn("CLW_LAWSUIT", "DEBT_BASE_TARIFF_SUM", DbType.Decimal, ColumnProperty.NotNull, 0);
            this.Database.AddColumn("CLW_LAWSUIT", "DEBT_DECISION_TARIFF_SUM", DbType.Decimal, ColumnProperty.NotNull, 0);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "CUR_CHARGE_BASE_TARIFF_DEBT");
            this.Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "CUR_CHARGE_DECISION_TARIFF_DEBT");

            this.Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "ORIG_CHARGE_BASE_TARIFF_DEBT");
            this.Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "ORIG_CHARGE_DECISION_TARIFF_DEBT");

            this.Database.RemoveColumn("CLW_LAWSUIT", "DEBT_BASE_TARIFF_SUM");
            this.Database.RemoveColumn("CLW_LAWSUIT", "DEBT_DECISION_TARIFF_SUM");
        }
    }
}