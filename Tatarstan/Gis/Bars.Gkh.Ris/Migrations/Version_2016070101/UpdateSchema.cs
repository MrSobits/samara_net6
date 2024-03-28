namespace Bars.Gkh.Ris.Migrations.Version_2016070101
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016070101")]
    [MigrationDependsOn(typeof(Version_2016062700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddColumn("RIS_PAYMENT_DOC", new Column("ADVANCE_BLLING_PERIOD", DbType.Decimal));
            //this.Database.AddColumn("RIS_PAYMENT_DOC", new Column("DEBT_PREVIOUS_PERIODS", DbType.Decimal));

            //this.Database.AddColumn("RIS_HOUSING_SERVICE_CHARGE_INFO", new Column("TOTAL_PAYABLE", DbType.Decimal, ColumnProperty.NotNull));
            //this.Database.AddColumn("RIS_HOUSING_SERVICE_CHARGE_INFO", new Column("ACCOUNTING_PERIOD_TOTAL", DbType.Decimal, ColumnProperty.NotNull));
            //this.Database.AddColumn("RIS_HOUSING_SERVICE_CHARGE_INFO", new Column("CALC_EXPLANATION", DbType.String, 255));

            //this.Database.AddColumn("RIS_ADDITIONAL_SERVICE_CHARGE_INFO", new Column("TOTAL_PAYABLE", DbType.Decimal, ColumnProperty.NotNull));
            //this.Database.AddColumn("RIS_ADDITIONAL_SERVICE_CHARGE_INFO", new Column("ACCOUNTING_PERIOD_TOTAL", DbType.Decimal, ColumnProperty.NotNull));
            //this.Database.AddColumn("RIS_ADDITIONAL_SERVICE_CHARGE_INFO", new Column("CALC_EXPLANATION", DbType.String, 255));

            //this.Database.AddColumn("RIS_MUNICIPAL_SERVICE_CHARGE_INFO", new Column("TOTAL_PAYABLE", DbType.Decimal, ColumnProperty.NotNull));
            //this.Database.AddColumn("RIS_MUNICIPAL_SERVICE_CHARGE_INFO", new Column("ACCOUNTING_PERIOD_TOTAL", DbType.Decimal, ColumnProperty.NotNull));
            //this.Database.AddColumn("RIS_MUNICIPAL_SERVICE_CHARGE_INFO", new Column("CALC_EXPLANATION", DbType.String, 255));
            //this.Database.AddColumn("RIS_MUNICIPAL_SERVICE_CHARGE_INFO", new Column("INDIVIDUAL_CONSUMPTION_PAYABLE", DbType.Decimal, ColumnProperty.NotNull));
            //this.Database.AddColumn("RIS_MUNICIPAL_SERVICE_CHARGE_INFO", new Column("COMMUNAL_CONSUMPTION_PAYABLE", DbType.Decimal, ColumnProperty.NotNull));

        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveColumn("RIS_PAYMENT_DOC", "ADVANCE_BLLING_PERIOD");
            //this.Database.RemoveColumn("RIS_PAYMENT_DOC", "DEBT_PREVIOUS_PERIODS");

            //this.Database.RemoveColumn("RIS_HOUSING_SERVICE_CHARGE_INFO", "TOTAL_PAYABLE");
            //this.Database.RemoveColumn("RIS_HOUSING_SERVICE_CHARGE_INFO", "ACCOUNTING_PERIOD_TOTAL");
            //this.Database.RemoveColumn("RIS_HOUSING_SERVICE_CHARGE_INFO", "CALC_EXPLANATION");

            //this.Database.RemoveColumn("RIS_ADDITIONAL_SERVICE_CHARGE_INFO", "TOTAL_PAYABLE");
            //this.Database.RemoveColumn("RIS_ADDITIONAL_SERVICE_CHARGE_INFO", "ACCOUNTING_PERIOD_TOTAL");
            //this.Database.RemoveColumn("RIS_ADDITIONAL_SERVICE_CHARGE_INFO", "CALC_EXPLANATION");

            //this.Database.RemoveColumn("RIS_MUNICIPAL_SERVICE_CHARGE_INFO", "TOTAL_PAYABLE");
            //this.Database.RemoveColumn("RIS_MUNICIPAL_SERVICE_CHARGE_INFO", "ACCOUNTING_PERIOD_TOTAL");
            //this.Database.RemoveColumn("RIS_MUNICIPAL_SERVICE_CHARGE_INFO", "CALC_EXPLANATION");
            //this.Database.RemoveColumn("RIS_MUNICIPAL_SERVICE_CHARGE_INFO", "INDIVIDUAL_CONSUMPTION_PAYABLE");
            //this.Database.RemoveColumn("RIS_MUNICIPAL_SERVICE_CHARGE_INFO", "COMMUNAL_CONSUMPTION_PAYABLE");
        }
    }
}
