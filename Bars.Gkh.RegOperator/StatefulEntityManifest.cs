namespace Bars.Gkh.RegOperator
{
    using System.Collections.Generic;

    using Bars.B4.Modules.States;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Loan;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    public class StatefulEntityManifest: IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("gkh_regop_personal_account", "Лицевой счет(модуль Регоператора)", typeof(BasePersonalAccount)),
                new StatefulEntityInfo("gkh_regop_reality_object_loan", "Займ дома", typeof(RealityObjectLoan)),
                new StatefulEntityInfo("rf_transfer_ctr", "Заявка на перечисление средств подрядчикам", typeof(TransferCtr)),
                new StatefulEntityInfo("clw_debtor_claim_work", "Претензионная работа по неплательщикам", typeof(DebtorClaimWork))
            };
        }
    }
}