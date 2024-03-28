namespace Bars.Gkh.RegOperator.DomainService.Period.CloseCheckers
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Repositories;

    using Castle.Windsor;

    using NHibernate.Transform;

    public abstract class OutcomingSaldoInconstistenceChecker : IPeriodCloseChecker
    {
        public IWindsorContainer Container { get; set; }

        protected abstract bool ActiveAccounts { get; }

        /// <summary>
        /// Системный код проверки
        /// </summary>
        public abstract string Impl { get; }

        /// <summary>
        /// Бессысленный код проверки, для отображения
        /// </summary>
        public abstract string Code { get; }

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Выполнить проверку
        /// </summary>
        /// <param name="periodId">Идентификатор проверяемого периода</param>
        /// <returns>Результат проверки</returns>
        public PeriodCloseCheckerResult Check(long periodId)
        {
            var result = new PeriodCloseCheckerResult();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var stateRepo = this.Container.Resolve<IStateRepository>();
            try
            {
                var activeState = stateRepo.GetAllStates<BasePersonalAccount>(x => x.Name == "Открыт").First();
                var session = sessionProvider.GetCurrentSession();

                var query =
                    session.CreateSQLQuery(
                        $@"SELECT pa.id AS Id,
	                        pa.acc_num AS AccountNumber,
	                        s.saldo_in AS SaldoIn,
	                        s.charge_tariff AS ChargeTariff,
	                        s.penalty AS Penalty,
	                        (s.tariff_payment + s.tariff_desicion_payment + s.penalty_payment) AS Paid,
	                        s.balance_change + s.dec_balance_change + s.penalty_balance_change AS BalanceChange,
	                        s.perf_work_charge + s.perf_work_charge_dec AS PerfWorkCharge,
	                        (s.recalc+s.recalc_decision+s.recalc_penalty) AS Recalc,
	                        s.saldo_out AS SaldoOut
                        FROM regop_pers_acc pa
                        JOIN regop_pers_acc_period_summ s ON s.account_id = pa.id
	                        AND s.period_id = :pid
                        WHERE {(this.ActiveAccounts
                                ? "pa.state_id = :sid"
                                : "not(pa.state_id = :sid)")}
	                        AND abs(s.saldo_in + s.charge_tariff + s.penalty 
		                        + s.recalc + s.recalc_decision + s.recalc_penalty
		                        - s.tariff_payment - s.tariff_desicion_payment - s.penalty_payment 
		                        + s.balance_change + s.dec_balance_change + s.penalty_balance_change 
		                        - s.perf_work_charge - s.perf_work_charge_dec
		                        - s.saldo_out)>0.01;");

                query.SetParameter("pid", periodId);
                query.SetParameter("sid", activeState.Id);

                query.SetResultTransformer(Transformers.AliasToBean<QueryDto>());

                var dtos = query.List<QueryDto>();

                result.Success = dtos.Count == 0;
                if (!result.Success)
                {
                    result.InvalidAccountIds = dtos.Select(x => x.Id).ToList();
                    result.Log.AppendLine("ЛС;Входящее сальдо;Начислено пени;Начислено;Оплачено;Перекидка;Зачет средств;Перерасчет;Исходящее сальдо");
                    foreach (var dto in dtos)
                    {
                        result.Log.AppendLine($"\"{dto.AccountNumber}\";{dto.SaldoIn};{dto.Penalty};{dto.ChargeTariff};{dto.Paid};{dto.BalanceChange};{dto.PerfWorkCharge};{dto.Recalc};{dto.SaldoOut}");
                    }
                }

                return result;
            }
            finally
            {
                this.Container.Release(sessionProvider);
                this.Container.Release(stateRepo);
            }
        }

        protected struct QueryDto
        {
            public long Id { get; set; }

            public string AccountNumber { get; set; }

            public decimal SaldoIn { get; set; }

            public decimal ChargeTariff { get; set; }

            public decimal Penalty { get; set; }

            public decimal Paid { get; set; }

            public decimal BalanceChange { get; set; }

            public decimal PerfWorkCharge { get; set; }

            public decimal Recalc { get; set; }

            public decimal SaldoOut { get; set; }
        }
    }

    public class OutcomingSaldoInconstistenceChecker_Active : OutcomingSaldoInconstistenceChecker
    {
        public static readonly string Id = typeof(OutcomingSaldoInconstistenceChecker_Active).FullName;

        protected override bool ActiveAccounts => true;

        /// <summary>
        /// Системный код проверки
        /// </summary>
        public override string Impl => OutcomingSaldoInconstistenceChecker_Active.Id;

        /// <summary>
        /// Бессысленный код проверки, для отображения
        /// </summary>
        public override string Code => "2";

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public override string Name => "П – Несоответствие сальдо в лицевом счете (по открытым ЛС)";
    }

    public class OutcomingSaldoInconstistenceChecker_Others : OutcomingSaldoInconstistenceChecker
    {
        public static readonly string Id = typeof(OutcomingSaldoInconstistenceChecker_Others).FullName;

        protected override bool ActiveAccounts => false;

        /// <summary>
        /// Системный код проверки
        /// </summary>
        public override string Impl => OutcomingSaldoInconstistenceChecker_Others.Id;

        /// <summary>
        /// Бессысленный код проверки, для отображения
        /// </summary>
        public override string Code => "5";

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public override string Name => "П – Несоответствие сальдо в лицевом счете (по остальным ЛС)";
    }
}