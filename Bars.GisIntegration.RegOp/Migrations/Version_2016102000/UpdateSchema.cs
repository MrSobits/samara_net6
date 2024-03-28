namespace Bars.GisIntegration.RegOp.Migrations.Version_2016102000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016102000")]
    [MigrationDependsOn(typeof(Version_2016101700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddColumn("RIS_CAPITAL_REPAIR_CHARGE", new Column("ORG_PPAGUID", DbType.String));

            //this.Database.AddColumn("RIS_CAPITAL_REPAIR_DEBT", new Column("ORG_PPAGUID", DbType.String));

            //this.Database.AddColumn("RIS_PAYMENT_DOC", new Column("PAYMENTS_TAKEN", DbType.Byte));

            //this.Database.AddRisTable(
            //    "RIS_INSURANCE",
            //    new Column("INSURANCE_PRODUCT_GUID", DbType.String),
            //    new Column("RATE", DbType.Decimal),
            //    new Column("ACCOUNTING_PERIOD_TOTAL", DbType.Decimal),
            //    new Column("CALC_EXPLANATION", DbType.String),
            //    new Column("TOTAL_PAYABLE", DbType.Decimal),
            //    new Column("MONEY_DISCOUNT", DbType.Decimal),
            //    new Column("MONEY_RECALCULATION", DbType.Decimal));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveColumn("RIS_CAPITAL_REPAIR_CHARGE", "ORG_PPAGUID");
            //this.Database.RemoveColumn("RIS_CAPITAL_REPAIR_DEBT", "ORG_PPAGUID");
            //this.Database.RemoveColumn("RIS_PAYMENT_DOC", "PAYMENTS_TAKEN");
            //this.Database.RemoveTable("RIS_INSURANCE");
        }
    }
}