namespace Bars.GkhGji.Regions.Khakasia.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Khakasia.Entities;

	/// <summary>
	/// Контроллер для Акт обследования (Хакасия)
	/// </summary>
	public class KhakasiaActSurveyController : ActSurveyController<KhakasiaActSurvey>
    {
		/// <summary>
		/// Сервис для больших текстов жилого дома акта обследования
		/// </summary>
		public IBlobPropertyService<ActSurvey, ActSurveyLongDescription> DescriptionService { get; set; }

		/// <summary>
		/// Сохранить описание
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.DescriptionService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

		/// <summary>
		/// Получить описание
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.DescriptionService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}