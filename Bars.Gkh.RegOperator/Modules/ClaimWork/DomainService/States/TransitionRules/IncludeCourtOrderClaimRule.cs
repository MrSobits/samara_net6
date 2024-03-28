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
    /// Проверка включения в список возможных документов заявления о выдаче приказа
    /// </summary>
    public class IncludeCourtOrderClaimRule : IClwTransitionRule
    {
        private readonly IGkhConfigProvider configProv;

        public IncludeCourtOrderClaimRule(IWindsorContainer container)
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

            var debtorClw = claimWork as DebtorClaimWork;
            if (debtorClw != null && docType == ClaimWorkDocumentType.CourtOrderClaim)
            {
                var configSection = this.configProv.Get<DebtorClaimWorkConfig>();
                var debtorConfig = this.configProv.Get<RegOperatorConfig>().DebtorConfig.DebtorRegistryConfig;

                var config = debtorClw.DebtorType == DebtorType.Legal
                    ? configSection.Legal
                    : configSection.Individual;

                if (config.CourtOrderClaim.CourtOrderClaimFormationKind == DocumentFormationKind.NoForm)
                {
                    return false;
                }

                if (config.CourtOrderClaim.CourtOrderClaimDebtSumType == DebtSumType.NotUsed)
                {
                    return true;
                }

                var debtSum = debtorConfig.DebtorSumCheckerType == DebtorSumCheckerType.BaseAdnDecisionTariff
                    ? debtorClw.CurrChargeBaseTariffDebt + debtorClw.CurrChargeDecisionTariffDebt
                    : debtorClw.CurrChargeDebt;

                var sum = config.CourtOrderClaim.CourtOrderClaimDebtSumType == DebtSumType.WithPenalty
                    ? debtSum + debtorClw.CurrPenaltyDebt
                    : debtSum;

                return sum >= config.CourtOrderClaim.CourtOrderClaimDebtSum;
            }

            return true;
        }

        #endregion
    }
}