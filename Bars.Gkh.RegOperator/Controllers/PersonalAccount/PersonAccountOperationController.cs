namespace Bars.Gkh.RegOperator.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Domain.PersonalAccountOperations;
    using Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;

    /// <summary>
    /// Контроллер операций над лицевым счетом
    /// </summary>
    public class PersonAccountOperationController : BaseController
    {
        public IPersonalAccountOperationService OperationService { get; set; }

        public IPersonalAccountMerger MergerService { get; set; }

        public ActionResult TrySetOpenRecord(BaseParams baseParams)
        {
            var result = this.OperationService.TrySetOpenRecord(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Закрытие лицевого счета
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        public ActionResult ClosePersonalAccount(BaseParams baseParams)
        {
            var result = this.OperationService.ClosePersonalAccount(baseParams);
            return result.Success 
                ? this.JsSuccess() 
                : new JsonNetResult(new { success = false, message = result.Message, data = result.Data });
        }

        /// <summary>
        /// Экспортировать сальдо ЛС
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        public ActionResult ExportExcelSaldo(BaseParams baseParams)
        {
            var result = this.OperationService.ExportExcelSaldo(baseParams);
            if (result.Success)
            {
                var reportResult = result as ReportResult;
                return new ReportStreamResult(reportResult.ReportStream, reportResult.FileName);
            }

            return result.ToJsonResult();

        }

        /// <summary>
        /// Слияние счетов
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        public ActionResult MergeAccounts(BaseParams baseParams)
        {
            return new JsonNetResult(this.MergerService.MergeAccounts(baseParams));
        }

        /// <summary>
        /// Удаление лицевых счетов
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        public ActionResult RemoveAccounts(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("accountIds").ToLongArray();
            var result = this.OperationService.RemoveAccounts(ids);

            return result.Success ? this.JsSuccess(result.Data) : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Вернуть список статусов для закрытия лс
        /// </summary>
        public ActionResult GetCloseStates(BaseParams baseParams)
        {
            var result = this.OperationService.GetCloseStates();
            return this.JsSuccess(result);
        }

        /// <summary>
        /// Повторное открытие лицевого счета
        /// </summary>
        /// <param name="baseParams">Параметры клиента</param>
        /// <returns>Результат повторного открытия счета</returns>
        public ActionResult ManuallyRecalc(BaseParams baseParams)
        {
            var manuallyRecalcOperation = this.Container.Resolve<IPersonalAccountOperation>(ManuallyRecalcOperation.Key);

            try
            {
                var result = manuallyRecalcOperation.Execute(baseParams);
                return new JsonNetResult(new { success = result.Success, message = result.Message });
            }
            finally
            {
                this.Container.Release(manuallyRecalcOperation);
            }
        }

        /// <summary>
        /// Применение распределения зачета средств за ранее выполненные работы
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult ApplyPerformedWorkDistribution(BaseParams baseParams)
        {
            return new JsonNetResult(this.OperationService.ApplyPerformedWorkDistribution(baseParams));
        }

        /// <summary>
        /// Получение доли собственности по лицевомц счету
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        public ActionResult RoomAccounts(BaseParams baseParams)
        {
            return new JsonNetResult(this.OperationService.RoomAccounts(baseParams));
        }

        /// <summary>
        /// Получение информации по лицевым счетам для зачета средств за ранее выполненные работы
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult GetAccountsInfoForPerformedWorkDistribution(BaseParams baseParams)
        {
            return new JsonNetResult(this.OperationService.GetAccountsInfoForPerformedWorkDistribution(baseParams));
        }
    }
}