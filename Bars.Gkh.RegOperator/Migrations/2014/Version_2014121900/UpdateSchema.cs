namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014121900
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014121200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Кто-то насоздавал вьюх в бд - теперь запрос не выполняется
//            ViewManager.DropAll(Database);

//            // Где для оракла? ¯\_(ツ)_/¯
//            //select 'alter table '||cl.relname||' alter column '||attr.attname||' type numeric(19, 2);'
//            //from pg_attribute attr
//            //join pg_class cl on cl.oid = attr.attrelid
//            //join pg_type t on t.oid = attr.atttypid
//            //where cl.relname like 'regop_%' and t.typname = 'numeric'

//            Database.ExecuteNonQuery(@"alter table regop_suspense_account alter column sum type numeric(19, 2);
//alter table regop_accumulated_funds alter column accumulated_sum type numeric(19, 2);
//alter table regop_bank_acc_stmnt alter column doc_sum type numeric(19, 2);
//alter table regop_bank_account alter column ccurr_balance type numeric(19, 2);
//alter table regop_bank_doc_import alter column imported_sum type numeric(19, 2);
//alter table regop_bankacc_stmnt_group alter column sum type numeric(19, 2);
//alter table regop_calc_acc alter column total_out type numeric(19, 2);
//alter table regop_calc_acc alter column total_in type numeric(19, 2);
//alter table regop_calc_acc alter column abalance type numeric(19, 2);
//alter table regop_calc_acc alter column abalance_in type numeric(19, 2);
//alter table regop_calc_acc alter column abalance_out type numeric(19, 2);
//alter table regop_calc_acc_credit alter column credit_sum type numeric(19, 2);
//alter table regop_calc_acc_credit alter column percent_sum type numeric(19, 2);
//alter table regop_calc_acc_credit alter column credit_debt type numeric(19, 2);
//alter table regop_calc_acc_credit alter column percent_debt type numeric(19, 2);
//alter table regop_calc_acc_credit alter column percent_rate type numeric(19, 2);
//alter table regop_calc_acc_overdraft alter column overdraft_limit type numeric(19, 2);
//alter table regop_calc_acc_overdraft alter column overdraft_period type numeric(19, 2);
//alter table regop_calc_acc_overdraft alter column percent_rate type numeric(19, 2);
//alter table regop_calc_acc_overdraft alter column available_sum type numeric(19, 2);
//alter table regop_cancel_payment alter column down_sum type numeric(19, 2);
//alter table regop_creditorg_service_cond alter column cash_service_size type numeric(19, 2);
//alter table regop_creditorg_service_cond alter column opening_acc_pay type numeric(19, 2);
//alter table regop_debtor alter column penalty_debt type numeric(19, 2);
//alter table regop_debtor alter column debt_sum type numeric(19, 2);
//alter table regop_fed_standard_fee_cr alter column value type numeric(19, 2);
//alter table regop_imported_payment alter column payment_sum type numeric(19, 2);
//alter table regop_money_lock alter column amount type numeric(19, 2);
//alter table regop_money_operation alter column amount type numeric(19, 2);
//alter table regop_payment_order_detail alter column paid_sum type numeric(19, 2);
//alter table regop_payment_order_detail alter column amount type numeric(19, 2);
//alter table regop_payment_penalties alter column percentage type numeric(19, 2);
//alter table regop_paysize_rec alter column dvalue type numeric(19, 2);
//alter table regop_paysize_rec_ret alter column dvalue type numeric(19, 2);
//alter table regop_penalty_change alter column current_val type numeric(19, 2);
//alter table regop_penalty_change alter column new_val type numeric(19, 2);
//alter table regop_pers_acc alter column area_share type numeric(19, 2);
//alter table regop_pers_acc alter column tariff type numeric(19, 2);
//alter table regop_pers_acc alter column carea type numeric(19, 2);
//alter table regop_pers_acc alter column larea type numeric(19, 2);
//alter table regop_pers_acc alter column tariff_charge_balance type numeric(19, 2);
//alter table regop_pers_acc alter column decision_charge_balance type numeric(19, 2);
//alter table regop_pers_acc alter column penalty_charge_balance type numeric(19, 2);
//alter table regop_pers_acc_charge alter column charge type numeric(19, 2);
//alter table regop_pers_acc_charge alter column charge_tariff type numeric(19, 2);
//alter table regop_pers_acc_charge alter column penalty type numeric(19, 2);
//alter table regop_pers_acc_charge alter column recalc type numeric(19, 2);
//alter table regop_pers_acc_charge alter column overplus type numeric(19, 2);
//alter table regop_pers_acc_payment alter column payment_sum type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column charge_tariff type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column penalty type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column recalc type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column saldo_in type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column saldo_out type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column tariff_payment type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column saldo_in_serv type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column saldo_change_serv type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column saldo_out_serv type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column penalty_payment type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column charge_base_tariff type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column tariff_desicion_payment type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column balance_change type numeric(19, 2);
//alter table regop_prev_work_pay alter column sum type numeric(19, 2);
//alter table regop_privileged_category alter column percent type numeric(19, 2);
//alter table regop_rent_payment_in alter column payment_sum type numeric(19, 2);
//alter table regop_ro_charge_acc_charge alter column ccharged type numeric(19, 2);
//alter table regop_ro_charge_acc_charge alter column cpaid type numeric(19, 2);
//alter table regop_ro_charge_acc_charge alter column ccharged_penalty type numeric(19, 2);
//alter table regop_ro_charge_acc_charge alter column cpaid_penalty type numeric(19, 2);
//alter table regop_ro_charge_acc_charge alter column csaldo_in type numeric(19, 2);
//alter table regop_ro_charge_acc_charge alter column csaldo_out type numeric(19, 2);
//alter table regop_ro_charge_acc_charge alter column balance_change type numeric(19, 2);
//alter table regop_ro_charge_account alter column charge_total type numeric(19, 2);
//alter table regop_ro_charge_account alter column paid_total type numeric(19, 2);
//alter table regop_ro_loan alter column loan_sum type numeric(19, 2);
//alter table regop_ro_loan alter column loan_returned_sum type numeric(19, 2);
//alter table regop_ro_loan_wallet alter column sum type numeric(19, 2);
//alter table regop_ro_loan_wallet alter column returned_sum type numeric(19, 2);
//alter table regop_ro_payment_acc_op alter column coper_sum type numeric(19, 2);
//alter table regop_ro_payment_account alter column debt_total type numeric(19, 2);
//alter table regop_ro_payment_account alter column credit_total type numeric(19, 2);
//alter table regop_ro_payment_account alter column money_locked type numeric(19, 2);
//alter table regop_ro_subsidy_acc_op alter column oper_sum type numeric(19, 2);
//alter table regop_ro_supp_acc_op alter column credit type numeric(19, 2);
//alter table regop_ro_supp_acc_op alter column debt type numeric(19, 2);
//alter table regop_summary_saldo_change alter column current_val type numeric(19, 2);
//alter table regop_summary_saldo_change alter column new_val type numeric(19, 2);
//alter table regop_suspacc_credit alter column cpayment type numeric(19, 2);
//alter table regop_suspacc_credit alter column ppayment type numeric(19, 2);
//alter table regop_tr_acc_cred alter column d_sum type numeric(19, 2);
//alter table regop_tr_acc_cred alter column conf_sum type numeric(19, 2);
//alter table regop_tr_acc_cred alter column divergence type numeric(19, 2);
//alter table regop_tr_acc_deb alter column d_sum type numeric(19, 2);
//alter table regop_tr_acc_deb alter column conf_sum type numeric(19, 2);
//alter table regop_tr_acc_deb alter column divergence type numeric(19, 2);
//alter table regop_transfer alter column amount type numeric(19, 2);
//alter table regop_transfer_obj alter column transferred_sum type numeric(19, 2);
//alter table regop_transit_acc alter column r_sum type numeric(19, 2);
//alter table regop_unaccept_charge alter column ccharge type numeric(19, 2);
//alter table regop_unaccept_charge alter column ccharge_tariff type numeric(19, 2);
//alter table regop_unaccept_charge alter column cpenalty type numeric(19, 2);
//alter table regop_unaccept_charge alter column crecalc type numeric(19, 2);
//alter table regop_unaccept_charge alter column tariff_overplus type numeric(19, 2);
//alter table regop_unaccept_pay alter column penalty_sum type numeric(19, 2);
//alter table regop_unaccept_pay alter column payment_sum type numeric(19, 2);
//alter table regop_unaccept_pay_loan alter column psum type numeric(19, 2);
//alter table regop_unaccept_pay_packet alter column packet_sum type numeric(19, 2);
//alter table regop_wallet alter column balance type numeric(19, 2);
//alter table regop_pers_acc_charge alter column recalc_decision type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column recalc_decision type numeric(19, 2);
//alter table regop_unaccept_charge alter column recalc_decision type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column overhaul_payment type numeric(19, 2);
//alter table regop_pers_acc_period_summ alter column recruitment_payment type numeric(19, 2);
//alter table regop_transfer_ctr_detail alter column paid_sum type numeric(19, 2);
//alter table regop_transfer_ctr_detail alter column amount type numeric(19, 2);");

//            ViewManager.CreateAll(Database);
        }

        public override void Down()
        {
        }
    }
}
