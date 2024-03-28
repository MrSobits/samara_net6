namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers
{
	using System;
	using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhCalendar.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Entities;

	/// <summary>
	/// Контроллер для День производственного календаря
	/// </summary>
    public class TaskCalendarController : B4.Alt.DataController<TaskCalendar>
    {
		/// <summary>
		/// Получить список дней
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Список дней</returns>
        public ActionResult GetDaysList(BaseParams baseParams)
        {
            var result = this.Resolve<ITaskCalendarService>().GetDays(baseParams);
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

        public ActionResult GetListLicRequest(BaseParams baseParams)
        {
            var resolutionService = Container.Resolve<ITaskCalendarService>();
            try
            {
                return resolutionService.GetListDisposal(baseParams).ToJsonResult();
            }
            catch
            {
                return null;
            }
            finally
            {

            }
        }
		public ActionResult GetListResolPros(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListResolPros(baseParams).ToJsonResult();
			}
			catch
			{
				return null;
			}
			finally
			{

			}
		}
		public ActionResult GetListProtocolsGji(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListProtocolsGji(baseParams).ToJsonResult();
			}
			catch
			{
				return null;
			}
			finally
			{
			}
		}

		public ActionResult GetListAppeals(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListAppeals(baseParams).ToJsonResult();
			}
			catch
			{
				return null;
			}
			finally
			{
			}
		}

		public ActionResult GetListAdmonitions(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListAdmonitions(baseParams).ToJsonResult();
			}
			catch
			{
				return null;
			}
			finally
			{
			}
		}

		public ActionResult GetListPrescriptionsGji(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListPrescriptionsGji(baseParams).ToJsonResult();
			}
			catch
			{
				return null;
			}
			finally
			{
			}
		}

		public ActionResult GetListSOPR(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListSOPR(baseParams).ToJsonResult();
			}
			catch
			{
				return null;
			}
			finally
			{
			}
		}
		public ActionResult GetListCourtPractice(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListCourtPractice(baseParams).ToJsonResult();
			}
			finally
			{

			}
		}
	}
}