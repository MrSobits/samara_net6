namespace Bars.GkhGji.Regions.Khakasia.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Khakasia.DomainService;
    using Bars.GkhGji.Regions.Khakasia.Entities;

	/// <summary>
	/// Контроллер для Исполнитель обращения
	/// </summary>
	public class AppealCitsExecutantController : B4.Alt.DataController<AppealCitsExecutant>
    {
		/// <summary>
		/// Добавить исполнителей
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public ActionResult AddExecutants(BaseParams baseParams)
        {
			var service = this.Container.Resolve<IAppealCitsExecutantService>();
            ActionResult actionResult;
            using (this.Container.Using(service))
            {
                var result = service.AddExecutants(baseParams);
                actionResult = result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }

            return actionResult;
        }

		/// <summary>
		/// Перенаправить исполнителя
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public ActionResult RedirectExecutant(BaseParams baseParams)
        {
			var service = this.Container.Resolve<IAppealCitsExecutantService>();
            ActionResult actionResult;
            using (this.Container.Using(service))
            {
                var result = this.Container.Resolve<IAppealCitsExecutantService>().RedirectExecutant(baseParams);
                actionResult = result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }

            return actionResult;
        }
    }
}