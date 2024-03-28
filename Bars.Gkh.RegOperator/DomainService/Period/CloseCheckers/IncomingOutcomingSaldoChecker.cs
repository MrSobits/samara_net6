namespace Bars.Gkh.RegOperator.DomainService.Period.CloseCheckers
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Repositories;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    using NHibernate.Transform;

    public abstract class IncomingOutcomingSaldoChecker : IPeriodCloseChecker
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

            var cpRepo = this.Container.Resolve<IChargePeriodRepository>();
            var cpDomain = this.Container.ResolveDomain<ChargePeriod>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var stateRepo = this.Container.Resolve<IStateRepository>();
            try
            {
                var currentPeriod = cpDomain.Get(periodId);
                var previousPeriod = cpRepo.GetPreviousPeriod(currentPeriod);
                var activeState = stateRepo.GetAllStates<BasePersonalAccount>(x => x.Name == "Открыт").First();
                var session = sessionProvider.GetCurrentSession();

                var query =
                    session.CreateSQLQuery(
                        $@"SELECT pa.id as Id,
       pa.acc_num as AccountNumber,
       s1.saldo_out as SaldoOut,
       s2.saldo_in as SaldoIn
FROM regop_pers_acc pa
JOIN regop_pers_acc_period_summ s1 ON s1.account_id = pa.id
AND s1.period_id = :ppid
JOIN regop_pers_acc_period_summ s2 ON s2.account_id = pa.id
AND s2.period_id = :cpid
WHERE {(this.ActiveAccounts
             ? "pa.state_id = :sid"
             : "not(pa.state_id = :sid)")}
  AND not(round(s1.saldo_out,2) = round(s2.saldo_in,2))");

                query.SetParameter("ppid", previousPeriod.Id);
                query.SetParameter("cpid", currentPeriod.Id);
                query.SetParameter("sid", activeState.Id);

                query.SetResultTransformer(Transformers.AliasToBean<QueryDto>());

                var dtos = query.List<QueryDto>();

                result.Success = dtos.Count == 0;
                if (!result.Success)
                {
                    result.InvalidAccountIds = dtos.Select(x => x.Id).ToList();
                    result.Log.AppendLine("ЛС;Исходящее сальдо;Входящее сальдо");
                    foreach (var dto in dtos)
                    {
                        result.Log.AppendLine($"\"{dto.AccountNumber}\";{dto.SaldoOut};{dto.SaldoIn}");
                    }
                }

                return result;
            }
            finally
            {
                this.Container.Release(cpRepo);
                this.Container.Release(cpDomain);
                this.Container.Release(sessionProvider);
                this.Container.Release(stateRepo);
            }
        }

        protected struct QueryDto
        {
            public long Id { get; set; }

            public string AccountNumber { get; set; }

            public decimal SaldoOut { get; set; }

            public decimal SaldoIn { get; set; }
        }
    }

    public class IncomingOutcomingSaldoChecker_Active : IncomingOutcomingSaldoChecker
    {
        public static readonly string Id = typeof(IncomingOutcomingSaldoChecker_Active).FullName;

        protected override bool ActiveAccounts => true;

        /// <summary>
        /// Системный код проверки
        /// </summary>
        public override string Impl => IncomingOutcomingSaldoChecker_Active.Id;

        /// <summary>
        /// Бессысленный код проверки, для отображения
        /// </summary>
        public override string Code => "1";

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public override string Name => "П – Входящее сальдо не равно Исходящему сальдо (по открытым ЛС)";
    }

    public class IncomingOutcomingSaldoChecker_Others : IncomingOutcomingSaldoChecker
    {
        public static readonly string Id = typeof(IncomingOutcomingSaldoChecker_Others).FullName;

        protected override bool ActiveAccounts => false;

        /// <summary>
        /// Системный код проверки
        /// </summary>
        public override string Impl => IncomingOutcomingSaldoChecker_Others.Id;

        /// <summary>
        /// Бессысленный код проверки, для отображения
        /// </summary>
        public override string Code => "4";

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public override string Name => "П – Входящее сальдо не равно Исходящему сальдо (по остальным ЛС)";
    }
}