namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ExecutionAction;

    /// <summary>
    /// Действие по переносу конфигурации по реестру должников из раздела
    /// </summary>
    public class MirateDebtorConfiguration : BaseExecutionAction
    {
        /// <inheritdoc />
        public override string Name => "Действие по переносу конфигурации по реестру должников из раздела";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        /// <inheritdoc />
        public override string Description => this.Name;

        private BaseDataResult Execute()
        {
            var configProvider = this.Container.Resolve<IGkhConfigProvider>();

            var debtorConfigFromRegop = configProvider.Get<RegOperatorConfig>().GeneralConfig.DebtorRegistryConfig;
            var debtorConfigFromDebtor = configProvider.Get<RegOperatorConfig>().DebtorConfig.DebtorRegistryConfig;

            debtorConfigFromDebtor.DebtOperand = debtorConfigFromRegop.DebtOperand;
            debtorConfigFromDebtor.DebtSum = debtorConfigFromRegop.DebtSum;
            debtorConfigFromDebtor.PenaltyDebt = debtorConfigFromRegop.PenaltyDebt;
            debtorConfigFromDebtor.ExpirationDaysCount = debtorConfigFromRegop.ExpirationDaysCount;

            configProvider.SaveChanges();

            return new BaseDataResult();
        }
    }
}