namespace Bars.Gkh.RegOperator.Controllers.Distribution
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.B4.Utils;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Domain;
    using Domain.Repository;
    using Gkh.Domain;
    using RegOperator.Distribution;

    /// <summary>
    /// Контроллер для распределений
    /// </summary>
    public class DistributionController : BaseController
    {
        /// <summary>
        /// Провайдер распределений со счетов НВС
        /// </summary>
        public IDistributionProvider Provider { get; set; }

        /// <summary>
        /// Сервис для подтверждения/отмены банковских выписок
        /// </summary>
        public IDistributionService Service { get; set; }

        /// <summary>
        /// Получить список всех распределений
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult List(BaseParams baseParams)
        {
            return this.Provider.List(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Валидация
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult Validate(BaseParams baseParams)
        {
            return this.Provider.Validate(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Валидация, которая предусматривает продолжение работы.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult SoftValidate(BaseParams baseParams)
        {
            return this.Provider.SoftValidate(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Валидация при распределении, которая предусматривает продолжение работы.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult SoftApplyValidate(BaseParams baseParams)
        {
            return this.Provider.SoftApplyValidate(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult Apply(BaseParams baseParams)
        {
            var result = this.Service.Apply(baseParams);

            return result.Success
                ? JsonNetResult.Message(result.Message)
                : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Отменить распределение
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult Undo(BaseParams baseParams)
        {
            var result = this.Service.Undo(baseParams);

            return result.Success 
                ? JsonNetResult.Message(result.Message) 
                : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Отменить распределение частично
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult UndoPartially(BaseParams baseParams)
        {
            return this.Service.UndoPartially(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Отменить зачисление
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult UndoCheckin(BaseParams baseParams)
        {
            return this.Provider.UndoCheckin(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Отменить либо удалить распределения
        /// </summary>
        public ActionResult UndoOperationOrUndoCheckin(BaseParams prms)
        {
            return this.Provider.UndoOperationOrUndoCheckin(prms).ToJsonResult();
        }

        /// <summary>
        /// Получить список объектов с распределенными суммами
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListDistributionObjs(BaseParams baseParams)
        {
            return this.Provider.ListDistributionObjs(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Вернуть список объектов распределения для автораспределений
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListAutoDistributionObjs(BaseParams baseParams)
        {
            return this.Provider.ListAutoDistributionObjs(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Метод проверяет дату выписки
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult CheckDate(BaseParams baseParams)
        {
            var date = baseParams.Params.GetAs<DateTime?>("date", (DateTime?)null);
            var dates = baseParams.Params.GetAs<DateTime[]>("dates");

            var chargeService = this.Resolve<IChargePeriodRepository>();

            var currentPeriod = chargeService.GetCurrentPeriod();
            var validateDateResult = date.HasValue && currentPeriod.GetEndDate().Date < date.Value.Date;
            var validateDatesResult = dates.IsNotEmpty() && dates.Any(x => currentPeriod.GetEndDate().Date < x.Date);

            if (validateDateResult || validateDatesResult)
            {
                return JsonNetResult.Failure("Дата поступления больше даты окончания текущего периода. Распределение невозможно.");
            }

            return this.JsSuccess();
        }

        /// <summary>
        /// Получить наименование плательщика
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Плательщик</returns>
        public ActionResult GetOriginatorName(BaseParams baseParams)
        {
            return this.Provider.GetOriginatorName(baseParams).ToJsonResult();
        }
    }
}