namespace Bars.GkhGji.Regions.Khakasia.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Regions.Khakasia.DomainService;
    using Bars.GkhGji.Regions.Khakasia.Entities;

	/// <summary>
	/// Контроллер для Группа нарушений для Документа ГЖИ
	/// </summary>
	public class DocumentViolGroupController : B4.Alt.DataController<DocumentViolGroup>
    {
		/// <summary>
		/// Сервис для больших текстов группы нарушений для Документа ГЖИ
		/// </summary>
		public IBlobPropertyService<DocumentViolGroup, DocumentViolGroupLongText> LongTextService { get; set; }

		/// <summary>
		/// Сервис для Группа нарушений для Документа ГЖИ
		/// </summary>
		public IViolationGroupService ViolationGroupoService { get; set; }

		/// <summary>
		/// Метод сохранения Пунктов нарушений для Описания
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual ActionResult SavePoints(BaseParams baseParams)
        {
            var result = this.ViolationGroupoService.SavePoints(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

		/// <summary>
		/// Метод сохранения Blob поля Описание
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

		/// <summary>
		/// Метод получения Blob поля Описание
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

		/// <summary>
		/// Метод сохранения Blob поля Мероприятие
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual ActionResult SaveAction(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

		/// <summary>
		/// Метод получения Blob поля Мероприятие
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual ActionResult GetAction(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}