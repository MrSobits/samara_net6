namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States.TransitionRules
{
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Проверка включения типа <see cref="ClaimWorkDocumentType.Lawsuit"/>
    /// </summary>
    public class IncludePretensionRule : IClwTransitionRule
    {
        private readonly IGkhConfigProvider configProv;

        public IncludePretensionRule(IWindsorContainer container)
        {
            this.configProv = container.Resolve<IGkhConfigProvider>();
        }

        #region Implementation of IClwDocumentCreationRule

        /// <summary>
        /// Проверка возможности сформировать документ
        /// </summary>
        /// <param name="docType">Тип документа для формирования</param>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="useCache">Использовать кэш для получения документов</param>
        public bool CanCreateDocumentOfType(ClaimWorkDocumentType docType, BaseClaimWork claimWork, bool useCache = false)
        {
            ArgumentChecker.NotNull(claimWork, "claimWork");

            if (claimWork is DebtorClaimWork)
            {
                if (docType == ClaimWorkDocumentType.Pretension)
                {
                    var configSection = this.configProv.Get<DebtorClaimWorkConfig>();
                    var debtorConfig = this.configProv.Get<RegOperatorConfig>().DebtorConfig.DebtorRegistryConfig;
                    var debtorClw = ((DebtorClaimWork) claimWork);

                    var config = debtorClw.DebtorType == DebtorType.Legal
                        ? configSection.Legal
                        : configSection.Individual;

                    if (config.Pretension.PretensionFormationKind == DocumentFormationType.NoForm)
                    {
                        return false;
                    }

                    if (config.Pretension.DebtSumType == DebtSumType.NotUsed)
                    {
                        return true;
                    }

                    var debtSum = debtorConfig.DebtorSumCheckerType == DebtorSumCheckerType.BaseAdnDecisionTariff
                        ? debtorClw.CurrChargeBaseTariffDebt + debtorClw.CurrChargeDecisionTariffDebt
                        : debtorClw.CurrChargeDebt;

                    var sum = config.Pretension.DebtSumType == DebtSumType.WithPenalty
                        ? debtSum + debtorClw.CurrPenaltyDebt
                        : debtSum;

                    return sum >= config.Pretension.DebtSum;
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}