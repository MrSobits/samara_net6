namespace Bars.GkhGji.Regions.Nso.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using B4;
	using DomainService;
	using GkhGji.Controllers;
    using Entities;
	using Gkh.Domain;

	/// <summary>
	/// Контроллер для Акт проверки предписания
	/// </summary>
	public class NsoActRemovalController : ActRemovalController<NsoActRemoval>
    {
		/// <summary>
		/// Объединить акты проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult MergeActs(BaseParams baseParams)
		{
			var service = Resolve<INsoActRemovalService>();

			try
			{
				return service.MergeActs(baseParams).ToJsonResult();
			}
			finally
			{
				Container.Release(service);
			}
		}
	}
}