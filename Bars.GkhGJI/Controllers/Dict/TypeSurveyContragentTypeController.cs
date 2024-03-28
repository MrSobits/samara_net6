namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService.Dict;
    using Bars.GkhGji.Entities.Dict;

	/// <summary>
	/// Контроллер для Типы контрагента
	/// </summary>
	public class TypeSurveyContragentTypeController : B4.Alt.DataController<TypeSurveyContragentType>
    {
		/// <summary>
		/// Добавить типы контрагента
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public ActionResult AddContragentTypes(BaseParams baseParams)
        {
            var result = this.Resolve<ITypeSurveyContragentTypeService>().AddContragentTypes(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}