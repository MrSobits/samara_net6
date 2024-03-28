namespace Bars.Gkh.RegOperator.DomainService.Period.CloseCheckers
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Caching.LinqExtensions;

    using Castle.Windsor;

    using NHibernate;
    using NHibernate.Transform;

    /// <summary>
    /// Контрольная проверка несоответствие оплат
    /// </summary>
    public class TransferInconsistencyChecker : IPeriodCloseChecker
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public static readonly string Id = typeof(TransferInconsistencyChecker).FullName;

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Системный код проверки
        /// </summary>
        public string Impl => TransferInconsistencyChecker.Id;

        /// <summary>
        /// Бессысленный код проверки, для отображения
        /// </summary>
        public string Code => "3";

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string Name => "П – Несоответствие оплат";


        /// <summary>
        /// Выполнить проверку
        /// </summary>
        /// <param name="periodId">Идентификатор проверяемого периода</param>
        /// <returns>Результат проверки</returns>
        public PeriodCloseCheckerResult Check(long periodId)
        {
            var result = new PeriodCloseCheckerResult();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            try
            {
                var session = sessionProvider.GetCurrentSession();

               /* var dtos = this.GetInconsistency(periodId, session);
                if (dtos.Count > 0)
                {
                    var problems = dtos;
                    this.TryFixInconsistency(session, periodId, dtos.Select(x => x.Id).ToList());
                    dtos = this.GetInconsistency(periodId, session);

                    result.Note = $"Была запущена процедура учета оплат. Ошибок до: {problems.Count}, после: {dtos.Count}";
                    result.FullLog.AppendLine("Лицевой счет;Сумма оплат до исправления;Сумма оплат после исправления;Разница сумм");

                    var validAccounts = this.GetNoProblems(problems.Where(x => dtos.All(y => x.AccountNumber != y.AccountNumber)).Select(x => x.AccountNumber).ToList(), periodId, session);
                    foreach (var noProblem in validAccounts)
                    {
                        var sumProblem = problems.Where(x => x.AccountNumber == noProblem.AccountNumber).Select(x => x.Summary).FirstOrDefault();
                        result.FullLog.AppendLine($"\"{noProblem.AccountNumber}\";{sumProblem};{noProblem.Summary};{noProblem.Summary - sumProblem}");
                    }
                }*/

                result.Success = true;
               /* if (!result.Success)
                {
                    result.InvalidAccountIds = dtos.Select(x => x.Id).ToList();
                    result.Log.AppendLine("ЛС;Оплаты в сальдо;Оплаты в детализации");
                    foreach (var dto in dtos)
                    {
                        result.Log.AppendLine($"\"{dto.AccountNumber}\";{dto.Saldo};{dto.Summary}");
                    }
                }*/

                return result;
            }
            finally
            {
                this.Container.Release(sessionProvider);
            }
        }

        private IList<QueryDto> GetInconsistency(long periodId, ISession session)
        {
            var query = session.CreateSQLQuery($@"SELECT t2.account_id AS Id,
                            v.acc_num AS AccountNumber,
                            t2.amount AS Saldo,
                            t2.r_p_acc_period_summ AS Summary
                     FROM
                       (SELECT ps.account_id,
                               t1.amount amount,
                               'tariff_payment' AS type_,
                               ps.tariff_payment + ps.tariff_desicion_payment + ps.penalty_payment AS r_p_acc_period_summ
                        FROM regop_pers_acc_period_summ_period_{periodId} ps
                        LEFT JOIN
                          (SELECT coalesce(sum(tt.amount), 0) AS amount,
                                  tt.id
                           FROM
                             (SELECT t.amount*target_coef amount,
                                     pa.id,
                                     t.reason
                              FROM regop_transfer_period_{periodId} t
                              JOIN regop_wallet w ON w.wallet_guid = t.target_guid
                              JOIN regop_pers_acc pa ON pa.id = t.owner_id
                              UNION ALL SELECT (-1)*t.amount*target_coef AS amount,
                                               pa.id,
                                               t.reason
                              FROM regop_transfer_period_{periodId} t
                              JOIN regop_wallet w ON w.wallet_guid = t.source_guid
                              JOIN regop_pers_acc pa ON pa.id = t.owner_id) tt
                           GROUP BY tt.id) t1 ON t1.id = ps.account_id) t2
                     LEFT JOIN z_view_regop_pa v ON v.id=t2.account_id
                     WHERE 1=1
                       AND round(COALESCE(abs(t2.amount),0),2)<>round(COALESCE(abs(t2.r_p_acc_period_summ),0),2)
                     ORDER BY 1");
            
            query.SetResultTransformer(Transformers.AliasToBean<QueryDto>());

            return query.List<QueryDto>();
        }

        private void TryFixInconsistency(ISession session, long periodId, IEnumerable<long> accountIds)
        {
            var query = session.CreateSQLQuery($@"
            DROP TABLE IF EXISTS t_wallet_ids;
            CREATE TEMP TABLE t_wallet_ids as 
            SELECT id as account_id, UNNEST(ARRAY[bt_wallet_id,dt_wallet_id,p_wallet_id]) wallet_id,
                                     UNNEST(ARRAY['bt_wallet_id','dt_wallet_id','p_wallet_id']) wallet_type
            FROM regop_pers_acc WHERE id IN (:ids);
            
            drop table if exists t_wallet_guids;
            create temp table t_wallet_guids as 
            select i.account_id, w.wallet_guid , i.wallet_type
            from regop_wallet w JOIN t_wallet_ids i ON i.wallet_id=w.id;
            
            create index on t_wallet_guids(account_id,wallet_guid) where wallet_type='bt_wallet_id';
            create index on t_wallet_guids(account_id,wallet_guid) where wallet_type='dt_wallet_id';
            create index on t_wallet_guids(account_id,wallet_guid) where wallet_type='p_wallet_id';
            analyze t_wallet_guids;
            
            UPDATE regop_pers_acc_period_summ_period_{periodId} ps
            SET tariff_payment=
              (SELECT coalesce(sum(tt.amount), 0)
               FROM
                 (SELECT t.amount
                  FROM regop_transfer_period_{periodId} t
                  JOIN t_wallet_guids w ON
                    w.wallet_guid = t.target_guid
                      AND w.account_id = t.owner_id
                      AND w.wallet_type='bt_wallet_id'
                      AND w.account_id = ps.account_id
                   UNION ALL
            	  SELECT (-1) * t.target_coef * t.amount
                  FROM regop_transfer_period_{periodId} t
                  JOIN t_wallet_guids w ON
                      w.wallet_guid = t.source_guid
                        AND w.account_id = t.owner_id
                        AND w.wallet_type='bt_wallet_id'
                        AND w.account_id = ps.account_id
            	) tt),
                TARIFF_DESICION_PAYMENT =
              (SELECT coalesce(sum(tt.amount), 0)
               FROM
                 (SELECT t.amount
                  FROM regop_transfer_period_{periodId} t
                  JOIN t_wallet_guids w ON
                    w.wallet_guid = t.target_guid
                      AND w.account_id = t.owner_id
                      AND w.wallet_type='dt_wallet_id'
                      AND w.account_id = ps.account_id
                   UNION ALL
            	  SELECT (-1) * t.target_coef * t.amount
                  FROM regop_transfer_period_{periodId} t
                  JOIN t_wallet_guids w ON
                      w.wallet_guid = t.source_guid
                        AND w.account_id = t.owner_id
                        AND w.wallet_type='dt_wallet_id'
                        AND w.account_id = ps.account_id
            	) tt),
                PENALTY_PAYMENT =
              (SELECT coalesce(sum(tt.amount), 0)
               FROM
                 (SELECT t.amount
                  FROM regop_transfer_period_{periodId} t
                  JOIN t_wallet_guids w ON
                    w.wallet_guid = t.target_guid
                      AND w.account_id = t.owner_id
                      AND w.wallet_type='p_wallet_id'
                      AND w.account_id = ps.account_id
                   UNION ALL
            	  SELECT (-1) * t.target_coef * t.amount
                  FROM regop_transfer_period_{periodId} t
                  JOIN t_wallet_guids w ON
                      w.wallet_guid = t.source_guid
                        AND w.account_id = t.owner_id
                        AND w.wallet_type='p_wallet_id'
                        AND w.account_id = ps.account_id
            	) tt)
            WHERE  ps.account_id IN (:ids)");

            foreach (var ids in accountIds.Split(1000))
            {
                query.SetParameterList("ids", ids);
                query.ExecuteUpdate();
            }
        }

        private IList<ValidAccount> GetNoProblems(IEnumerable<string> accNums, long periodId, ISession session)
        {
            if (accNums.Any())
            {
                var query = session.CreateSQLQuery(@"select acc.ACC_NUM as AccountNumber, 
                    sum.tariff_payment + sum.tariff_desicion_payment + sum.penalty_payment as Summary
                    from REGOP_PERS_ACC acc inner join 
                    REGOP_PERS_ACC_PERIOD_SUMM sum on acc.id =  sum.ACCOUNT_ID 
                    where acc.ACC_NUM in (:accNum) and sum.PERIOD_ID=:pid");

                query.SetParameter("pid", periodId);
                query.SetParameterList("accNum", accNums);
                
                query.SetResultTransformer(Transformers.AliasToBean<ValidAccount>());

                return query.List<ValidAccount>();
            }
            else
            {
                return  new List<ValidAccount>();
            }
        }

        /// <summary>
        ///  QueryDto
        /// </summary>
        protected struct QueryDto
        {
            /// <summary>
            /// Идентификатор
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Номер счета
            /// </summary>
            public string AccountNumber { get; set; }

            /// <summary>
            /// Сальдо
            /// </summary>
            public decimal Saldo { get; set; }

            /// <summary>
            /// Summary
            /// </summary>
            public decimal Summary { get; set; }
        }

        protected struct ValidAccount
        {
            /// <summary>
            /// Номер счета
            /// </summary>
            public string AccountNumber { get; set; }

            /// <summary>
            /// Summary
            /// </summary>
            public decimal Summary { get; set; }
        }
    }
}