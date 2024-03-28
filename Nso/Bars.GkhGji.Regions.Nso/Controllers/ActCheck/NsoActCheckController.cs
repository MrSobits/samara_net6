namespace Bars.GkhGji.Regions.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Gkh.Domain;
    using GkhGji.Controllers;
    using Entities;

	/// <summary>
	/// На основе существующего контроллера делаем свой для того чтобы все запросы шли на новый Url
	/// </summary>
	public class NsoActCheckController : ActCheckController<NsoActCheck>
    {
		/// <summary>
		/// Проверить, есть ли нарушения
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public ActionResult IsAnyHasViolation(BaseParams baseParams)
        {
			var service = Resolve<INsoActCheckService>();
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
                Container.Release(service);
            }
        }

		/// <summary>
		/// Объединить акты проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult MergeActs(BaseParams baseParams)
		{
			var service = Resolve<INsoActCheckService>();

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