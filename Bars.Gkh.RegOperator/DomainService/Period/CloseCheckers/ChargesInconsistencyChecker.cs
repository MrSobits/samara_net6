namespace Bars.Gkh.RegOperator.DomainService.Period.CloseCheckers
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;

    using Castle.Windsor;

    using NHibernate;
    using NHibernate.Transform;

    public class ChargesInconsistencyChecker : IPeriodCloseChecker
    {
        public static readonly string Id = typeof(ChargesInconsistencyChecker).FullName;

        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Системный код проверки
        /// </summary>
        public string Impl => ChargesInconsistencyChecker.Id;

        /// <summary>
        /// Код проверки
        /// </summary>
        public string Code => "8";

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string Name => "П - Несоответствие начислений";

        /// <summary>
        /// Выполнить проверку
        /// </summary>
        /// <param name="periodId">Идентификатор проверяемого периода</param>
        /// <returns>Результат проверки</returns>
        public PeriodCloseCheckerResult Check(long periodId)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            var result = new PeriodCloseCheckerResult();
            try
            {
                var session = sessionProvider.GetCurrentSession();
                result.Success = true;

                var chargesDiff = this.GetChargesDiff(session, periodId);
                result.Success = chargesDiff.Count == 0;

                if (!result.Success)
                {
                    result.InvalidAccountIds = chargesDiff.Select(x => x.AccountId).ToList();
                    result.Log.AppendLine("Лицевой счет;Начисления в сальдо;Начисления в детализации");
                    foreach (var diff in chargesDiff)
                    {
                        result.Log.AppendLine($"{diff.AccountNumber};{diff.SummaryCharge};{diff.TransferCharge}");
                    }
                }

                return result;
            }
            finally
            {
                this.Container.Release(sessionProvider);
            }
        }

        private IList<QueryDto> GetChargesDiff(ISession session, long periodId)
        {
            var query = session.CreateSQLQuery(@"
                    drop table if exists temp_transfer;
                    create temp table temp_transfer 
                        (wallet_guid varchar(40),
                        amount numeric(19,5));
                    
                    --исходящие все
                    insert into temp_transfer (wallet_guid, amount)
                    select source_guid as wallet_guid,
                      sum(amount * target_coef) as amount
                    from regop_charge_transfer t
                    where
                      t.period_id = :period_id
                    group by source_guid;
                    
                    --переносы начислений (слияние)
                    insert into temp_transfer (wallet_guid, amount)  
                    select target_guid as wallet_guid,
                      sum(amount) as amount
                    from regop_charge_transfer t
                    where
                      t.period_id = :period_id 
                      and t.reason IS NULL 
                      and t.amount > 0
                    group by target_guid, t.reason;
                    
                    drop table if exists t_wallet_guids;
                    create temp table t_wallet_guids as 
                    SELECT p.id,
                    MAX(CASE WHEN w.id=p.bt_wallet_id THEN w.wallet_guid END) as bt_wallet_guid,
                    MAX(CASE WHEN w.id=p.dt_wallet_id THEN w.wallet_guid END) as dt_wallet_guid,
                    MAX(CASE WHEN w.id=p.p_wallet_id THEN w.wallet_guid END) as p_wallet_guid
                    FROM  public.regop_wallet w JOIN public.regop_pers_acc p ON (p.bt_wallet_id = w.id or p.dt_wallet_id = w.id or p.p_wallet_id = w.id)
                    GROUP BY p.id;
                    
                    create index on t_wallet_guids(id);
                    analyze t_wallet_guids;
                    
                    --имитируем трансферы начислений и перерасчетов по базовому тарифу
                    insert into temp_transfer (wallet_guid, amount)  
                    select g.bt_wallet_guid, unnest(array[charge_tariff-overplus, recalc])
                    from public.regop_pers_acc_charge ch 
                    join t_wallet_guids g ON ch.pers_acc_id=g.id
                    where ch.period_id=:period_id and is_active;
                    
                    --имитируем трансферы начислений и перерасчетов по тарифу решения
                    insert into temp_transfer (wallet_guid, amount)  
                    select g.dt_wallet_guid, unnest(array[overplus, recalc_decision])
                    from public.regop_pers_acc_charge ch 
                    join t_wallet_guids g ON ch.pers_acc_id=g.id
                    where ch.period_id=:period_id and is_active;
                    
                    --имитируем трансферы начислений и перерасчетов по пеням
                    insert into temp_transfer (wallet_guid, amount)  
                    select g.dt_wallet_guid, unnest(array[penalty, recalc_penalty])
                    from public.regop_pers_acc_charge ch 
                    join t_wallet_guids g ON ch.pers_acc_id=g.id
                    where ch.period_id=:period_id and is_active;

                    -- непосаженные зачеты средств
                    insert into temp_transfer (wallet_guid, amount)
                    select w.wallet_guid, -ch.sum
                    from regop_perf_work_charge_source source
                    join regop_pers_acc p on source.account_id = p.id
                    join regop_wallet w on (p.bt_wallet_id = w.id or p.dt_wallet_id = w.id or p.p_wallet_id = w.id)
                    join regop_perf_work_charge ch on
                            ch.charge_op_id = source.id
                            and ch.period_id = :period_id
                            and ch.distribute_type = w.wallet_type
                            and not ch.is_active;
                    
                    delete from temp_transfer where amount=0;
                    
                    CREATE INDEX ind_temp_transfer ON temp_transfer USING btree (wallet_guid);
                    analyze temp_transfer;
                    
                    select 
                        a.id as AccountId,
                        acc_num as AccountNumber,
                        summary_sum as SummaryCharge,
                        transfer_sum as TransferCharge
                    from
                    (
                      select pa.id, 
                        pa.acc_num, 
                        sum(t.amount) as transfer_sum,
                        (ps.charge_tariff + 
                            ps.penalty + 
                            ps.recalc + 
                            ps.recalc_decision + 
                            ps.recalc_penalty +
                            ps.balance_change +
                            ps.DEC_BALANCE_CHANGE +
                            ps.PENALTY_BALANCE_CHANGE -
                            ps.PERF_WORK_CHARGE
                        ) as summary_sum
                      from 
                      temp_transfer t
                      join
                      regop_wallet w on w.wallet_guid = t.wallet_guid
                      join regop_pers_acc pa on pa.bt_wallet_id = w.id 
                        or pa.dt_wallet_id = w.id 
                        or pa.p_wallet_id = w.id 
                      join regop_pers_acc_period_summ ps on ps.account_id=pa.id 
                        and ps.period_id = :period_id 
                      group by pa.id, ps.id 
                    )a
                    where abs(summary_sum-transfer_sum)>0.03;");

            query.SetParameter("period_id", periodId);
            query.SetResultTransformer(Transformers.AliasToBean<QueryDto>());

            return query.List<QueryDto>();
        }

        protected struct QueryDto
        {
            public long AccountId { get; set; }
            public string AccountNumber { get; set; }
            public decimal SummaryCharge { get; set; }
            public decimal TransferCharge { get; set; }
        }
    }
}