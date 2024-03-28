namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.ClaimWork.Dto;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    using Castle.Core;
    using Castle.Windsor;

    public class RestructDebtScheduleService : IRestructDebtScheduleService, IInitializable
    {
        private int legalAllowDelay = 0;
        private int individualAllowDelay = 0;
        private const int TakePortion = 2500;

        public IWindsorContainer Container { get; set; }
        public IDomainService<PersonalAccountPaymentTransfer> TransferDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PeriodSummaryDomain { get; set; }
        public IDomainService<RecalcHistory> RecalcHistoryDomain { get; set; }
        public IDomainService<RestructDebtSchedule> RestructDebtScheduleDomain { get; set; }
        public IRepository<RestructDebtScheduleDetail> RestructDebtScheduleDetailRepos { get; set; }

        /// <inheritdoc />
        public IDataResult CreateRestructSchedule(BaseParams baseParams)
        {
            var restructDebtDomain = this.Container.ResolveDomain<RestructDebt>();

            var configProvider = this.Container.Resolve<IGkhConfigProvider>();
            using (this.Container.Using(restructDebtDomain, configProvider))
            {
                var restructDebtScheduleParam = baseParams.GetSaveEntity<RestructDebtScheduleParam>();

                if (restructDebtScheduleParam == null || !restructDebtScheduleParam.IsValid())
                {
                    return BaseDataResult.Error("Не переданы параметры графика реструктуризации");
                }
                var startDate = restructDebtScheduleParam.StartDate.Value;
                var endDate = restructDebtScheduleParam.EndDate.Value;

                if (startDate > endDate)
                {
                    return BaseDataResult.Error("Не верно указан период");
                }

                var monthCount = startDate.GetDiffMonthCount(endDate) + 1;

                var toSave = new List<RestructDebtSchedule>(monthCount);

                var restructDebt = restructDebtDomain.Get(restructDebtScheduleParam.RestructDebt);
                var personalAccount = restructDebtScheduleParam.GetPersonalAccount();
                var totalDebtSum = restructDebtScheduleParam.TotalDebtSum.Value;

                var planedPaymentDay = new DateTime(startDate.Year, startDate.Month, restructDebtScheduleParam.PaymentDay.Value);
                var planedPaymentSum = (restructDebtScheduleParam.TotalDebtSum.Value / monthCount).RegopRoundDecimal(2);

                if (planedPaymentDay < (restructDebt.DocumentDate ?? DateTime.MaxValue))
                {
                    return BaseDataResult.Error($"Дата начала графика реструктуризации {planedPaymentDay:dd.MM.yyyy} не может быть ранее даты документа");
                }

                var deltaError = planedPaymentSum * monthCount - totalDebtSum;
                var penny = -0.01m * Math.Sign(deltaError);

                var correctCount = (int)(deltaError * 100);
                var startCorrect = correctCount < 0 ? 0 : monthCount - correctCount;
                var endCorrect = correctCount < 0 ? -correctCount : monthCount;

                for (int i = 0; i < monthCount; i++)
                {
                    toSave.Add(new RestructDebtSchedule
                    {
                        RestructDebt = restructDebt,
                        PersonalAccount = personalAccount,
                        TotalDebtSum = totalDebtSum,
                        PlanedPaymentDate = planedPaymentDay.AddMonths(i),
                        PlanedPaymentSum = planedPaymentSum + (startCorrect <= i && i < endCorrect ? penny : 0)
                    });
                }

                TransactionHelper.InsertInManyTransactions(this.Container, toSave, useStatelessSession: true);

                return new BaseDataResult();
            }
        }

        /// <inheritdoc />
        public IDataResult ListAccountInfo(BaseParams baseParams)
        {
            var domainService = this.Container.Resolve<IDomainService<ClaimWorkAccountDetail>>();
            var viewModel = this.Container.Resolve<IViewModel<ClaimWorkAccountDetail>>();

            using (this.Container.Using(domainService, viewModel))
            {
                var restructDebtId = baseParams.Params.GetAsId("restructDebtId");

                if (restructDebtId == 0)
                {
                    return BaseDataResult.Error("Не указан идентификатор документа реструктуризации");
                }

                baseParams.Params["excludeAccountIds"] = this.RestructDebtScheduleDomain.GetAll()
                    .Where(x => x.RestructDebt.Id == restructDebtId)
                    .Select(x => x.PersonalAccount.Id)
                    .Distinct()
                    .ToArray();

                return viewModel.List(domainService, baseParams);
            }
        }

        /// <inheritdoc />
        public void SumsUpdate(IQueryable<DebtorClaimWork> debtorClaimWorks)
        {
            var persAccDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var clwPersAccDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();

            var scheduleDetailRepos = this.Container.ResolveRepository<RestructDebtScheduleDetail>();

            using (this.Container.Using(persAccDomain, clwPersAccDomain, scheduleDetailRepos))
            {
                var clwPersAccQuery = clwPersAccDomain.GetAll()
                    .Where(x => debtorClaimWorks.Any(y => y.Id == x.ClaimWork.Id))
                    .OrderBy(x => x.PersonalAccount.Id);

                var persAccCount = clwPersAccQuery.Count();

                for (int skip = 0; skip < persAccCount; skip += RestructDebtScheduleService.TakePortion)
                {
                    var clwPersAccPortion = clwPersAccQuery
                        .Skip(skip)
                        .Take(RestructDebtScheduleService.TakePortion);

                    var restructDebtSchedules = this.RestructDebtScheduleDomain.GetAll()
                        .Where(x => x.RestructDebt.DocumentState == RestructDebtDocumentState.Active)
                        .Where(x => clwPersAccPortion.Any(y => y.PersonalAccount == x.PersonalAccount))
                        .ToList();

                    if (restructDebtSchedules.IsEmpty())
                    {
                        continue;
                    }

                    var baseWalletGuids = clwPersAccPortion.Select(x => new
                        {
                            BaseTariffWallet = x.PersonalAccount.BaseTariffWallet.WalletGuid,
                            DecisionTariffWallet = x.PersonalAccount.DecisionTariffWallet.WalletGuid,
                            PenaltyWallet = x.PersonalAccount.PenaltyWallet.WalletGuid
                        })
                        .AsEnumerable()
                        .SelectMany(x => new[]
                        {
                            x.BaseTariffWallet,
                            x.DecisionTariffWallet,
                            x.PenaltyWallet
                        })
                        .ToHashSet();

                    var amicAgrWalletGuids = clwPersAccPortion.Select(x => x.PersonalAccount.RestructAmicableAgreementWallet.WalletGuid)
                        .ToHashSet();

                    var persAccOwnerTypeDict = clwPersAccPortion.Select(x => new
                    {
                        x.PersonalAccount.Id,
                        x.PersonalAccount.AccountOwner.OwnerType
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.OwnerType)
                    .ToDictionary(x => x.Key, x => x.First());

                    var basePaymentInfo =
                        this.GetPaymentInfo(restructDebtSchedules.Where(x => x.RestructDebt.DocumentType == ClaimWorkDocumentType.RestructDebt),
                            persAccOwnerTypeDict.Keys,
                            baseWalletGuids);

                    var amicAgrPaymentInfo =
                        this.GetPaymentInfo(restructDebtSchedules.Where(x => x.RestructDebt.DocumentType == ClaimWorkDocumentType.RestructDebtAmicAgr),
                            persAccOwnerTypeDict.Keys,
                            amicAgrWalletGuids,
                            false);

                    if (basePaymentInfo == null && amicAgrPaymentInfo == null)
                    {
                        continue;
                    }

                    var debtSchedules = restructDebtSchedules.GroupBy(x => x.PersonalAccount.Id)
                        .ToDictionary(x => x.Key, x => x.OrderBy(z => z.PlanedPaymentDate).ToList());

                    var detailToSave = this.InitRestructDebtScheduleDetail(restructDebtSchedules.Select(z => z.Id));
                    foreach (var scheduleByAccount in debtSchedules)
                    {
                        scheduleByAccount.Value.ForEach(x =>
                        {
                            x.PaymentSum = 0;
                            x.PaymentDate = null;
                        });
                        var delayDays = this.GetDelayDays(persAccOwnerTypeDict.Get(scheduleByAccount.Key));

                        var transfers = basePaymentInfo?.TransferDict.Get(scheduleByAccount.Key);
                        if (transfers != null)
                        {
                            var periodSum = basePaymentInfo.PeriodSummaryDict?.Get(scheduleByAccount.Key);
                            var recalc = basePaymentInfo.RecalcHistoryDict?.Get(scheduleByAccount.Key);

                            this.DistributeTransfers(transfers,
                                scheduleByAccount.Value.Where(x => x.RestructDebt.DocumentType == ClaimWorkDocumentType.RestructDebt),
                                detailToSave, delayDays, periodSum, recalc);
                        }

                        var amicAgrTransfers = amicAgrPaymentInfo?.TransferDict.Get(scheduleByAccount.Key);
                        if (amicAgrTransfers != null)
                        {
                            this.DistributeTransfers(amicAgrTransfers,
                                scheduleByAccount.Value.Where(x => x.RestructDebt.DocumentType == ClaimWorkDocumentType.RestructDebtAmicAgr),
                                detailToSave, delayDays);
                        }
                    }

                    TransactionHelper.InsertInManyTransactions(this.Container,
                        debtSchedules.SelectMany(x => x.Value).ToList(),
                        useStatelessSession: true);
                    TransactionHelper.InsertInManyTransactions(this.Container, detailToSave, useStatelessSession: true);
                }
            }
        }

        /// <inheritdoc />
        public void Initialize()
        {
            var configProvider = this.Container.Resolve<IGkhConfigProvider>();
            using (this.Container.Using())
            {
                var debtorConfig = configProvider.Get<DebtorClaimWorkConfig>();
                this.legalAllowDelay = debtorConfig?.Legal.RestructDebt.AllowDelayPaymentDays ?? 0;
                this.individualAllowDelay = debtorConfig?.Individual.RestructDebt.AllowDelayPaymentDays ?? 0;
            }
        }

        private List<RestructDebtScheduleDetail> InitRestructDebtScheduleDetail(IEnumerable<long> restructDebtSchedules)
        {
            this.RestructDebtScheduleDetailRepos.GetAll()
                .WhereContainsBulked(x => x.ScheduleRecord.Id, restructDebtSchedules)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => this.RestructDebtScheduleDetailRepos.Delete(x));

            return new List<RestructDebtScheduleDetail>();
        }

        private int GetDelayDays(PersonalAccountOwnerType ownerType)
        {
            return ownerType == PersonalAccountOwnerType.Legal
                ? this.legalAllowDelay
                : ownerType == PersonalAccountOwnerType.Individual
                    ? this.individualAllowDelay
                    : 0;
        }

        private void DistributeTransfers(ICollection<TransferDto> transfers,
            IEnumerable<RestructDebtSchedule> scheduleByAccount,
            List<RestructDebtScheduleDetail> detailToSave,
            int delayDays,
            Dictionary<DateTime, decimal> periodSumDict = null,
            Dictionary<DateTime, decimal> recalcDict = null)
        {
            if (transfers == null)
            {
                return;
            }

            foreach (var transfer in transfers)
            {
                var prevPeriodDate = transfer.PaymentDate.GetFirstDayOfMonth(-1);
                var charge = periodSumDict?.Get(prevPeriodDate) ?? 0 + recalcDict?.Get(prevPeriodDate) ?? 0;
                var amount = transfer.Amount - charge;

                if (amount <= 0)
                {
                    continue;
                }

                foreach (var schedule in scheduleByAccount
                    .Where(x => x.PaymentSum < x.PlanedPaymentSum))
                {
                    var transferSum = schedule.PlanedPaymentSum - schedule.PaymentSum;
                    amount -= transferSum;

                    schedule.PaymentDate = transfer.PaymentDate;

                    if (amount >= 0)
                    {
                        schedule.PaymentSum = schedule.PlanedPaymentSum;
                    }
                    else
                    {
                        schedule.PaymentSum = schedule.PlanedPaymentSum + amount;
                        transferSum += amount;
                    }

                    schedule.IsExpired = schedule.PlanedPaymentDate.AddDays(delayDays) < transfer.PaymentDate
                        || schedule.PaymentSum < schedule.PlanedPaymentSum;

                    detailToSave.Add(new RestructDebtScheduleDetail
                    {
                        ScheduleRecord = schedule,
                        TransferId = transfer.Id,
                        Sum = transferSum
                    });

                    if (amount <= 0)
                    {
                        break;
                    }
                }
            }
        }

        private Dictionary<long, List<TransferDto>> GetTransfers(ICollection<long> persAccOwnerIds, ICollection<string> walletGuids, DateTime startDate, DateTime endDate)
        {
            if (!startDate.IsValid() || !endDate.IsValid())
            {
                return new Dictionary<long, List<TransferDto>>();
            }

            return this.TransferDomain.GetAll()
                .WhereContainsBulked(x => x.Owner.Id, persAccOwnerIds)
                .Where(x => x.IsAffect && !x.Operation.IsCancelled)
                .Where(x => x.Operation.CanceledOperation == null)
                .Where(x => walletGuids.Contains(x.TargetGuid))
                .Where(x => x.PaymentDate >= startDate)
                .Where(x => x.PaymentDate <= endDate)
                .Select(x => new TransferDto
                {
                    Id = x.Id,
                    PaymentDate = x.PaymentDate,
                    Amount = walletGuids.Contains(x.SourceGuid)
                        ? -1 * x.Amount
                        : x.Amount,
                    AccountId = x.Owner.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.AccountId)
                .ToDictionary(x => x.Key, x => x.OrderBy(z => z.PaymentDate).ToList());
        }

        private Dictionary<long, Dictionary<DateTime, decimal>> GetPeriodSummary(ICollection<long> persAccOwnerIds, DateTime startDate, DateTime endDate)
        {
            return this.PeriodSummaryDomain.GetAll()
                .WhereContainsBulked(x => x.PersonalAccount.Id, persAccOwnerIds)
                .Where(x => x.Period.StartDate >= startDate)
                .Where(x => x.Period.StartDate <= endDate)
                .Where(x => x.Period.IsClosed)
                .Select(x => new
                {
                    x.PersonalAccount.Id,
                    Charge = x.ChargeTariff + x.Penalty,
                    x.Period.StartDate
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key,
                    x => x.ToDictionary(y => y.StartDate, y => y.Charge));
        }

        private Dictionary<long, Dictionary<DateTime, decimal>> GetRegalcHistory(ICollection<long> persAccOwnerIds, DateTime startDate, DateTime endDate)
        {
            if (!startDate.IsValid() || !endDate.IsValid())
            {
                return null;
            }

            return this.RecalcHistoryDomain.GetAll()
                .WhereContainsBulked(x => x.PersonalAccount.Id, persAccOwnerIds)
                .Where(x => x.RecalcPeriod.StartDate >= startDate)
                .Where(x => x.RecalcPeriod.StartDate <= endDate)
                .Where(x => x.CalcPeriod.IsClosed)
                .Where(x => x.RecalcSum != 0)
                .Select(x => new
                {
                    x.PersonalAccount.Id,
                    x.RecalcSum,
                    x.RecalcPeriod.StartDate
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key,
                    x => x.GroupBy(y => y.StartDate, y => y.RecalcSum)
                        .ToDictionary(y => y.Key, y => y.Sum()));
        }

        private PaymentsInfo GetPaymentInfo(IEnumerable<RestructDebtSchedule> restructDebtSchedules, ICollection<long> persAccOwnerIds, ICollection<string> walletGuids, bool getCharges = true)
        {
            if (restructDebtSchedules.IsEmpty())
            {
                return null;
            }

            var startDate = restructDebtSchedules.Where(x => x.RestructDebt.DocumentDate.HasValue)
                .Select(x => x.RestructDebt.DocumentDate.Value)
                .Min();
            var endDate = restructDebtSchedules.Select(x => x.PlanedPaymentDate).Max();
            var prevPeriodDate = startDate.IsValid() ? startDate.GetFirstDayOfMonth(-1) : startDate;

            var transferDict = this.GetTransfers(persAccOwnerIds,
                walletGuids, startDate, endDate);

            if (transferDict.Count == 0)
            {
                return null;
            }

            var result = new PaymentsInfo
            {
                TransferDict = transferDict
            };

            if (getCharges)
            {
                result.PeriodSummaryDict = this.GetPeriodSummary(persAccOwnerIds, prevPeriodDate, endDate);
                result.RecalcHistoryDict = this.GetRegalcHistory(persAccOwnerIds, prevPeriodDate, endDate);
            }

            return result;
        }

        private class TransferDto
        {
            public long Id { get; set; }
            public DateTime PaymentDate { get; set; }
            public decimal Amount { get; set; }
            public long AccountId { get; set; }
        }

        private class PaymentsInfo
        {
            public Dictionary<long, List<TransferDto>> TransferDict { get; set; }
            public Dictionary<long, Dictionary<DateTime, decimal>> PeriodSummaryDict { get; set; }
            public Dictionary<long, Dictionary<DateTime, decimal>> RecalcHistoryDict { get; set; }
        }
    }
}