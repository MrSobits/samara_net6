namespace Bars.GkhGji.Regions.Khakasia.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Khakasia.DomainService;
    using Bars.GkhGji.Regions.Khakasia.Entities;

	/// <summary>
	/// Контроллер для Мероприятия по контролю распоряжения ГЖИ
	/// </summary>
	public class DisposalControlMeasuresController : B4.Alt.DataController<DisposalControlMeasures>
    {
		/// <summary>
		/// Добавить мероприятия по контролю распоряжения ГЖИ
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult AddDisposalControlMeasures(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IDisposalControlMeasuresService>().AddDisposalControlMeasures(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}