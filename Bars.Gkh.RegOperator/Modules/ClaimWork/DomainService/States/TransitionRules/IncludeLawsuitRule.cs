namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States.TransitionRules
{
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ConfigSections.RegOperator.Debtor;
    using Bars.Gkh.Enums;
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
    public class IncludeLawsuitRule : IClwTransitionRule
    {
        private readonly IGkhConfigProvider configProv;
        private readonly IRepository<CourtOrderClaim> courtClaimRepo;
        private readonly IDebtorStateCache debtorStateCache;

        public IncludeLawsuitRule(IWindsorContainer container)
        {
            this.debtorStateCache = container.Resolve<IDebtorStateCache>();
            this.courtClaimRepo = container.ResolveRepository<CourtOrderClaim>();
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

            if (claimWork is DebtorClaimWork
                && docType == ClaimWorkDocumentType.Lawsuit)
            {
                CourtOrderClaim courtClaim;

                if (useCache)
                {
                    courtClaim = this.debtorStateCache.GetCourtClaim(claimWork);
                }
                else
                {
                    courtClaim = this.courtClaimRepo.GetAll()
                        .FirstOrDefault(x => x.ClaimWork.Id == claimWork.Id);
                }

                if (courtClaim != null)
                {
                    if (!(courtClaim.ResultConsideration == LawsuitResultConsideration.Denied
                        || courtClaim.ObjectionArrived == YesNo.Yes || courtClaim.IsDeterminationCancel))
                    {
                        return false;
                    }
                }

                var configSection = this.configProv.Get<DebtorClaimWorkConfig>();
                var debtorConfig = this.configProv.Get<RegOperatorConfig>().DebtorConfig.DebtorRegistryConfig;
                var debtorClw = ((DebtorClaimWork)claimWork);

                var config = debtorClw.DebtorType == DebtorType.Legal
                    ? configSection.Legal
                    : configSection.Individual;

                if (config.Lawsuit.LawsuitFormationKind == DocumentFormationKind.NoForm)
                {
                    return false;
                }

                if (config.Lawsuit.LawsuitDebtSumType == DebtSumType.NotUsed)
                {
                    return true;
                }

                var debtSum = debtorConfig.DebtorSumCheckerType == DebtorSumCheckerType.BaseAdnDecisionTariff
                    ? debtorClw.CurrChargeBaseTariffDebt + debtorClw.CurrChargeDecisionTariffDebt
                    : debtorClw.CurrChargeDebt;

                var sum = config.CourtOrderClaim.CourtOrderClaimDebtSumType == DebtSumType.WithPenalty
                    ? debtSum + debtorClw.CurrPenaltyDebt
                    : debtSum;

                return sum >= config.Lawsuit.LawsuitDebtSum;
            }

            return true;
        }

        #endregion
    }
}