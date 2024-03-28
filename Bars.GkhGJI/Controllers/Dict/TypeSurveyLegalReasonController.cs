namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService.Dict;
    using Bars.GkhGji.Entities.Dict;

	/// <summary>
	/// Контроллер для Правовые основания
	/// </summary>
	public class TypeSurveyLegalReasonController : B4.Alt.DataController<TypeSurveyLegalReason>
    {
		/// <summary>
		/// Добавить правовые основания
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public ActionResult AddLegalReasons(BaseParams baseParams)
        {
            var result = this.Resolve<ITypeSurveyLegalReasonService>().AddLegalReasons(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}