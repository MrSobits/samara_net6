namespace Bars.GkhGji.Regions.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Entities;
    using Gkh.DomainService;
    using GkhGji.Entities;

	/// <summary>
	/// Контроллер для Документ ГЖИ
	/// </summary>
    public class NsoDocumentGjiController : BaseController
    {
		/// <summary>
		/// Сервис для работы с текстом
		/// </summary>
        public IBlobPropertyService<DocumentGji, NsoDocumentLongText> LongTextService { get; set; }

		/// <summary>
		/// Получить информацию о лицах, допустивших нарушение
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public ActionResult GetPersonViolationInfo(BaseParams baseParams)
        {
            return GetBlob(baseParams);
        }

		/// <summary>
		/// Сохранить информацию о лицах, допустивших нарушение
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual ActionResult SavePersonViolationInfo(BaseParams baseParams)
        {
            return SaveBlob(baseParams);
        }

		/// <summary>
		/// Получить Сведения о том, что нарушения были допущены в результате
		/// виновных действий (бездействия) должностных лиц и/или
		/// работников проверяемого лица
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult GetPersonViolationActionInfo(BaseParams baseParams)
        {
            return GetBlob(baseParams);
        }

		/// <summary>
		/// Сохранить Сведения о том, что нарушения были допущены в результате
		/// виновных действий (бездействия) должностных лиц и/или
		/// работников проверяемого лица
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual ActionResult SavePersonViolationActionInfo(BaseParams baseParams)
        {
            return SaveBlob(baseParams);
        }

		/// <summary>
		/// Получить Описание нарушения
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult GetViolationDescription(BaseParams baseParams)
		{
			return GetBlob(baseParams);
		}

		/// <summary>
		/// Сохранить Описание нарушения
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual ActionResult SaveViolationDescription(BaseParams baseParams)
		{
			return SaveBlob(baseParams);
		}

		private ActionResult SaveBlob(BaseParams baseParams)
        {
            var result = LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        private ActionResult GetBlob(BaseParams baseParams)
        {
            var result = LongTextService.Get(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}