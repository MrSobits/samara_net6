namespace Bars.GkhGji.Regions.Khakasia.Controllers
{
    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Khakasia.Entities;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
	/// Контроллер для Жилой дом акта проверки
	/// </summary>
    public class ActCheckRealityObjectController : Bars.GkhGji.Controllers.ActCheckRealityObjectController
    {
		/// <summary>
		/// Сервис для больших текстов жилого дома акта проверки
		/// </summary>
        public IBlobPropertyService<ActCheckRealityObject, ActCheckRoLongDescription> LongTextService { get; set; }

		/// <summary>
		/// Сохранить описание
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

		/// <summary>
		/// Получить описание
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}