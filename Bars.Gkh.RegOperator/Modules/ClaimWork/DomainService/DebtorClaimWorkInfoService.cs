namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Repository;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Entities;
    using Enums;

    /// <summary>
    /// Сервис для получения информации о должнике
    /// </summary>
    public class DebtorClaimWorkInfoService : IClaimWorkInfoService
    {
        public IWindsorContainer Container { get; set; }

        public ILawsuitRepository LawsuitRepo { get; set; }
        public IRepository<PretensionClw> PretensionRepo { get; set; }

        /// <inheritdoc />
        public void GetInfo(BaseClaimWork claimWork, DynamicDictionary resultDict)
        {
            var debtorClaimWork = claimWork as DebtorClaimWork;
            if (claimWork.ClaimWorkTypeBase != ClaimWorkTypeBase.Debtor || debtorClaimWork == null  )
            {
                return;
            }

            resultDict.Add("CurrChargeBaseTariffDebt", debtorClaimWork.CurrChargeBaseTariffDebt);
            resultDict.Add("CurrChargeDecisionTariffDebt", debtorClaimWork.CurrChargeDecisionTariffDebt);
            resultDict.Add("CurrChargeDebt", debtorClaimWork.CurrChargeDebt);
            resultDict.Add("CurrPenaltyDebt", debtorClaimWork.CurrPenaltyDebt);
        }

        /// <inheritdoc />
        public ListDataResult GetDebtPersAccPayments(BaseParams baseParams)
        {
            var debtorClaimWorkDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var transferDomain = this.Container.ResolveDomain<PersonalAccountPaymentTransfer>();

            try
            {
                var info = this.GetInfoForDebtPayment(baseParams);

                if (info == null)
                {
                    return null;
                }

                if (!info.DateStart.HasValue)
                {
                    return null;
                }
                var loadParams = baseParams.GetLoadParam();

                var walletGuids = debtorClaimWorkDomain.GetAll()
                    .Where(x => x.ClaimWork.Id == info.ClaimWorkId)
                    .Select(x => new
                        {
                            BaseTariffWallet = x.PersonalAccount.BaseTariffWallet.WalletGuid,
                            DecisionTariffWallet = x.PersonalAccount.DecisionTariffWallet.WalletGuid,
                            PenaltyWallet = x.PersonalAccount.PenaltyWallet.WalletGuid
                        })
                    .AsEnumerable()
                    .SelectMany(x => new []
                    {
                        x.BaseTariffWallet,
                        x.DecisionTariffWallet,
                        x.PenaltyWallet
                    })
                    .ToHashSet();

                var startDate = info.DateStart;
                var endDate = info.DateEnd.GetValueOrDefault(DateTime.MaxValue);

                return transferDomain.GetAll()
                    .Where(x => x.IsAffect && !x.Operation.IsCancelled)
                    .Where(x => x.Operation.CanceledOperation == null)
                    .Where(x => walletGuids.Contains(x.TargetGuid) || walletGuids.Contains(x.SourceGuid))
                    .Where(x => startDate <= x.PaymentDate && x.PaymentDate <= endDate)
                    .Select(x => new
                    {
                        x.Id,
                        x.PaymentDate,
                        PaymentType = x.Reason,
                        Sum = walletGuids.Contains(x.SourceGuid)
                            ? -1 * x.Amount
                            : x.Amount,
                    })
                    .ToListDataResult(loadParams);
            }
            finally
            {
                this.Container.Release(debtorClaimWorkDomain);
                this.Container.Release(transferDomain);
            }
        }

        private InfoForDebtPayment GetInfoForDebtPayment(BaseParams baseParams)
        {
            var pretensionId = baseParams.Params.GetAs<long>("pretensionId");

            var pretension = this.PretensionRepo.Get(pretensionId);
            if (pretension == null)
            {
                return null;
            }

            var lawSuit = this.LawsuitRepo.FindPetitionOrCourtOrderClaim(pretension.ClaimWork);

            var lawSuitBidDate = lawSuit?.DocumentDate;

            return new InfoForDebtPayment
            {
                ClaimWorkId = pretension.ClaimWork.Id,
                DateStart = pretension.DocumentDate,
                DateEnd = lawSuitBidDate ?? DateTime.Today
            };
        }

        public class InfoForDebtPayment
        {
            public long ClaimWorkId { get; set; }
            public DateTime? DateStart { get; set; }
            public DateTime? DateEnd { get; set; }
        }
    }
}