namespace Bars.Gkh.RegOperator.Tasks.Period.Executors
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Tasks.Common.Service;
    using B4.Modules.Tasks.Common.Utils;
    using Castle.Windsor;
    using Domain;
    using Domain.Repository;
    using DomainModelServices.PersonalAccount;
    using DomainService.RealityObjectAccount;
    using Microsoft.Extensions.Logging;
    
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.PersonalAccountGroup;
    using Bars.Gkh.RegOperator.Report.ReportManager;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Gkh.Exceptions;

    using NHibernate;

    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Закрытие периода (Этап 3)
    /// </summary>
    public class PeriodCloseTaskExecutor_Step3 : ITaskExecutor
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IWindsorContainer container;
        private readonly IRealtyObjectRegopOperationService summaryCreator;
        private readonly IChargePeriodRepository periodRepo;
        private readonly IDomainService<ChargePeriod> periodDomain;
        private readonly ISessionProvider sessionProvider;
        private readonly ILogger logger;
        private readonly IPersonalAccountRecalcEventManager recalcEventManager;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="summaryCreator">Провайдер разреза дома на период</param>
        /// <param name="periodRepo">Репозиторий периодов</param>
        /// <param name="periodDomain">Домен-сервис периодов</param>
        /// <param name="sessionProvider">Провайдер сессии</param>
        /// <param name="logger">Логгер</param>
        /// <param name="recalcEventManager">Интерфейс создания отсечек перерасчета для ЛС</param>
        public PeriodCloseTaskExecutor_Step3(IWindsorContainer container,
            IRealtyObjectRegopOperationService summaryCreator,
            IChargePeriodRepository periodRepo,
            IDomainService<ChargePeriod> periodDomain,
            ISessionProvider sessionProvider,
            ILogger logger,
            IPersonalAccountRecalcEventManager recalcEventManager)
        {
            this.container = container;
            this.summaryCreator = summaryCreator;
            this.periodRepo = periodRepo;
            this.periodDomain = periodDomain;
            this.sessionProvider = sessionProvider;
            this.logger = logger;
            this.recalcEventManager = recalcEventManager;
        }

        #region Implementation of ITaskExecutor

        /// <summary>
        /// Выполнить задачу
        /// </summary>
        /// <param name="params">Параметры</param>
        /// <param name="ctx">Контекст выполнения</param>
        /// <param name="indicator">Индикатор</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns></returns>
        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            using (var tr = this.container.Resolve<IDataTransaction>())
            {
                var period = this.periodRepo.GetCurrentPeriod(false);

                try
                {
                    period.IsClosed = true;
                    period.IsClosing = false;

                    var endDate = period.GetEndDate();
                    period.EndDate = endDate;

                    this.periodDomain.Update(period);

                    var ci = CultureInfo.GetCultureInfo("ru-RU");

                    var newChargePeriod = new ChargePeriod(
                        endDate.AddDays(1).ToString("yyyy MMMM", ci),
                        endDate.AddDays(1));
                    indicator.Indicate(null, 0, "Создание нового периода");
                    this.periodDomain.Save(newChargePeriod);
                    ChargePeriodProvider.Repository.InvalidateCache();

                    this.Clean(period);
                    //создаем партиции под новый период
                    this.CreateNewPartitions(newChargePeriod);

                    this.RefreshRecalcHistoryCharge();

                    this.CreateDayDocTemplates(newChargePeriod);

                    tr.Commit();

                    indicator.Indicate(null, 0, "Создание данных для нового периода");
                    this.logger.LogInformation("Создание данных для нового периода. Заключительный этап");

                    int i = 0;

                    while (i++ < 3)
                    {
                        try
                        {
                            this.summaryCreator.CreateAllPersonalAccountSummaries(newChargePeriod, indicator);
                            this.summaryCreator.CreateAllRealtyObjectChargeOperations(indicator);
                        }
                        catch (Exception e)
                        {
                            if (i + 1 < 4) continue;
                            throw new GkhException("Ошибка создания нового периода после трех попыток", e);
                        }

                        break;
                    }

                    this.logger.LogInformation("Период закрыт");
                    indicator.Indicate(null, 100, "Период закрыт");
                }
                catch (Exception e)
                {
                    this.logger.LogError(e, "Ошибка закрытия периода");

                    if (tr.IsActive)
                    {
                        tr.Rollback();
                    }

                    period.IsClosing = false;
                    this.periodDomain.Update(period);

                    throw;
                }
                finally
                {
                    var cfg = @params.Params.GetAs<OperationLockConfig>("operationLock");
                    if (cfg != null && cfg.Enabled)
                    {
                        PeriodCloseTableLocker.Unlock(this.container);
                    }
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Код исполнителя
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        /// <summary>
        /// Очистка данных в закрываемом периоде
        /// </summary>
        /// <param name="period">Закрываемый период</param>
        private void Clean(IPeriod period)
        {
            var session = this.sessionProvider.GetCurrentSession();

            var query = session.CreateQuery(
                "delete from CalculationParameterTrace as t"
                + "   where ChargePeriod.Id=(:periodId) and not exists"
                + "   (select c from PersonalAccountCharge as c"
                + "    where c.ChargePeriod.Id=(:periodId) and t.CalculationGuid = c.Guid)");
            query.SetInt64("periodId", period.Id);
            query.ExecuteUpdate();
            
            query = session.CreateQuery("delete from PersonalAccountCalcParam_tmp as t");
            query.ExecuteUpdate();

            query = session.CreateQuery(
                "delete from PersonalAccountCharge as t"
                + "   where ChargePeriod.Id=(:periodId) and IsFixed = false");
            query.SetInt64("periodId", period.Id);
            query.ExecuteUpdate();

            query = session.CreateQuery("update from PersonalAccountCharge set Packet = null where ChargePeriod.Id=(:periodId)");
            query.SetInt64("periodId", period.Id);
            query.ExecuteUpdate();

            query = session.CreateSQLQuery("truncate REGOP_UNACCEPT_C_PACKET");
            query.ExecuteUpdate();

            this.recalcEventManager.ClearPersistedEvents();

            var formedInOpenPeriodSystemGroup = this.container.Resolve<IGroupManager>("FormedInOpenPeriodSystemGroup");
            formedInOpenPeriodSystemGroup.RemoveAllFromGroup(period);

            this.container.Release(formedInOpenPeriodSystemGroup);
        }

        /// <summary>
        /// Обновляем содержимое материализованного представления с перерасчетами (recalc_type in (10,20))
        /// </summary>
        private void RefreshRecalcHistoryCharge()
        {
            var session = this.sessionProvider.GetCurrentSession();
            var sql = $@" REFRESH MATERIALIZED VIEW public.REGOP_RECALC_HISTORY_CHARGE;";
            var query = session.CreateSQLQuery(sql);
            query.ExecuteUpdate();
        }

        /// <summary>
        /// Создает партиции за новый период
        /// </summary>
        /// <param name="period"></param>
        private void CreateNewPartitions(ChargePeriod period)
        {
            var session = this.sessionProvider.GetCurrentSession();
            
            //ключ - имя мастер-таблицы, значение - поле по которому идет разъбиение
            var listPartitionedTablesByPeriodId = new Dictionary<string, string>
            {
                {"regop_pers_acc_charge", "period_id"},
                {"regop_pers_acc_change", "period_id"},
                {"regop_calc_param_trace", "period_id"},
                {"regop_transfer", "period_id"},
                {"regop_charge_transfer", "period_id"},
                {"regop_reality_transfer", "period_id"},
                {"regop_pers_acc_period_summ", "period_id"},
                {"regop_recalc_history", "recalc_period_id"},
            };


            foreach (var tableName in listPartitionedTablesByPeriodId)
            {
                //создаем партицию с указанием наследуемой таблицы
                var sql =
                    $@" CREATE TABLE IF NOT EXISTS public.{tableName.Key}_period_{period.Id}
					          (LIKE {tableName.Key
                        } INCLUDING ALL, 
						      CONSTRAINT {tableName.Key}_constraint 
					          CHECK ({tableName.Value} = {period.Id} ))
					          INHERITS ({tableName.Key}); ";
                var query = session.CreateSQLQuery(sql);
                query.ExecuteUpdate();

                var transferTables = new[] { "regop_transfer", "regop_charge_transfer", "regop_reality_transfer" };
                var functionName = new Dictionary<string, string>
                {
                    {"regop_transfer", "delete_from_transfer()"},
                    {"regop_charge_transfer", "delete_from_ch_transfer()"},
                    {"regop_reality_transfer", "delete_from_ro_transfer()"},
                };
                
                if (transferTables.Contains(tableName.Key))
                {
                    var sqlTransfer =
                        $@"CREATE TRIGGER delete_{tableName.Key}_period_{period.Id}_tr
                               BEFORE DELETE
                               ON public.{tableName.Key}_period_{period.Id}
                               FOR EACH ROW
                               EXECUTE PROCEDURE {functionName[tableName.Key]};";

                    var queryTransfer = session.CreateSQLQuery(sqlTransfer);
                    queryTransfer.ExecuteUpdate();
                }
            }

            this.CreateConstraintPartitions(period, session);
        }

        private void CreateConstraintPartitions(ChargePeriod period, ISession session)
        {
            //  regop_pers_acc_charge
            var sqlConstraint =
                $@"ALTER TABLE public.regop_pers_acc_charge_period_{period.Id}
                                      ADD CONSTRAINT fk_regop_pers_acc_charge_{
                    period.Id}_period FOREIGN KEY (period_id)
                                      REFERENCES public.regop_period (id)";
            var query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            sqlConstraint =
                $@"ALTER TABLE public.regop_pers_acc_charge_period_{period.Id}
                                   ADD CONSTRAINT fk_regop_pers_acc_charge_{
                    period.Id}_pa FOREIGN KEY (pers_acc_id)
                                   REFERENCES public.regop_pers_acc (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            // regop_pers_acc_change
            sqlConstraint =
                $@"ALTER TABLE regop_pers_acc_change_period_{period.Id}
                                   ADD CONSTRAINT fk_regop_pers_acc_change_{period.Id
                    }_doc FOREIGN KEY (doc_id)
                                   REFERENCES public.b4_file_info (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            sqlConstraint =
                $@"ALTER TABLE regop_pers_acc_change_period_{period.Id}
                                   ADD CONSTRAINT fk_regop_pers_acc_change_{period.Id
                    }_pa FOREIGN KEY (acc_id)
                                   REFERENCES public.regop_pers_acc (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            // regop_transfer
            sqlConstraint =
                $@"ALTER TABLE regop_transfer_period_{period.Id}
                                   ADD CONSTRAINT fk_regop_transfer_period_{period.Id
                    }_op FOREIGN KEY (op_id)
                                   REFERENCES public.regop_money_operation (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            sqlConstraint =
                $@"ALTER TABLE regop_transfer_period_{period.Id}
                                   ADD CONSTRAINT fk_regop_transfer_period_{period.Id
                    }_owner_id FOREIGN KEY (owner_id)
                                   REFERENCES public.regop_pers_acc (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            // regop_charge_transfer
            sqlConstraint =
                $@"ALTER TABLE regop_charge_transfer_period_{period.Id}
                                   ADD CONSTRAINT fk_regop_ch_transfer_period_{period.Id
                    }_op FOREIGN KEY (op_id)
                                   REFERENCES public.regop_money_operation (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            sqlConstraint =
                $@"ALTER TABLE regop_charge_transfer_period_{period.Id}
                                   ADD CONSTRAINT fk_regop_ch_transfer_period_{period.Id
                    }_owner_id FOREIGN KEY (owner_id)
                                   REFERENCES public.regop_pers_acc (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            // regop_reality_transfer
            sqlConstraint =
                $@"ALTER TABLE regop_reality_transfer_period_{period.Id}
                                   ADD CONSTRAINT fk_regop_ro_transfer_period_{period.Id
                    }_op FOREIGN KEY (op_id)
                                   REFERENCES public.regop_money_operation (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            sqlConstraint =
                $@"ALTER TABLE regop_reality_transfer_period_{period.Id}
                                   ADD CONSTRAINT fk_regop_ro_transfer_period_{period.Id
                    }_owner_id FOREIGN KEY (owner_id)
                                   REFERENCES public.regop_ro_payment_account (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            sqlConstraint =
                $@"ALTER TABLE regop_reality_transfer_period_{period.Id}
                                   ADD CONSTRAINT fk_regop_ro_transfer_period_{period.Id
                    }_copy_transfer_id FOREIGN KEY (copy_transfer_id)
                                   REFERENCES public.regop_transfer_period_{period.Id} (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            // regop_pers_acc_period_summ
            sqlConstraint =
                $@"ALTER TABLE regop_pers_acc_period_summ_period_{period.Id}
                                   ADD CONSTRAINT fk_regop_pers_acc_period_summ_{
                    period.Id}_pa FOREIGN KEY (account_id)
                                   REFERENCES public.regop_pers_acc (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            sqlConstraint =
                $@"ALTER TABLE regop_pers_acc_period_summ_period_{period.Id}
                                 ADD CONSTRAINT fk_regop_pers_acc_period_summ_{
                    period.Id}_period FOREIGN KEY (period_id)
                                 REFERENCES public.regop_period (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            // regop_recalc_history
            sqlConstraint =
                $@"ALTER TABLE regop_recalc_history_period_{period.Id}
                             ADD CONSTRAINT fk_regop_recalc_history_{period.Id
                    }_pa FOREIGN KEY (account_id)
                             REFERENCES public.regop_pers_acc (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            sqlConstraint =
                $@"ALTER TABLE regop_recalc_history_period_{period.Id}
                             ADD CONSTRAINT fk_regop_recalc_history_{period.Id
                    }_per FOREIGN KEY (calc_period_id)
                             REFERENCES public.regop_period (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();

            sqlConstraint =
                $@"ALTER TABLE regop_recalc_history_period_{period.Id}
                             ADD CONSTRAINT fk_regop_recalc_history_{period.Id}_recalc_per 
                             FOREIGN KEY (recalc_period_id)
                             REFERENCES public.regop_period (id)";
            query = session.CreateSQLQuery(sqlConstraint);
            query.ExecuteUpdate();
        }

        private void CreateDayDocTemplates(ChargePeriod period)
        {
            var templateRepo = this.container.ResolveRepository<PaymentDocumentTemplate>();

            try
            {
                using (var reportManager = new PaymentDocReportManager(templateRepo))
                {
                    reportManager.CreateTemplateCopyIfNotExist(period);
                }
            }
            finally
            {
                this.container.Release(templateRepo);
            }
        }
    }
}