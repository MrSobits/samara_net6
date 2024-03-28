namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Действие перерасчета исходящего сальдо для текущего периода по лс
    /// </summary>
    public class RecalcPeriodSummarySaldoOutAction : BaseExecutionAction
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        public string Code => "RecalcPeriodSummarySaldoOutAction";

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Перерасчет исходящего сальдо с округлением до двух знаков";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "РегОператор - Перерасчет исходящего сальдо с округлением до двух знаков";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => RecalcPeriodSummarySaldoOutAction.Execute;

        private static BaseDataResult Execute()
        {
            var container = ApplicationContext.Current.Container;

            var periodRepo = container.Resolve<IChargePeriodRepository>();
            var periodSummaryDomain = container.ResolveDomain<PersonalAccountPeriodSummary>();
            var sessionProvider = container.Resolve<ISessionProvider>();

            var currentPeriod = periodRepo.GetCurrentPeriod();

            if (currentPeriod == null)
            {
                return new BaseDataResult();
            }

            var count = periodSummaryDomain.GetAll()
                .Count(x => x.Period.Id == currentPeriod.Id);

            var take = 5000;

            for (int i = 0; i < count; i += take)
            {
                var summaries = periodSummaryDomain.GetAll()
                    .Where(x => x.Period.Id == currentPeriod.Id)
                    .OrderBy(x => x.Id)
                    .Skip(i)
                    .Take(take)
                    .ToArray();

                foreach (var summary in summaries)
                {
                    summary.SaldoIn = summary.SaldoIn.RegopRoundDecimal(2);

                    summary.RecalcSaldoOut();
                }

                TransactionHelper.InsertInManyTransactions(container, summaries, take, useStatelessSession: true);

                sessionProvider.GetCurrentSession().Clear();
            }

            return new BaseDataResult();
        }
    }
}