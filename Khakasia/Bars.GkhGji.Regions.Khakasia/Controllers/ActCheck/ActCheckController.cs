namespace Bars.GkhGji.Regions.Khakasia.Controllers.ActCheck
{
	using System.Collections;
	using Microsoft.AspNetCore.Mvc;
	using Bars.B4;
	using Bars.GkhGji.Regions.Khakasia.DomainService;

	/// <summary>
	/// Контроллер для Акт проверки
	/// </summary>
	public class ActCheckController : GkhGji.Controllers.ActCheckController
    {
		/// <summary>
		/// Получить список жилых домов для акта проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public ActionResult ListRealObjForActCheck(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IKhakasiaActCheckService>();

            try
            {
                var result = (ListDataResult)service.ListRealObjForActCheck(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}