namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States.TransitionRules
{
    using System.Linq;

    using Bars.B4.DataAccess;
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
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Проверка включения типа <see cref="ClaimWorkDocumentType.Notification"/>
    /// </summary>
    public class IncludeNotificationRule : IClwTransitionRule
    {
        private readonly IGkhConfigProvider configProv;

        public IncludeNotificationRule(IWindsorContainer container)
        {
            this.configProv = container.Resolve<IGkhConfigProvider>();
        }

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
            if (debtorClw != null && docType == ClaimWorkDocumentType.Notification)
            {
                var configSection = this.configProv.Get<DebtorClaimWorkConfig>();
                var debtorConfig = this.configProv.Get<RegOperatorConfig>().DebtorConfig.DebtorRegistryConfig;

                var config = debtorClw.DebtorType == DebtorType.Legal
                    ? configSection.Legal
                    : configSection.Individual;

                if (config.DebtNotification.NotifFormationKind == DocumentFormationType.NoForm)
                {
                    return false;
                }

                if (config.DebtNotification.NotifDebtSumType == NotifSumType.Ignore)
                {
                    return true;
                }

                var debtSum = debtorConfig.DebtorSumCheckerType == DebtorSumCheckerType.BaseAdnDecisionTariff
                        ? debtorClw.CurrChargeBaseTariffDebt + debtorClw.CurrChargeDecisionTariffDebt
                    : debtorClw.CurrChargeDebt;

                var sum = config.DebtNotification.NotifDebtSumType == NotifSumType.WithPenalty
                    ? debtSum + debtorClw.CurrPenaltyDebt
                    : debtSum;

                return sum >= config.DebtNotification.NotifDebtSum;
            }
            return true;
        }
    }
}