namespace Bars.Gkh.RegOperator.DomainService.Petition
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    //todo отключен, тк пока не описана логика работы
    /// <summary>
    /// Сервис для работы с исковыми заявлениями
    /// </summary>
    /*public class PetitionService : IPetitionService
    {
        /// <summary>
        ///  Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Трансферы
        /// </summary>
        public IRepository<Transfer> TransferRepository { get; set; }

        /// <summary>
        /// Начисления
        /// </summary>
        public IRepository<PersonalAccountCharge> ChargeRepository { get; set; }

        /// <summary>
        /// ЛС на период
        /// </summary>
        public IRepository<PersonalAccountPeriodSummary> PeriodSummaryRepository { get; set; }

        /// <summary>
        /// Репозиторий периода начислений
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        public decimal GetChargeDebt(BasePersonalAccount account, DateTime documentDate)
        {
            var paymentPenaltiesDomain = this.Container.ResolveDomain<PaymentPenalties>();
            try
            {
                var countOfDelayDays = paymentPenaltiesDomain.GetAll()
                    .Where(x => x.DateStart <= documentDate)
                    .Where(x => x.DateEnd > documentDate || x.DateEnd == null)
                    .Where(x => x.DecisionType == (CrFundFormationDecisionType) account.Room.RealityObject.AccountFormationVariant)
                    .Select(x => x.Days)
                    .FirstOrDefault();

                var documentDay = documentDate.Day;
                var period = this.ChargePeriodRepository.GetPeriodByDate(documentDate);

                if (countOfDelayDays >= documentDay)
                {
                    var debtSum = this.GetPrevSaldoIn(account, period) 
                        - this.GetPrevPaymentsSum(account, period, documentDate)
                        + this.GetCancelsPaymentsSum(account, period);

                    return debtSum;
                }
                else
                {
                    var debtSum = this.GetPrevSaldoIn(account, period)
                        - this.GetPrevPaymentsSum(account, period, documentDate)
                        + this.GetCancelsPaymentsSum(account, period) 
                        + this.GetChargeSum(account, period);

                    return debtSum;
                }
            }
            finally
            {
                this.Container.Release(paymentPenaltiesDomain);
            }
        }

        private decimal GetPrevPaymentsSum(BasePersonalAccount account, ChargePeriod period, DateTime documentDate)
        {
            var inWalletGuids = new[]
            {
                account.BaseTariffWallet.WalletGuid,
                account.DecisionTariffWallet.WalletGuid

            };

            return this.TransferRepository.GetAll()
                .Where(x => inWalletGuids.Contains(x.TargetGuid))
                .Where(x => x.Operation.CanceledOperation == null)
                .Where(x => x.PaymentDate <= documentDate)
                .Where(x => x.OperationDate >= period.StartDate.AddMonths(-1))
                .Sum(x => (decimal?) x.Amount) ?? 0;
        }

        private decimal GetCancelsPaymentsSum(BasePersonalAccount account, ChargePeriod period)
        {
            var inWalletGuids = new[]
            {
                account.BaseTariffWallet.WalletGuid,
                account.DecisionTariffWallet.WalletGuid
            };

            return this.TransferRepository.GetAll()
                .Where(x => inWalletGuids.Contains(x.SourceGuid))
                .Where(x => x.Operation.CanceledOperation != null)
                .Where(x => x.PaymentDate >= period.StartDate)
                .Where(x => period.EndDate == null || x.PaymentDate <= period.EndDate)
                .Where(x => x.OperationDate >= period.StartDate && x.OperationDate <= period.EndDate)
                .Where(x => x.Reason.ToLower().Contains("отмена оплаты"))
                .Sum(x => (decimal?) x.Amount) ?? 0;
        }

        private decimal GetChargeSum(BasePersonalAccount account, ChargePeriod period)
        {
            return this.PeriodSummaryRepository.GetAll()
                .Where(x => x.PersonalAccount.Id == account.Id)
                .Where(x => x.Period.StartDate == period.StartDate.AddMonths(-1))
                .Select(x => x.RecalcByBaseTariff + x.RecalcByDecisionTariff + x.ChargeTariff)
                .Sum();
        }

        private decimal GetPrevSaldoIn(BasePersonalAccount account, ChargePeriod period)
        {
            return this.PeriodSummaryRepository.GetAll()
                .Where(x => x.PersonalAccount.Id == account.Id)
                .Where(x => x.Period.IsClosed)
                .Where(x => x.Period.StartDate < period.StartDate)
                .OrderByDescending(x => x.Period.StartDate)
                .Select(x => x.BaseTariffDebt + x.DecisionTariffDebt)
                .FirstOrDefault();
        }
    }*/
}