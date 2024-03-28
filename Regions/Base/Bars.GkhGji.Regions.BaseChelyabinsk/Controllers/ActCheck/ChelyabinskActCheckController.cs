namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.ActCheck
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;

    /// <summary>
	/// На основе существующего контроллера делаем свой для того чтобы все запросы шли на новый Url
	/// </summary>
	public class ChelyabinskActCheckController : ActCheckController<ChelyabinskActCheck>
    {
		/// <summary>
		/// Проверить, есть ли нарушения
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public ActionResult IsAnyHasViolation(BaseParams baseParams)
        {
			var service = this.Resolve<IChelyabinskActCheckService>();
			try
			{
				var dataResult = service.IsAnyHasViolation(baseParams);
				return new JsonNetResult(new
				{
					success = dataResult.Success,
					message = dataResult.Message,
					data = dataResult.Data
				});
			}
			catch (ValidationException ex)
			{
				return JsonNetResult.Failure(ex.Message);
			}
            finally
            {
                this.Container.Release(service);
            }
        }

		/// <summary>
		/// Объединить акты проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult MergeActs(BaseParams baseParams)
		{
			var service = this.Resolve<IChelyabinskActCheckService>();

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