namespace Bars.Gkh.RegOperator.Controllers.Period
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.RegOperator.DomainService.Period;
    using Bars.Gkh.RegOperator.Entities.Period;

    public class PeriodCloseCheckResultController : B4.Alt.DataController<PeriodCloseCheckResult>
    {
        /// <summary>
        /// Запускает проверку
        /// </summary>
        /// <returns></returns>
        public ActionResult RunCheck(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IPeriodCloseCheckService>();
            try
            {
                return new JsonNetResult(service.RunCheck());
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Запускает проверку и закрытие периода в случае успеха
        /// </summary>
        /// <returns></returns>
        public ActionResult RunCheckAndClosePeriod(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IPeriodCloseCheckService>();
            try
            {
                return new JsonNetResult(service.RunCheckAndClosePeriod(baseParams));
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}