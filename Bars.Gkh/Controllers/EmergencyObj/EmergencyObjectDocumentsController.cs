namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using DomainService;

	/// <summary>
	/// Контроллер для Документы аварийного дома
	/// </summary>
	public class EmergencyObjectDocumentsController : FileStorageDataController<EmergencyObjectDocuments>
    {
		/// <summary>
		/// Получить идентификатор документов
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public ActionResult GetDocumentsIdByEmergencyObject(BaseParams baseParams)
        {
			var result = (BaseDataResult)this.Resolve<IEmergencyObjectDocumentsService>().GetDocumentsIdByEmergencyObject(baseParams);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}