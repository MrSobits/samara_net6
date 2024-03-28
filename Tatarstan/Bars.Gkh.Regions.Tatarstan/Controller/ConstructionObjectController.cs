namespace Bars.Gkh.Regions.Tatarstan.Controller
{
	using Microsoft.AspNetCore.Mvc;

	using Bars.B4;
	using Bars.Gkh.Regions.Tatarstan.DomainService;
	using Bars.Gkh.Regions.Tatarstan.Entities;

	/// <summary>
	/// Контроллер для <see cref="ConstructionObject"/>
	/// </summary>
    public class ConstructionObjectController : B4.Alt.DataController<ConstructionObject>
    {
		/// <summary>
		/// Массово сменить статусы объектов строительства
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult MassChangeState(BaseParams baseParams)
		{
			var service = this.Container.Resolve<IConstructionObjectService>();

			try
			{
				var result = service.MassChangeState(baseParams);
				return result.Success ? new JsonNetResult(new { success = true, message = result.Message }) : JsonNetResult.Failure(result.Message);
			}
			finally
			{
				this.Container.Release(service);
			}

		}
	}
}