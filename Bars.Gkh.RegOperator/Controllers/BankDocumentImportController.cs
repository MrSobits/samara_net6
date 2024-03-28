
namespace Bars.Gkh.RegOperator.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;

    using Bars.Gkh.Utils.PerformanceLogging;

    using DomainService.BankDocumentImport;

    using Entities;

    using Enums;
    using System;

    using Bars.Gkh.Authentification;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    using Dapper;

    /// <summary>
    ///     Контроллер
    /// </summary>
    public class BankDocumentImportController : B4.Alt.DataController<BankDocumentImport>
    {
        /// <summary>
        ///     Сервис для выполнения действий
        /// </summary>
        public IBankDocumentImportService Service { get; set; }

        /// <summary>
        /// Провайдер сессий
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Проверить реестр оплат
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult CheckPayments(BaseParams baseParams)
        {
            var result = this.Service.TaskCheckPayments(baseParams);
            return result.Success
                ? new JsonNetResult(new { sucess = true, message = result.Message })
                : this.JsFailure(result.Message);
        }

        /// <summary>
        ///     Подтвердить оплаты
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns></returns>
        public ActionResult AcceptPayments(BaseParams baseParams)
        {
            var inProgress = this.CheckBankDocumentImportAsInProgress(baseParams);
            if (inProgress)
            {
                return this.JsFailure("Запись уже подтверждается");
            }

            var markResult = this.MarkBankDocumentImportAsInProgress(baseParams);
            if (!markResult.Success)
            {
                return this.JsFailure(markResult.Message);
            }

            var result = this.Service.TaskAcceptDocuments(baseParams);
            return result.Success
                ? new JsonNetResult(new {sucess = true, message = result.Message})
                : this.JsFailure(result.Message);
        }

        /// <summary>
        ///     Удаление реестра оплат
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат удаления реестра оплат</returns>
        public ActionResult DeletePayments(BaseParams baseParams)
        {
            var result = this.Service.DeletePayments(baseParams);
            return result.Success
                ? new JsonNetResult(new {sucess = true, message = result.Message})
                : this.JsFailure(result.Message);
        }

        /// <summary>
        ///     Отмена подтверждения оплат
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат отмены подтверждения оплат реестра</returns>
        public ActionResult CancelPayments(BaseParams baseParams)
        {
            var inProgress = this.CheckBankDocumentImportAsInProgress(baseParams, false);
            if (inProgress)
            {
                return this.JsFailure("Запись уже отменяется");
            }

            var markResult = this.MarkBankDocumentImportAsInProgress(baseParams, false);
            if (!markResult.Success)
            {
                return this.JsFailure(markResult.Message);
            }

            var result = this.Service.TaskCancelDocuments(baseParams);
            return result.Success
                ? new JsonNetResult(new {sucess = true, message = result.Message})
                : this.JsFailure(result.Message);
        }

        private IDataResult MarkBankDocumentImportAsInProgress(BaseParams baseParams, bool acceptOperation = true)
        {
            var packetIds = baseParams.Params.GetAs<long[]>("packetIds", ignoreCase: true);
            var bankDocumentImportId = baseParams.Params.GetAs<int>("bankDocumentImportId");
            var importedPaymentIds = baseParams.Params.GetAs<long[]>("importedPaymentIds", ignoreCase: true);
            var bankDocumentImportDomain = this.Container.ResolveDomain<BankDocumentImport>();
            var innerPaymentsDomain = this.Container.ResolveDomain<ImportedPayment>();
            var factory = this.Container.Resolve<IPerformanceLoggerFactory>();
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var logger = factory.GetLogger();
            var collector = factory.GetCollector();

            var allowAccountStates = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.BankDocumentImportAccountStates.Select(x => x.Id).ToArray();
            using (this.Container.Using(bankDocumentImportDomain, innerPaymentsDomain, factory, logger, collector, userManager))
            {
                logger.StartTimer("MarkBankDocumentImportAsInProgress");
                try
                {
                    var session = this.SessionProvider.GetCurrentSession();
                    var connection = session.Connection;

                    var filterByAccountState = acceptOperation
                            ? (allowAccountStates.Length > 0 
                                ? $@" and exists(select 1 from regop_pers_acc acc where acc.id = p.pers_acc_id and acc.state_id in ({allowAccountStates.AggregateWithSeparator(x => x, ",")}))"
                                : " and false")
                            : string.Empty;

                    var bankIdConditionString = packetIds.IsNotEmpty()
                        ? packetIds.AggregateWithSeparator(x => x.ToString(), ",")
                        : bankDocumentImportId > 0
                            ? bankDocumentImportId.ToString()
                            : "";

                    var sql = $@"select p.id
                                from regop_imported_payment p 
                                where 
                                bank_doc_id in ({bankIdConditionString})
                                and pad_state = {(int)ImportedPaymentPersAccDeterminateState.Defined}
                                {filterByAccountState}
                                {(importedPaymentIds.IsNotEmpty() 
                                    ? $" and p.Id in ({importedPaymentIds.AggregateWithSeparator(x => x.ToString(), ",")})" 
                                    : string.Empty)}";

                    var importPaymentIds = connection.Query<long>(sql).ToList();

                    if (importPaymentIds.Count == 0)
                    {
                        return new BaseDataResult(false, "Не выбраны платежи, у которых статус определения ЛС = \"Определен\" и статус подтверждения оплат = \"Не подтвержден\", " +
                            "либо статус ЛС не соответствует настройкам");
                    }

                    var user = userManager.GetActiveUser();

                    this.Container.InTransaction(() => {
                        var query = session
                            .CreateQuery(
                                "update BankDocumentImport b set State = :state, PaymentConfirmationState = :pc_state " +
                                "where b.Id in (:ids) and b.PaymentConfirmationState in (:f_state)")
                            .SetParameter("state", PaymentOrChargePacketState.InProgress)
                            .SetParameter("pc_state", acceptOperation ? PaymentConfirmationState.WaitingConfirmation : PaymentConfirmationState.WaitingCancellation)
                            .SetParameterList(
                                "f_state",
                                acceptOperation
                                    ? new[]
                                    {
                                        PaymentConfirmationState.NotDistributed,
                                        PaymentConfirmationState.PartiallyDistributed
                                    }
                                    : new[]
                                    {
                                        PaymentConfirmationState.PartiallyDistributed,
                                        PaymentConfirmationState.Distributed
                                    });

                        if (packetIds.IsNotEmpty())
                        {
                            query.SetParameterList("ids", packetIds);
                        }

                        if (bankDocumentImportId > 0)
                        {
                            query.SetParameterList("ids", new[] { bankDocumentImportId });
                        }

                        logger.StartTimer("SetBankDocumentImportState");
                        query.ExecuteUpdate();
                        logger.StopTimer("SetBankDocumentImportState");

                        //GeneralStateHistory
                        var finalState = acceptOperation
                            ? ImportedPaymentPaymentConfirmState.WaitingConfirmation
                            : ImportedPaymentPaymentConfirmState.WaitingCancellation;

                        query = session.CreateSQLQuery(
                                $@"insert into GKH_GENERAL_STATE_HISTORY 
                            (
	                            object_version, object_create_date, object_edit_date, 
	                            CHANGE_DATE, ENTITY_ID, ENTITY_TYPE, CODE, 
	                            START_STATE, FINAL_STATE, 
	                            USER_NAME, USER_LOGIN
                            )
                            select 
	                            0, now(), now(),
	                            :now, p.id, 'Bars.Gkh.RegOperator.Entities.ImportedPayment', 'regop_imported_payment',
	                            p.pc_state as START_STATE, '{(int)finalState}',
	                            :userName, :userLogin
                            from regop_imported_payment p
                            where p.id in (:ids)
                            ")
                            .SetParameter("userName", user.Name)
                            .SetParameter("userLogin", user.Login)
                            .SetParameter("now", DateTime.UtcNow)
                            .SetParameterList("ids", importPaymentIds);

                        logger.StartTimer("SaveImportedPaymentStateHistory");
                        query.ExecuteUpdate();
                        logger.StopTimer("SaveImportedPaymentStateHistory");

                        //ImportedPayment
                        query = session.CreateSQLQuery(
                                $@"update regop_imported_payment p 
                                set pc_state = :state
                                where p.id in (:ids)")
                            .SetParameter("state", finalState)
                            .SetParameterList("ids", importPaymentIds);

                        logger.StartTimer("SetImportedPaymentState");
                        query.ExecuteUpdate();
                        logger.StopTimer("SetImportedPaymentState");

                        logger.StartTimer("Commit");
                        logger.StopTimer("Commit");
                    });
                }
                catch (Exception e)
                {
                    return new BaseDataResult(false, e.Message);
                }
                finally
                {
                    logger.StopTimer("MarkBankDocumentImportAsInProgress");
                    logger.SaveLogs(collector, x => x.OrderByDescending(y => y.TimeSpan).First());
                    logger.ClearSession();
                }
            }

            return new BaseDataResult();
        }

        private bool CheckBankDocumentImportAsInProgress(BaseParams baseParams, bool acceptOperation = true)
        {
            var packetIds = baseParams.Params.GetAs<long[]>("packetIds", ignoreCase: true);
            var bankDocumentImportDomain = this.Container.ResolveDomain<BankDocumentImport>();
            using (this.Container.Using(bankDocumentImportDomain))
            {
                return bankDocumentImportDomain.GetAll()
                    .WhereIf(acceptOperation, x => x.PaymentConfirmationState == PaymentConfirmationState.WaitingConfirmation)
                    .WhereIf(!acceptOperation, x => x.PaymentConfirmationState == PaymentConfirmationState.WaitingCancellation)
                    .Where(x => x.State == PaymentOrChargePacketState.InProgress)
                    .Any(x => packetIds.Contains(x.Id));
            }
        }

        /// <summary>
        ///     Подтверждение внутренних оплат
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns></returns>
        public ActionResult AcceptInternalPayments(BaseParams baseParams)
        {
            var tempResult = this.MarkBankDocumentImportAsInProgress(baseParams);
            if (tempResult.Success)
            {
                var result = this.Service.AcceptInternalPayments(baseParams);
                return result.Success
                    ? new JsonNetResult(new {sucess = true, message = result.Message})
                    : this.JsFailure(result.Message.Replace("\r\n", "<br />"));
            }
            else
            {
                return this.JsFailure(tempResult.Message.Replace("\r\n", "<br />"));
            }
        }

        /// <summary>
        /// Отмена внутренних оплат
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns></returns>
        public ActionResult CancelInternalPayments(BaseParams baseParams)
        {
            var tempResult = this.MarkBankDocumentImportAsInProgress(baseParams, false);
            if (tempResult.Success)
            {
                return this.Service.CancelInternalPayments(baseParams).ToJsonResult();
            }
            else
            {
                return this.JsFailure(tempResult.Message.Replace("\r\n", "<br />"));
            }
        }

        public ActionResult Import(BaseParams baseParams)
        {
            ActionResult actionResult;
            try
            {
                var result = Service.TaskImport(baseParams);
                if (result.Success)
                {
                    actionResult = new JsonNetResult(new
                    {
                        success = result.Success,
                        title = string.Empty,
                        message = "Задачи успешно поставлены в очередь на выполнение"
                    });
                }
                else
                {
                    actionResult = JsonNetResult.Failure(result.Message);
                }
            }
            catch (Exception e)
            {
                actionResult = JsonNetResult.Failure(e.Message);
            }

            return actionResult;
        }
    }
}