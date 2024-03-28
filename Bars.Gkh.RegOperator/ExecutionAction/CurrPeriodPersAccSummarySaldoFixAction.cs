namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.Repositories.ChargePeriod;

    public class CurrPeriodPersAccSummarySaldoFixAction : BaseExecutionAction
    {
        public static string Code = "CurrPeriodPersAccSummarySaldoFixAction";

        public override string Description => "Присвоение значения исходящего сальдо предыдущего периода входящему сальдо текущего периода";

        public override string Name => "Присвоение значения исходящего сальдо предыдущего периода входящему сальдо текущего периода";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var chargePeriodRepo = this.Container.Resolve<IChargePeriodRepository>();
            var persAccPeriodSummaryDomain = this.Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var saldoChangeDomain = this.Container.ResolveDomain<PeriodSummaryBalanceChange>();
            var rentPaymentDomain = this.Container.ResolveDomain<RentPaymentIn>();

            try
            {
                var period = chargePeriodRepo.GetCurrentPeriod();

                if (period == null)
                {
                    return new BaseDataResult();
                }

                var oldPeriod = chargePeriodRepo.GetPeriodByDate(period.StartDate.AddDays(-1));

                if (oldPeriod == null)
                {
                    return new BaseDataResult();
                }

                var saldoChangeQuery = saldoChangeDomain.GetAll();
                var rentPaymentInQuery = rentPaymentDomain.GetAll();
                ;

                var newPeriodSummaries = persAccPeriodSummaryDomain.GetAll()
                    .Where(x => x.Period.Id == period.Id)
                    .ToList();

                var oldPeriodSummaries = persAccPeriodSummaryDomain.GetAll()
                    .Where(x => x.Period.Id == oldPeriod.Id)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.SaldoIn,
                            x.TariffPayment,
                            x.PenaltyPayment,
                            Recalc = x.RecalcByBaseTariff, // TODO fix recalc
                            x.Penalty,
                            x.ChargeTariff,
                            AccountId = x.PersonalAccount.Id,
                            RentPayment = rentPaymentInQuery
                                .Where(y => y.Account.Id == x.PersonalAccount.Id)
                                .Where(y => y.OperationDate >= x.Period.StartDate)
                                .Where(y => y.OperationDate <= x.Period.EndDate)
                                .Select(y => (decimal?) y.Sum)
                                .Sum() ?? 0,
                            SaldoChange = saldoChangeQuery
                                .Where(y => y.PeriodSummary.PersonalAccount.Id == x.PersonalAccount.Id)
                                .Where(y => y.ObjectCreateDate >= x.Period.StartDate)
                                .Where(y => y.ObjectCreateDate <= x.Period.EndDate)
                                .Select(y => (decimal?) y.NewValue - y.CurrentValue)
                                .Sum() ?? 0
                        })
                    .AsEnumerable()
                    .Select(
                        x => new
                        {
                            x.AccountId,
                            SaldoOut = x.SaldoIn
                                + x.ChargeTariff
                                + x.Penalty
                                + x.Recalc
                                + x.SaldoChange
                                - x.RentPayment
                                - x.PenaltyPayment
                                - x.TariffPayment
                        })
                    .GroupBy(x => x.AccountId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.SaldoOut).First());

                var listToUpd = new List<PersonalAccountPeriodSummary>();
                foreach (var newPeriodSummary in newPeriodSummaries)
                {
                    var oldPeriodSaldoOut = oldPeriodSummaries.Get(newPeriodSummary.PersonalAccount.Id);

                    if (newPeriodSummary.SaldoIn != oldPeriodSaldoOut)
                    {
                        newPeriodSummary.SaldoIn = oldPeriodSaldoOut;
                        listToUpd.Add(newPeriodSummary);
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, listToUpd, 10000, true, true);
            }
            finally
            {
                this.Container.Release(chargePeriodRepo);
                this.Container.Release(persAccPeriodSummaryDomain);
                this.Container.Release(saldoChangeDomain);
                this.Container.Release(rentPaymentDomain);
            }

            return new BaseDataResult();
        }
    }
}