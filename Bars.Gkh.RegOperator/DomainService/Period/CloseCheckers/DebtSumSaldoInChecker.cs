namespace Bars.Gkh.RegOperator.DomainService.Period.CloseCheckers
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Caching.LinqExtensions;
    using Bars.B4.Utils;

    using Castle.Windsor;

    using NHibernate;
    using NHibernate.Transform;

    public class DebtSumSaldoInChecker : IPeriodCloseChecker
    {
        public static readonly string Id = typeof(DebtSumSaldoInChecker).FullName;

        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Системный код проверки
        /// </summary>
        public string Impl => DebtSumSaldoInChecker.Id;

        /// <summary>
        /// Бессысленный код проверки, для отображения
        /// </summary>
        public string Code => "6";

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string Name => "П - Сумма задолженностей не равна входящему сальдо";

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

				var beforeFixingData = this.GetIncorrectData(session, periodId);
				if (beforeFixingData.Count > 0)
				{
				    this.FixIncorrectData(session, beforeFixingData);

                    var summaryIds = beforeFixingData.Select(x => x.Id).ToArray();
                    var afterFixingData = this.GetIncorrectData(session, periodId, summaryIds);
					if (afterFixingData.Count != beforeFixingData.Count)
					{
						result.Note = $"Была запущена процедура обновления задолженности. Ошибок до: {beforeFixingData.Count}, после: {afterFixingData.Count}";
					}
					
					var newDebtSumsDict = afterFixingData
						.GroupBy(x => x.Id)
						.ToDictionary(x => x.Key, x => x.First().DebtSum);

					result.FullLog.AppendLine("Номер ЛС;Статус ЛС;Прежнее значение;Новое значение;Сумма расхождений");
					foreach (var data in beforeFixingData)
					{
						if (!newDebtSumsDict.ContainsKey(data.Id))
						{
							result.FullLog.AppendLine($"\"{data.AccountNumber}\";{data.AccountState};{data.DebtSum};{data.SaldoInSum};{data.DebtSum - data.SaldoInSum}");
						}
						else
						{
							var afterFixingDebt = newDebtSumsDict.Get(data.Id);
							if (data.DebtSum != afterFixingDebt)
							{
								result.FullLog.AppendLine($"\"{data.AccountNumber}\";{data.AccountState};{data.DebtSum};{afterFixingDebt};{data.DebtSum - afterFixingDebt}");
							}
						}
					}

					result.InvalidAccountIds = afterFixingData.Select(x => x.AccountId)
						.Distinct()
						.ToList();

					result.Success = afterFixingData.Count == 0;
					if (!result.Success)
					{
						result.Log.AppendLine("Номер ЛС;Статус ЛС;Сумма задолженностей;Входящее сальдо");
						foreach (var dto in afterFixingData)
						{
							result.Log.AppendLine($"\"{dto.AccountNumber}\";{dto.AccountState};{dto.DebtSum};{dto.SaldoInSum}");
						}
					}
				}

				return result;
            }
            finally
            {
                Container.Release(sessionProvider);
            }
        }

        private IList<QueryDto> GetIncorrectData(ISession session, long periodId, long[] summaryIds = null)
        {
            
            if (summaryIds.IsNotEmpty())
            {
                var cpmmasepar = string.Join(",", summaryIds.ToList());
                var query = session.CreateSQLQuery($@"
                SELECT t.id, 
	                   t.AccountId, 
	                   t.AccountNumber, 
	                   t.AccountState, 
	                   t.DebtSum, 
	                   t.SaldoInSum
                FROM (
	                SELECT ps.id, 
		                   ps.account_id as AccountId, 
		                   pa.acc_num as AccountNumber, 
		                   st.name as AccountState, 
	                       round(COALESCE((ps.base_tariff_debt + ps.dec_tariff_debt + ps.penalty_debt),0),2) as DebtSum,
	                       round(COALESCE(ps.saldo_in,0),2) as SaldoInSum
	                FROM regop_pers_acc_period_summ ps
	                JOIN regop_pers_acc pa ON ps.account_id = pa.id
	                JOIN b4_state st ON pa.state_id = st.id
                        join (select cast(ps_id as int) as ps_id from regexp_split_to_table('{cpmmasepar}',',') ps_id) psid on psid.ps_id = ps.id
	                WHERE ps.period_id = :period_id
	                ORDER BY 1
                ) t
                WHERE t.DebtSum != t.SaldoInSum
                ");

                query.SetParameter("period_id", periodId);               

                query.SetResultTransformer(Transformers.AliasToBean<QueryDto>());

                return query.List<QueryDto>();
            }
            else
            {
                var query = session.CreateSQLQuery($@"
                SELECT t.id, 
	                   t.AccountId, 
	                   t.AccountNumber, 
	                   t.AccountState, 
	                   t.DebtSum, 
	                   t.SaldoInSum
                FROM (
	                SELECT ps.id, 
		                   ps.account_id as AccountId, 
		                   pa.acc_num as AccountNumber, 
		                   st.name as AccountState, 
	                       round(COALESCE((ps.base_tariff_debt + ps.dec_tariff_debt + ps.penalty_debt),0),2) as DebtSum,
	                       round(COALESCE(ps.saldo_in,0),2) as SaldoInSum
	                FROM regop_pers_acc_period_summ ps
	                JOIN regop_pers_acc pa ON ps.account_id = pa.id
	                JOIN b4_state st ON pa.state_id = st.id
	                WHERE ps.period_id = :period_id
	                ORDER BY 1
                ) t
                WHERE t.DebtSum != t.SaldoInSum
                ");

                query.SetParameter("period_id", periodId);              
                query.SetResultTransformer(Transformers.AliasToBean<QueryDto>());
                return query.List<QueryDto>();
            }
			
        }

        private void FixIncorrectData(ISession session, IEnumerable<QueryDto> summaries)
        {
            var query = session.CreateSQLQuery(@"
                DROP TABLE IF EXISTS t_pers_acc_period_summ;
                CREATE TEMP TABLE t_pers_acc_period_summ as 
                SELECT * FROM REGOP_PERS_ACC_PERIOD_SUMM ps 
                WHERE ps.account_id IN (:acc_ids);

                CREATE INDEX ON t_pers_acc_period_summ(period_id,account_id);
                ANALYZE t_pers_acc_period_summ;
                    
                UPDATE REGOP_PERS_ACC_PERIOD_SUMM ps

                SET base_tariff_debt = coalesce((
                 select sum(ps1.charge_base_tariff + ps1.balance_change + ps1.recalc - ps1.tariff_payment - ps1.perf_work_charge)
                 from t_pers_acc_period_summ ps1
                 join regop_period p1 on p1.id = ps1.period_id
                 where ps1.account_id = ps.account_id 
                 and p1.cstart<p.cstart), 0),

                dec_tariff_debt = coalesce((
                 select sum(ps1.charge_tariff - ps1.charge_base_tariff + ps1.dec_balance_change 
                                + ps1.recalc_decision - ps1.tariff_desicion_payment - ps1.perf_work_charge_dec)
                 from t_pers_acc_period_summ ps1
                 join regop_period p1 on p1.id = ps1.period_id
                 where ps1.account_id = ps.account_id 
                 and p1.cstart<p.cstart), 0),

                penalty_debt = coalesce((
                 select sum(ps1.penalty + ps1.recalc_penalty + ps1.penalty_balance_change - ps1.penalty_payment)
                 from t_pers_acc_period_summ ps1
                 join regop_period p1 on p1.id = ps1.period_id
                 where ps1.account_id = ps.account_id
                 and p1.cstart<p.cstart), 0)

                FROM regop_period p
                WHERE ps.period_id = p.id
                AND ps.id IN (:ids)");

            foreach (var summaryChunk in summaries.Split(1000))
            {
                query.SetParameterList("ids", summaryChunk.Select(x => x.Id).ToArray());
                query.SetParameterList("acc_ids", summaryChunk.Select(x => x.AccountId).ToArray());
                query.ExecuteUpdate();
            }
        }

        protected struct QueryDto
        {
            public long Id { get; set; }
            public long AccountId { get; set; }
			public string AccountNumber { get; set; }
			public string AccountState { get; set; }
			public decimal DebtSum { get; set; }
			public decimal SaldoInSum { get; set; }
        }
    }
}