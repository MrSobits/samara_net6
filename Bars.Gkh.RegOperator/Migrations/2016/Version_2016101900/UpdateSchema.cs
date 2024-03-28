namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016101900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция RegOperator 2016101900
    /// </summary>
    [Migration("2016101900")]
    [MigrationDependsOn(typeof(Version_2016101700.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016101500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            var oldTimeout = this.Database.CommandTimeout;
            this.Database.CommandTimeout = 2 * 60 * 60;

            // добавляем столбцы периода в базовые
            // оставил так, в случае, если не сможем заполнить нормально, то вручную создадим столбцы
            // и заполним данными
            if (!this.Database.ColumnExists("REGOP_PAYMENT_OPERATION_BASE", "PERIOD_ID"))
            {
                this.Database.AddRefColumn("REGOP_PAYMENT_OPERATION_BASE", new RefColumn("PERIOD_ID", ColumnProperty.Null, "REGOP_PAYMENT_OPERATION_BASE_PI", "REGOP_PERIOD", "ID"));
            }

            if (!this.Database.ColumnExists("REGOP_CHARGE_OPERATION_BASE", "PERIOD_ID"))
            {
                this.Database.AddRefColumn("REGOP_CHARGE_OPERATION_BASE", new RefColumn("PERIOD_ID", ColumnProperty.Null, "REGOP_CHARGE_OPERATION_BASE_PI", "REGOP_PERIOD", "ID"));
            }

            // проставляем период по обеим таблицам
            this.Database.ExecuteNonQuery(@"UPDATE regop_payment_operation_base base
                                            SET period_id = q.period_id
                                            FROM
                                              (SELECT
                                                 b.id,
                                                 p.id period_id
                                               FROM regop_payment_operation_base b
                                                 LEFT JOIN regop_period p ON p.cstart <= b.fact_op_date::date AND b.fact_op_date::date <= coalesce(p.cend, 'infinity')) q
                                            WHERE q.id = base.id and base.period_id is null");

            this.Database.ExecuteNonQuery(@" update regop_charge_operation_base base
                                              set period_id = q.period_id
                                              from (select id, period_id from regop_saldo_change_source) q
                                             where q.id = base.id and base.period_id is null");

            this.Database.ExecuteNonQuery(@"UPDATE regop_charge_operation_base base
                                            SET period_id = q.period_id
                                            FROM
                                              (SELECT
                                                 b.id,
                                                 p.id period_id
                                               FROM regop_charge_operation_base b
                                                 LEFT JOIN regop_period p ON p.cstart <= b.FACT_OP_DATE::date AND b.FACT_OP_DATE::date <= coalesce(p.cend, 'infinity')) q
                                            WHERE q.id = base.id and base.period_id is null");

            // меняем nullable для этих таблиц
            this.Database.ExecuteNonQuery(@" 
                alter table regop_pers_acc_period_summ alter COLUMN dec_balance_change set DEFAULT 0;
                alter table regop_pers_acc_period_summ alter COLUMN penalty_balance_change set DEFAULT 0;");

            this.Database.AlterColumnSetNullable("REGOP_PAYMENT_OPERATION_BASE", "PERIOD_ID", false);
            this.Database.AlterColumnSetNullable("REGOP_CHARGE_OPERATION_BASE", "PERIOD_ID", false);

            this.Database.AlterColumnSetNullable("REGOP_PERS_ACC_PERIOD_SUMM", "DEC_BALANCE_CHANGE", false);
            this.Database.AlterColumnSetNullable("REGOP_PERS_ACC_PERIOD_SUMM", "PENALTY_BALANCE_CHANGE", false);

            // таблица для оплат в закрытый период
            if (!this.Database.TableExists("REGOP_CLOSED_PERIOD_PAYMENT"))
            {
                this.Database.AddTable("REGOP_CLOSED_PERIOD_PAYMENT", new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique));
                this.Database.AddForeignKey("FK_REGOP_CLOSED_PERIOD_PAYMENT_ID", "REGOP_CLOSED_PERIOD_PAYMENT", "ID", "REGOP_PAYMENT_OPERATION_BASE", "ID");
            }

            if (!this.Database.ColumnExists("PAYMENTS_TO_CLOSED_PERIODS_IMPORT", "PAYMENT_OP_ID"))
            {
                this.Database.AddRefColumn(
                    "PAYMENTS_TO_CLOSED_PERIODS_IMPORT", 
                    new RefColumn("PAYMENT_OP_ID", "PAYMENTS_TO_CLOSED_PERIODS_IMPORT_OP_ID", "REGOP_PAYMENT_OPERATION_BASE", "ID"));
            }

            // Индексы
            this.Database.AddIndex("REGOP_PAYMENT_OPERATION_BASE_GUID", false, "REGOP_PAYMENT_OPERATION_BASE", "GUID");
            this.Database.AddIndex("REGOP_CHARGE_OPERATION_BASE_GUID", false, "REGOP_CHARGE_OPERATION_BASE", "GUID");
            this.Database.AddIndex("PAYMENTS_TO_CLOSED_PERIODS_IMPORT_GUID", false, "PAYMENTS_TO_CLOSED_PERIODS_IMPORT", "TRANSFER_GUID");

            // удаляем период из изменения сальдо
            this.Database.RemoveColumn("REGOP_SALDO_CHANGE_SOURCE", "PERIOD_ID");

            this.Database.CommandTimeout = oldTimeout;
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
        }
    }
}
