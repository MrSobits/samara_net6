namespace Bars.Gkh.RegOperator.DomainService.Period
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.Period;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using RealityObjectAccount;
    using Entities;

    using Castle.Windsor;
    using Domain.Repository;

	/// <summary>
	/// Сервис для Период начислений
	/// </summary>
	public class ChargePeriodService : IChargePeriodService
    {
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        public IGkhUserManager GkhUserManager { get; set; }

        private static readonly object SyncRoot = new object();

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="periodDomain">Домен сервис для Период начислений</param>
		/// <param name="container">Контейнер</param>
		/// <param name="realtyObjectRegopOperationService">Сервис для операций регоператора жилого дома</param>
		/// <param name="calculatedRepo">Домен сервис для вычисляемых параметров лицевого счета</param>
		/// <param name="chargePeriodRepo">Репозиторий для Период начислений</param>
        public ChargePeriodService(
            IDomainService<ChargePeriod> periodDomain,
            IWindsorContainer container,
            IRealtyObjectRegopOperationService realtyObjectRegopOperationService,
            IDomainService<PersonalAccountCalculatedParameter> calculatedRepo,
            IChargePeriodRepository chargePeriodRepo)
        {
            this.PeriodDomain = periodDomain;
            this.Container = container;
            this.RealtyObjectRegopOperationService = realtyObjectRegopOperationService;
            this.CalculatedRepo = calculatedRepo;
            this.ChargePeriodRepo = chargePeriodRepo;
        }

		/// <summary>
		/// Получить список закрытых периодов
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult ListClosedPeriods(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = this.PeriodDomain.GetAll()
                .Where(x => x.IsClosed)
                .Filter(loadParams, this.Container)
                .Order(loadParams);

            return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
        }

		/// <summary>
		/// Открыть начальный период
		/// </summary>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult CreateFirstPeriod()
        {
            if (!Monitor.TryEnter(ChargePeriodService.SyncRoot))
            {
                return new BaseDataResult(new {message = "Создание периода в процессе.", type = "info"});
            }

            try
            {
                var anyOpenPeriod = this.PeriodDomain.GetAll().Any(x => !x.IsClosed);

                if (anyOpenPeriod)
                {
                    return new BaseDataResult();
                }

                var chargePeriod =
                    new ChargePeriod("Начальный период {0}".FormatUsing(DateTime.Now.ToString("yyyy MMMM")),
                        new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));

                this.PeriodDomain.Save(chargePeriod);

                this.AfterCreateAction(chargePeriod);
            }
            finally
            {
                Monitor.Exit(ChargePeriodService.SyncRoot);
            }
            return new BaseDataResult();
        }

		/// <summary>
		/// Получить дату начала первого периода
		/// </summary>
		/// <returns>Дата начала первого периода</returns>
		public DateTime GetStartDateOfFirstPeriod()
	    {
		    return this.PeriodDomain.GetAll().Min(x => x.StartDate);
	    }

        /// <summary>
        /// Откатить закрытый месяц
        /// </summary>
        /// <returns></returns>
        public IDataResult RollbackClosedPeriod(BaseParams baseParams)
        {
            var isDeleteSnapshot = baseParams.Params.GetAs<bool>("isDeleteSnapshot");
            var reason = baseParams.Params.GetAs<string>("reason");

            var user = this.GkhUserManager.GetActiveUser();

            var curPeriod = this.ChargePeriodRepository.GetCurrentPeriod();
            var lastPeriod = this.ChargePeriodRepository.GetLastClosedPeriod();

            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var perCloseRollbackHistDomain = this.Container.ResolveDomain<PeriodCloseRollbackHistory>();
            try
            {
                var existsRollbakPeriod = perCloseRollbackHistDomain
                    .GetAll()
                    .Any(x => x.Period.Id == curPeriod.Id);

                if (existsRollbakPeriod)
                {
                    return new BaseDataResult(false, "Запрещено откатывать период больше одного раза подряд");
                }

                var session = sessionProvider.GetCurrentSession();

                this.Container.InTransaction(
                    () =>
                    {
                        var deleteSql = @"select 'alter table '||t.relname||' drop constraint '|| c.conname||';'
                  from pg_constraint c
                  join pg_class t on c.conrelid = t.oid
                  join pg_namespace n on t.relnamespace = n.oid
                  where t.relname like 'regop_transfer%' and c.conname like '%_op';";

                        var createSql =
                            $@"select 'alter table '||t.relname||' add constraint '|| c.conname||' FOREIGN KEY (op_id) 
                  REFERENCES regop_money_operation (id) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION'||';'
                  from pg_constraint c
                  join pg_class t on c.conrelid = t.oid
                  join pg_namespace n on t.relnamespace = n.oid
                  where t.relname like 'regop_transfer%' and c.conname like '%_op' and c.conname not like '%period_{curPeriod.Id}%'";


                        var deleteQuery = session.CreateSQLQuery(deleteSql);
                        var createQuery = session.CreateSQLQuery(createSql);

                        var createResult = createQuery.List<string>().AggregateWithSeparator("\n");
                        var deleteResult = deleteQuery.List<string>().AggregateWithSeparator("\n");
                        session.CreateSQLQuery(deleteResult).ExecuteUpdate();

                        var rollbackSql =
                            $@"drop table regop_recalc_history_period_{curPeriod.Id};
                        drop table regop_calc_param_trace_period_{curPeriod.Id};
                        drop table regop_reality_transfer_period_{curPeriod.Id};
                        drop table regop_transfer_period_{curPeriod.Id};
                        drop table regop_charge_transfer_period_{curPeriod.Id};
                        delete from regop_bank_stmnt_op where op_id in (select id from regop_money_operation where period_id={curPeriod.Id});
                        delete from regop_money_lock where operation_id in (select id from regop_money_operation where period_id={curPeriod.Id});
                        delete from regop_money_operation where period_id={curPeriod.Id};
                        drop table regop_pers_acc_charge_period_{curPeriod.Id};
                        drop table regop_pers_acc_period_summ_period_{curPeriod.Id};
                        delete from regop_ro_charge_acc_charge where period_id={curPeriod.Id};
                        delete from regop_payment_doc_templ where period_id={curPeriod.Id};
                        delete from regop_pers_paydoc_snap where snapshot_id in (select id from regop_payment_doc_snapshot where period_id={curPeriod.Id});
                        delete from regop_payment_doc_snapshot where period_id={curPeriod.Id};
                        delete from regop_payment_doc_acc_log where log_id in (select id from regop_payment_doc_log where period_id={curPeriod.Id});
                        delete from regop_payment_doc_log where period_id = {curPeriod.Id};
                        delete from regop_period_cls_chck_res where period_id={curPeriod.Id};
                        delete from regop_pers_acc_recalc_evt where period_id={curPeriod.Id};
                        delete from regop_saldo_change_source where id in (select id from regop_charge_operation_base where period_id={curPeriod.Id});
                        delete from regop_saldo_change_detail where charge_op_id in (select id from regop_charge_operation_base where period_id={curPeriod.Id});
                        delete from regop_cancel_charge where charge_op_id in (select id from regop_charge_operation_base where period_id={curPeriod.Id});
                        delete from regop_charge_cancel_source where id in (select id from regop_charge_operation_base where period_id={curPeriod.Id});
                        delete from regop_split_acc_source where id in (select id from regop_charge_operation_base where period_id={curPeriod.Id});
                        delete from regop_perf_work_charge_source where id in (select id from regop_charge_operation_base where period_id={curPeriod.Id});
                        delete from regop_split_acc_detail where charge_op_id in (select id from regop_charge_operation_base where period_id={curPeriod.Id});
                        delete from regop_perf_work_charge where charge_op_id in (select id from regop_charge_operation_base where period_id={curPeriod.Id});
                        delete from regop_charge_operation_base where period_id={curPeriod.Id};
                        delete from regop_recalc_history where calc_period_id={curPeriod.Id};
                        delete from regop_saldo_change_export_pa where change_id in (select id from regop_saldo_change_export where period_id={curPeriod.Id});
                        delete from regop_saldo_change_export where period_id={curPeriod.Id};
                        delete from regop_payment_correction
                        where payment_op_id in (select id from regop_payment_operation_base where period_id={curPeriod.Id});
                        delete from regop_payment_correction_source
                        where id in (select id from regop_payment_operation_base where period_id={curPeriod.Id});
                        delete from regop_payment_operation_base where period_id={curPeriod.Id};
                        delete from REGOP_PERS_ACC_CALC_PARAM where object_create_date >= :lastPeriodDate;
                        delete from regop_period where id={curPeriod.Id};
                        delete from regop_charge_transfer_period_{lastPeriod.Id} tr
                        where exists (select 1 from regop_pers_acc_charge_period_{lastPeriod.Id} ch where ch.guid=tr.target_guid);
                        update regop_pers_acc_charge_period_{lastPeriod.Id} set is_fixed=false;
                        update regop_period set cis_closed=false where id={lastPeriod.Id};";

                        if (isDeleteSnapshot)
                        {
                            rollbackSql.Append($@"
                            delete from regop_pers_paydoc_snap where snapshot_id in (select id from regop_payment_doc_snapshot where period_id={lastPeriod.Id});
                            delete from regop_payment_doc_snapshot where period_id ={lastPeriod.Id};");
                        }

                        session.CreateSQLQuery(rollbackSql)
                            .SetParameter("lastPeriodDate", lastPeriod.ObjectCreateDate)
                            .ExecuteUpdate();

                        session.CreateSQLQuery(createResult).ExecuteUpdate();

                        var closeHistory = new PeriodCloseRollbackHistory
                        {
                            PeriodName = curPeriod.Name,
                            Date = DateTime.Now,
                            User = user,
                            Reason = reason,
                            Period = lastPeriod
                        };

                        perCloseRollbackHistDomain.Save(closeHistory);
                    });
            }
            finally
            {
                this.Container.Release(sessionProvider);
                this.Container.Release(perCloseRollbackHistDomain);
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult ListPeriodsByPersonalAccount(BaseParams baseParams)
        {
            var personalAccountId = baseParams.Params.GetAsId();
            ListDataResult result = null;
            this.Container.UsingForResolved<IDomainService<PersonalAccountPeriodSummary>>((container, domain) =>
            {
                result = domain.GetAll()
                    .Where(x => x.PersonalAccount.Id == personalAccountId)
                    .Select(x => x.Period)
                    .ToListDataResult(baseParams.GetLoadParam(), container);
            });
            return result ?? new ListDataResult{ Success = false, Message = "Не удалось получить список периодов" };
        }

        private void AfterCreateAction(ChargePeriod entity)
        {
            /*
             * 1) При открытии периода мы создаем операции начисления в счете начислений дома,
             * проставляя исходящее сальдо из предыдущего периода
             * 2) Для ЛС создаем summary по ЛС за новый период с заполненными сальдо 
             */
            // 1)
            Stopwatch st = Stopwatch.StartNew();
            this.CreateRealtyObjectChargeOperations(entity);

            // 2)
            this.CreatePersonalAccountSummaries(entity);
            st.Stop();
        }

        private void CreatePersonalAccountSummaries(ChargePeriod entity)
        {
            this.RealtyObjectRegopOperationService.CreatePersonalAccountSummaries(entity, null);
        }

        private void CreateRealtyObjectChargeOperations(ChargePeriod entity)
        {
            this.RealtyObjectRegopOperationService.CreateRealtyObjectChargeOperations(entity, null);
        }

        protected internal readonly IDomainService<ChargePeriod> PeriodDomain;
        protected internal readonly IWindsorContainer Container;
        protected internal readonly IRealtyObjectRegopOperationService RealtyObjectRegopOperationService;
        protected internal readonly IDomainService<PersonalAccountCalculatedParameter> CalculatedRepo;
        protected readonly IChargePeriodRepository ChargePeriodRepo;
    }
}