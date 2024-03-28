namespace Bars.GkhCalendar.Controllers
{
	using System;
    using Bars.B4;
    using Bars.GkhCalendar.DomainService;
    using Bars.GkhCalendar.Entities;

	using Microsoft.AspNetCore.Mvc;

	/// <summary>
	/// Контроллер для День производственного календаря
	/// </summary>
    public class DayController : B4.Alt.DataController<Day>
    {
		/// <summary>
		/// Получить список дней
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Список дней</returns>
        public ActionResult GetDaysList(BaseParams baseParams)
        {
            var result = this.Resolve<IDayService>().GetDays(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

		/// <summary>
		/// Получить дату после заданного количества рабочих дней
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Дата</returns>
		public ActionResult GetDateAfterWorkDays(BaseParams baseParams)
		{
			var date = baseParams.Params.GetAs<DateTime>("date");
			var workDaysCount = baseParams.Params.GetAs<uint>("workDaysCount");

			var result = this.Resolve<IIndustrialCalendarService>().GetDateAfterWorkDays(date, workDaysCount);
			return new JsonNetResult(result);
		}
	}
}