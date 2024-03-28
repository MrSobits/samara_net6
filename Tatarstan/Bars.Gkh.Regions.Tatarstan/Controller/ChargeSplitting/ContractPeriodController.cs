namespace Bars.Gkh.Regions.Tatarstan.Controller.ChargeSplitting
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Контроллер <see cref="ContractPeriod"/>
    /// </summary>
    public class ContractPeriodController : B4.Alt.DataController<ContractPeriod>
    {
        /// <summary>
        /// Сервис работы с расщеплением платежей
        /// </summary>
        public IChargeSplittingService ChargeSplittingService { get; set; }

        /// <summary>
        /// Сформировать отчетный период
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат</returns>
        public ActionResult CreatePeriod(BaseParams baseParams)
        {
            var startDate = baseParams.Params.GetAs<DateTime>("startDate");

            var period = new ContractPeriod {StartDate = new DateTime(startDate.Year, startDate.Month, 1)};

            if((period.StartDate.Month >= DateTime.Today.Month && period.StartDate.Year >= DateTime.Today.Year))
            {
                return this.JsFailure("Формирование списка запрещено, список расчетов формируется только за закрытые периоды");
            }

            if (this.DomainService.GetAll().Any(x => x.StartDate == period.StartDate))
            {
                return this.JsFailure("Формирование списка запрещено, список расчетов за данный отчетный период уже сформирован");
            }

            var cultureInfo = CultureInfo.GetCultureInfo("ru-RU");
            period.Name = period.StartDate.ToString("MMMM yyyy", cultureInfo);
            period.EndDate = period.StartDate.AddMonths(1).AddDays(-1);

            IDataResult result = BaseDataResult.Error("Ошибка при сохранении");

            this.Container.UsingForResolved<IDataTransaction>(
                (container, transaction) =>
                {
                    try
                    {
                        this.DomainService.Save(period);

                        result = this.ChargeSplittingService.CreateSummaries(period);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();

                        throw;
                    }
                });

            return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Актуализировать сведения за период
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат</returns>
        public ActionResult Actualize(BaseParams baseParams)
        {
            return this.ChargeSplittingService.ActualizeSummaries(baseParams).ToJsonResult();
        }
    }
}