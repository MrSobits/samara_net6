namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016101700
{
    using System.Data;
    
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2016101700
    /// </summary>
    [Migration("2016101700")]
    [MigrationDependsOn(typeof(Version_2016100600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"-- проставление на 0 новых полей
                                drop index ind_rop_pers_acc_per_sum_p;
                                drop index ind_rop_pers_acc_per_sum_acc;

                                update REGOP_PERS_ACC_PERIOD_SUMM s
                                set
                                    dec_balance_change = coalesce(s.dec_balance_change, 0),
                                    penalty_balance_change = coalesce(s.penalty_balance_change, 0)
                                where dec_balance_change is null or penalty_balance_change is null;

                                CREATE INDEX ind_rop_pers_acc_per_sum_p ON regop_pers_acc_period_summ USING btree (period_id);
                                CREATE INDEX ind_rop_pers_acc_per_sum_acc ON regop_pers_acc_period_summ USING btree (account_id);

                                analyze regop_pers_acc_period_summ;");
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {

        }
    }
}
