namespace Bars.Gkh.RegOperator.Controllers
{
    using System;
    using System.Collections;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.B4.Application;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.PersonalAccountGroup;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Domain;
    using Domain.Repository;
    using DomainModelServices;
    using DomainService.Period;

    /// <summary>
    /// Контроллер для Период начислений
    /// </summary>
    public class ChargePeriodController : B4.Alt.DataController<ChargePeriod>
    {
        /// <summary>
        /// Сервис для закрытия периода начислений
        /// </summary>
        public IChargePeriodCloseService PeriodCloseService { get; set; }

        /// <summary>
        /// Сервис для Период начислений
        /// </summary>
        public IChargePeriodService PeriodService { get; set; }

        /// <summary>
        /// Закрыть текущий период
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult CloseCurrentPeriod(BaseParams baseParams)
        {
            var data = PeriodCloseService.CloseCurrentPeriod(baseParams);

            return data.Success ? JsSuccess(data) : JsFailure(data.Message);
        }

        /// <summary>
        /// Открыть начальный период
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult CreateFirstPeriod(BaseParams baseParams)
        {
            return new JsonNetResult(PeriodService.CreateFirstPeriod());
        }

        /// <summary>
        /// Получить открытый период
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult GetOpenPeriod(BaseParams baseParams)
        {
            var period = DomainService.GetAll().FirstOrDefault(x => !x.IsClosed);

            return JsSuccess(period);
        }

        /// <summary>
        /// Получить список закрытых периодов
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult ListClosed(BaseParams baseParams)
        {
            var result = (ListDataResult)PeriodService.ListClosedPeriods(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Проверить дату на вхождение в текущий период
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult CheckDate(BaseParams baseParams)
        {
            var date = baseParams.Params.GetAs<DateTime>("date");

            var period = Resolve<IChargePeriodRepository>().GetCurrentPeriod();

            if (period.InPeriod(date))
            {
                return JsonNetResult.Failure("Дата поступления больше даты окончания текущего периода. Распределение невозможно.");
            }

            return JsonNetResult.Success;
        }

        /// <summary>
        /// Получить дату начала первого периода
        /// </summary>
        /// <returns>Дата начала первого периода</returns>
        public ActionResult GetStartDateOfFirstPeriod()
        {
            var result = this.PeriodService.GetStartDateOfFirstPeriod();
            return new JsonNetResult(result);
        }

        /// <summary>
        /// Получить количество документов сформированных в открытом периоде
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCountDocumentFormedInOpenPerod()
        {
            var formedInOpenPeriodSystemGroup = ApplicationContext.Current.Container.Resolve<IGroupManager>("FormedInOpenPeriodSystemGroup");

            try
            {
                return new JsonNetResult(formedInOpenPeriodSystemGroup.GetCountByGroup());
            }
            finally
            {
                this.Container.Release(formedInOpenPeriodSystemGroup);
            }

        }

        /// <summary>
        /// Откатить закрытый месяц
        /// </summary>
        public ActionResult RollbackClosedPeriod(BaseParams baseParams)
        {
            return new JsonNetResult(this.PeriodService.RollbackClosedPeriod(baseParams));
        }

        /// <summary>
        /// Список периодов ЛС
        /// </summary>
        /// <param name="baseParams">id - идентификатор ЛС</param>
        public ActionResult ListPeriodsByPersonalAccount(BaseParams baseParams)
        {
            return this.PeriodService.ListPeriodsByPersonalAccount(baseParams).ToJsonResult();
        }
    }
}