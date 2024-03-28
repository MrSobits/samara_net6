namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    using NHibernate.Linq;

    public class InputSaldoReculcAction : BaseExecutionAction
    {
        public static string Code = "InputSaldoReculcAction";

        public override string Description => "Пересчет входящего сальдо";

        public override string Name => "Пересчет входящего сальдо";

        public override Func<IDataResult> Action => this.Execute;

        public ILogger Logger { get; set; }

        private BaseDataResult Execute()
        {
            var personalAccountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var accountPerionSummaryDomain = this.Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var saldoChangeDomain = this.Container.ResolveDomain<PeriodSummaryBalanceChange>();
            var rentPaymentDomain = this.Container.ResolveDomain<RentPaymentIn>();

            bool wasError = false;

            try
            {
                var accountsIds = personalAccountDomain.GetAll().Select(x => x.Id);

                var accountSummaries =
                    accountPerionSummaryDomain.GetAll().Where(x => accountsIds.Contains(x.PersonalAccount.Id)).Fetch(x => x.Period);

                var totalCount = accountSummaries.Count();

                var data = new List<PersonalAccountPeriodSummary>();

                var summariesForSave = new List<PersonalAccountPeriodSummary>();

                int start = 0;
                const int Max = 10000;

                while (start < totalCount)
                {
                    int count = totalCount - start;
                    if (count > Max)
                    {
                        count = Max;
                    }

                    data.AddRange(accountSummaries.Skip(start).Take(count).ToArray());

                    start += count;
                }

                foreach (var accountId in accountsIds.ToArray())
                {
                    long id = accountId;
                    var summaries = data.Where(x => x.PersonalAccount.Id == id).OrderBy(x => x.Period.StartDate);

                    var saldoChangeQuery = saldoChangeDomain.GetAll().Where(y => y.PeriodSummary.PersonalAccount.Id == id);

                    var rentPaymentInQuery = rentPaymentDomain.GetAll().Where(x => x.Account.Id == id);

                    PersonalAccountPeriodSummary prevSummary = null;

                    foreach (var summary in summaries)
                    {
                        var rentPayment =
                            rentPaymentInQuery.Where(y => y.OperationDate >= summary.Period.StartDate)
                                .Where(y => y.OperationDate <= summary.Period.EndDate)
                                .Select(y => (decimal?) y.Sum)
                                .Sum() ?? 0;

                        var saldoChange =
                            saldoChangeQuery.Where(y => y.OperationDate >= summary.Period.StartDate)
                                .Where(y => y.OperationDate <= summary.Period.EndDate)
                                .Select(y => (decimal?) y.NewValue - y.CurrentValue)
                                .Sum() ?? 0;
                        if (prevSummary != null)
                        {
                            var saldoOut = prevSummary.SaldoIn + prevSummary.ChargeTariff + prevSummary.Penalty
                                + prevSummary.RecalcByBaseTariff + saldoChange - rentPayment - prevSummary.PenaltyPayment
                                - prevSummary.TariffPayment; // TODO fix recalc

                            if (saldoOut != summary.SaldoIn)
                            {
                                summary.SaldoIn = saldoOut;
                                summariesForSave.Add(summary);
                            }
                        }

                        prevSummary = summary;
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, summariesForSave);
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Возникла ошибка в процессе пересчета");
                wasError = true;
            }
            finally
            {
                this.Container.Release(personalAccountDomain);
                this.Container.Release(accountPerionSummaryDomain);
                this.Container.Release(saldoChangeDomain);
                this.Container.Release(rentPaymentDomain);
            }

            if (wasError)
            {
                return new BaseDataResult(false, "Возникла ошибка в процессе пересчета");
            }

            return new BaseDataResult();
        }
    }
}