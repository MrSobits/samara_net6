namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using DomainService.Interface;
    /// <summary>
    /// Контроллер начислений
    /// </summary>
    public class ChargeController : BaseController
    {
        /// <summary>
        /// Начисления по Л/С
        /// </summary>
        public IPersonalAccountCharger Charger { get; set; }
        /// <summary>
        /// Создать неподтвержденное начисление
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult MakeUnacceptedCharge(BaseParams baseParams)
        {
            return new JsonNetResult(this.Charger.CreateUnacceptedCharges(baseParams));
        }
        /// <summary>
        /// Подтвердить неподтвержденное начисление
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult Accept(BaseParams baseParams)
        {
            return new JsonNetResult(this.Charger.AcceptUnaccepted(baseParams));
        }

        /*public ActionResult ClearAll(BaseParams baseParams)
        {
            const string query =
                @"TRUNCATE regop_period CASCADE;
                TRUNCATE regop_pers_acc_charge CASCADE;
                TRUNCATE regop_pers_acc_payment CASCADE;
                TRUNCATE regop_pers_acc_period_summ CASCADE;
                TRUNCATE regop_ro_charge_acc_charge CASCADE;
                TRUNCATE regop_ro_loan CASCADE;
                TRUNCATE regop_ro_payment_acc_op CASCADE;
                TRUNCATE regop_ro_supp_acc_op CASCADE;
                TRUNCATE regop_saldo_change CASCADE;
                TRUNCATE regop_unaccept_c_packet CASCADE;
                TRUNCATE regop_unaccept_charge CASCADE;
                TRUNCATE regop_unaccept_pay_loan CASCADE;
                TRUNCATE regop_unaccept_pay CASCADE;
                TRUNCATE REGOP_PERS_ACC_CALC_PARAM CASCADE;
                TRUNCATE regop_persaccalcparamtmp CASCADE;
                UPDATE regop_ro_payment_account SET debt_total = 0;
                UPDATE regop_ro_payment_account SET credit_total = 0;
                UPDATE regop_ro_charge_account SET charge_total = 0;
                UPDATE regop_ro_charge_account SET paid_total = 0;";

            Container.Resolve<ISessionProvider>()
                .GetCurrentSession()
                .CreateSQLQuery(query)
                .ExecuteUpdate();

            new EntityLogHelper(Container).ClearTablebutLeaveLastApplied();

            return JsSuccess();
        }*/
    }
}