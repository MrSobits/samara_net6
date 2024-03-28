namespace Bars.Gkh.RegOperator.Domain.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.Period;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Services.DataContracts.Accounts;

    using NHibernate.Linq;

    /// <summary>
    /// Сервис для Ситуация по ЛС на период
    /// </summary>
    public class AccountPeriodSummaryService : IAccountPeriodSummaryService
    {
        public IDomainService<PersonalAccountPeriodSummary> SummaryDomain { get; set; }
        public IDomainService<PeriodSummaryBalanceChange> SaldoChangeDomain { get; set; }
        public IDomainService<RentPaymentIn> RentPaymentDomain { get; set; }
        public IDomainService<AccumulatedFunds> AccumFundsDomain { get; set; }
        public IDomainService<PersonalAccountPayment> AccountPaymentDomain { get; set; }
        public IDomainService<CashPaymentCenterPersAcc> CashPaymentCenterPersAccDomain { get; set; }
        public IDomainService<PersonalAccountPaymentTransfer> TransferDomainService { get; set; }
        public IPersonalAccountService PersonalAccountService { get; set; }
        public IChargePeriodService ChargePeriodService { get; set; }

        public IDomainService<RealityObjectChargeAccount> ChargeAccDomain { get; set; }

        /// <summary>
        /// Получить сводную информацию по лицевому счету
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="period">Период начислений</param>
        /// <param name="summary">Ситуация по ЛС на период</param>
        /// <returns></returns>
        public AccountPeriodSummary GetSummary(BasePersonalAccount account, ChargePeriod period,
            PersonalAccountPeriodSummary summary)
        {
            var startPenaltyDebt = this.GetStartPenaltyDebt(period, account);
            var tariff = this.GetLastTariff(account, period);
            var socialSupport = this.AccountPaymentDomain.GetAll()
                .Where(x => account.Id == x.BasePersonalAccount.Id)
                .Where(x => x.PaymentDate >= period.StartDate)
                .Where(x => x.PaymentDate <= period.EndDate)
                .Where(x => x.Type == PaymentType.SocialSupport)
                .SafeSum(x => x.Sum);
            var startDebt = summary.SaldoIn;
            var chargedSum = summary.ChargeTariff + this.GetSaldoChange(account, period);
            var chargedSolution = summary.ChargeTariff - summary.ChargedByBaseTariff;
            var paidSum = summary.TariffPayment + this.GetAdditionalPayments(account, period);
            var endOverPay = startDebt
                + chargedSum
                + summary.RecalcByBaseTariff
                + summary.Penalty
                + summary.RecalcByPenalty
                - summary.PenaltyPayment
                - socialSupport - paidSum; // TODO fix recalc

            var penaltyDebt = summary.PenaltyDebt
             + summary.Penalty
             + summary.RecalcByPenalty
             - summary.PenaltyPayment;

            var realtyId = account.Room.RealityObject.Id;

            var entity = this.ChargeAccDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == realtyId);
            
            var paidTotalOnHouse = entity.Operations.SafeSum(y => y.PaidTotal + y.PaidPenalty);            

            var houseTotalPaid = this.SummaryDomain.GetAll()
                .Where(x => x.Period.Id == period.Id)
                .Where(x => x.PersonalAccount.Room.RealityObject.Id == realtyId).ToList()
                .Sum(x => x.TariffPayment + this.GetAdditionalPayments(x.PersonalAccount, x.Period));

            // Получаем РКЦ лицевика
            var cashPayCenter = this.CashPaymentCenterPersAccDomain.GetAll()
                .Where(x => x.PersonalAccount.Id == account.Id)
                .Select(x => new
                {
                    x.CashPaymentCenter.Identifier,
                    x.CashPaymentCenter.Contragent.Name
                })
                .FirstOrDefault();
            string typeOwnership = string.Empty;
            switch (account.Room.OwnershipType)
            {
                case Gkh.Enums.RoomOwnershipType.Commerse:
                    typeOwnership = "Коммерческая";
                    break;
                case Gkh.Enums.RoomOwnershipType.Federal:
                    typeOwnership = "Федеральная";
                    break;
                case Gkh.Enums.RoomOwnershipType.Goverenment:
                    typeOwnership = "Государственная";
                    break;
                case Gkh.Enums.RoomOwnershipType.Mixed:
                    typeOwnership = "Смешанная";
                    break;
                case Gkh.Enums.RoomOwnershipType.Municipal:
                    typeOwnership = "Муниципальная";
                    break;
                case Gkh.Enums.RoomOwnershipType.MunicipalAdm:
                    typeOwnership = "Муниципальная";
                    break;
                case Gkh.Enums.RoomOwnershipType.NotSet:
                    typeOwnership = "Не определена";
                    break;
                case Gkh.Enums.RoomOwnershipType.Private:
                    typeOwnership = "Частная";
                    break;
                case Gkh.Enums.RoomOwnershipType.Regional:
                    typeOwnership = "Областная";
                    break;
            }

            return new AccountPeriodSummary
            {
                ResponseCode = 0,
                Address = string.Format("{0}, кв. {1}", account.Room.RealityObject.Address, account.Room.RoomNum),
                BeginDebt = startDebt > 0 ? startDebt : 0,
                BeginOverPay = startDebt < 0 ? -startDebt : 0,
                ChargedSum = chargedSum,
                ChargedBase = summary.ChargedByBaseTariff + summary.BaseTariffChange,
                ChargedSolution = chargedSolution + summary.DecisionTariffChange,
                PenaltyCharged = summary.Penalty + summary.PenaltyChange,
                Recalc = summary.RecalcByBaseTariff,
                RecalcSolution = summary.RecalcByDecisionTariff,
                RecalcPenalty = summary.RecalcByPenalty,
                OwnerName = account.AccountOwner.Name,
                PersonalAccountNum = account.PersonalAccountNum,
                ExNum = account.PersAccNumExternalSystems,
                Message = string.Empty,
                Tariff = tariff,
                TotalArea = account.Room.Area,
                MonthCharge = tariff * account.Room.Area * account.AreaShare,
                PenaltyDebt = penaltyDebt,
                OwnershipType = typeOwnership,
                PaidSum = paidSum,
                PaidPenalty = summary.PenaltyPayment,
                PaidSoiution = summary.TariffDecisionPayment,
                PaidBase = summary.TariffPayment,
                PenaltySum = summary.Penalty + summary.RecalcByPenalty,
                EndDebt = (endOverPay > 0 ? endOverPay : 0) - penaltyDebt,
                EndOverPay = endOverPay < 0 ? -endOverPay : 0,
                SocialSupport = socialSupport,
                HouseTotalPaid = houseTotalPaid,
                RkcIdentifier = cashPayCenter != null ? cashPayCenter.Identifier : string.Empty,
                RkcName = cashPayCenter != null ? cashPayCenter.Name : string.Empty,
                IncreaseAccountPaidMonthSum = this.GetIncreaseAccountPaidMonthSum(account, period),
                SaldoIn = summary.SaldoIn,
                SaldoOut = summary.SaldoOut,
                ChargeTotal = summary.ChargeTariff + summary.Penalty + summary.RecalcByBaseTariff + summary.RecalcByPenalty,
                PaidTotalOnHouse = paidTotalOnHouse
            };
        }

        private decimal GetStartPenaltyDebt(ChargePeriod period, BasePersonalAccount account)
        {
            var summaries = this.SummaryDomain.GetAll()
                .Where(x => x.PersonalAccount.Id == account.Id)
                .Where(x => x.Period.StartDate < period.StartDate)
                .ToList();

            return !summaries.Any() ? 0m : summaries.Sum(x => x.Penalty - x.PenaltyPayment);
        }

        private decimal GetSaldoChange(BasePersonalAccount account, ChargePeriod period)
        {
            var saldoChages = this.SaldoChangeDomain.GetAll()
                .Where(x => x.PeriodSummary.PersonalAccount.Id == account.Id)
                .Where(x => x.OperationDate >= period.StartDate && x.OperationDate <= period.EndDate)
                .SafeSum(x => (x.CurrentValue - x.NewValue));
            return saldoChages;
        }

        private decimal GetLastTariff(BasePersonalAccount account, ChargePeriod period)
        {
            var ro = account.Room.Return(x => x.RealityObject);
            var roId = ro.Return(x => x.Id);
            var muId = ro.Return(x => x.Municipality).Return(x => x.Id);
            var settlId = ro.Return(x => x.MoSettlement).Return(x => x.Id);

            var end = period.EndDate ?? period.StartDate.AddMonths(1).AddDays(-1);
            return this.PersonalAccountService.GetTariff(roId, muId, settlId, end);
        }

        private decimal GetAdditionalPayments(BasePersonalAccount account, ChargePeriod period)
        {
            var rentPayments = this.RentPaymentDomain.GetAll()
                .Where(x => x.Account.Id == account.Id)
                .Where(x => x.OperationDate >= period.StartDate)
                .Where(x => x.OperationDate <= period.EndDate)
                .Select(x => new { AccId = x.Account.Id, x.Sum })
                .ToArray();

            var accumulatedFunds = this.AccumFundsDomain.GetAll()
                .Where(x => x.Account.Id == account.Id)
                .Where(x => x.OperationDate >= period.StartDate)
                .Where(x => x.OperationDate <= period.EndDate)
                .Select(x => new { AccId = x.Account.Id, x.Sum })
                .ToArray();

            return rentPayments.Union(accumulatedFunds).Sum(x => x.Sum);
        }

        private decimal GetIncreaseAccountPaidMonthSum(BasePersonalAccount account, ChargePeriod period)
        {
            var startDateOfFirstPeriod = this.ChargePeriodService.GetStartDateOfFirstPeriod();

            var walletGuids = new[]
            {
                account.BaseTariffWallet.WalletGuid,
                account.DecisionTariffWallet.WalletGuid,
                account.PenaltyWallet.WalletGuid
            };

            var inTransfersSum = this.TransferDomainService.GetAll()
                .Fetch(x => x.Operation)
                .Where(x => walletGuids.Contains(x.TargetGuid))
                .Where(x => x.Operation.CanceledOperation == null)
                .Where(x => !x.Operation.IsCancelled)
                .Where(x => x.PaymentDate.Date >= startDateOfFirstPeriod && (period.EndDate == null || x.PaymentDate.Date <= period.EndDate))
                .Where(x => x.Amount != 0)
                .Sum(x => (decimal?) x.Amount) ?? 0;

            var refundTransfersSum = this.TransferDomainService.GetAll()
                .Fetch(x => x.Operation)
                .Where(x => walletGuids.Contains(x.SourceGuid))
                .Where(x => x.Operation.CanceledOperation == null)
                .Where(x => !x.Operation.IsCancelled)
                .Where(x => x.PaymentDate.Date >= startDateOfFirstPeriod && (period.EndDate == null || x.PaymentDate.Date <= period.EndDate))
                .Where(x => x.Reason.ToLower().Contains("возврат"))
                .Where(x => x.Amount != 0)
                .Sum(x => (decimal?) x.Amount) ?? 0;

            return inTransfersSum - refundTransfersSum;
        }
    }
}