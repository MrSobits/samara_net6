namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.ActRemoval
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    /// <summary>
	/// Контроллер для Акт проверки предписания
	/// </summary>
	public class ChelyabinskActRemovalController : ActRemovalController<ChelyabinskActRemoval>
    {
		/// <summary>
		/// Объединить акты проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult MergeActs(BaseParams baseParams)
		{
			var service = this.Resolve<IChelyabinskActRemovalService>();

			try
			{
				return service.MergeActs(baseParams).ToJsonResult();
			}
			finally
			{
				this.Container.Release(service);
			}
		}
	}
}