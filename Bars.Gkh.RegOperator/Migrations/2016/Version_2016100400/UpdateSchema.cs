namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016100400
{
    using System.Data;
    
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2016100400
    /// </summary>
    [Migration("2016100400")]
    [MigrationDependsOn(typeof(Version_2016092300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016091001.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.RenameTable("REGOP_SALDO_CHAGE_SOURCE", "REGOP_SALDO_CHANGE_SOURCE");
            this.Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("DEC_BALANCE_CHANGE", DbType.Decimal));
            this.Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("PENALTY_BALANCE_CHANGE", DbType.Decimal));

            this.Database.ExecuteNonQuery(@"update regop_pers_acc_period_summ ps set
                  balance_change = ps.balance_change + ch.balance_change, --суммируем, если был зачёт средств за ранее проделанные работы или изменение баланса старым методом
                  dec_balance_change = ch.dec_change,
                  penalty_balance_change = ch.pen_change,
                  charge_base_tariff = ps.charge_base_tariff - ch.balance_change,
                  charge_tariff = ps.charge_tariff - ch.balance_change - ch.dec_change,
                  penalty = ps.penalty - ch.pen_change
                FROM (SELECT
                        sum(case when det.CHANGE_TYPE = 0 then det.new_value - det.old_value else 0 end) balance_change,
                        sum(case when det.CHANGE_TYPE = 1 then det.new_value - det.old_value else 0 end) dec_change,
                        sum(case when det.CHANGE_TYPE = 2 then det.new_value - det.old_value else 0 end) pen_change,
                        det.acc_id,
                        source.period_id
                  FROM regop_saldo_change_detail det
                  join regop_saldo_change_source source on source.id = det.charge_op_id
                  group by det.acc_id, source.period_id) ch 
                where ps.account_id = ch.acc_id and ps.period_id = ch.acc_id;");
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "DEC_BALANCE_CHANGE");
            this.Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "PENALTY_BALANCE_CHANGE");
        }
    }
}
