namespace Bars.Gkh.RegOperator.GeneralState
{
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Gkh.GeneralState;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Манифест для регистрации обобщенных состояний
    /// </summary>
    public class GeneralStatefulEntityManifest : IGeneralStatefulEntityManifest
    {
        /// <inheritdoc />
        public IEnumerable<GeneralStatefulEntityInfo> GetAllInfo()
        {
            return new GeneralStatefulEntityInfo[]
           {
                GeneralStatefulEntityInfo.Register<ImportedPayment, ImportedPaymentPaymentConfirmState>(
                    x => x.PaymentConfirmationState,
                    "regop_imported_payment",
                    x => x.GetDisplayName()),

                GeneralStatefulEntityInfo.Register<BankAccountStatement, DistributionState>(
                    x => x.DistributeState,
                    "regop_bank_acc_statement",
                    x => x.GetDisplayName()),
           };
        }
    }
}