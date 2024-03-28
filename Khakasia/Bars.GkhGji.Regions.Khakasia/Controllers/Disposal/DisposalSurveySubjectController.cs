namespace Bars.GkhGji.Regions.Khakasia.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Khakasia.DomainService;
    using Bars.GkhGji.Regions.Khakasia.Entities;

	/// <summary>
	/// Контроллер для Предмет проверки для приказа 
	/// </summary>
	public class DisposalSurveySubjectController : B4.Alt.DataController<DisposalSurveySubject>
    {
		/// <summary>
		/// Домен сервис для Предмет проверки для приказа 
		/// </summary>
		public IDisposalSurveySubjectService Service { get; set; }

		/// <summary>
		/// Добавить Предмет проверки для приказа
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult AddDisposalSurveySubject(BaseParams baseParams)
        {
            var result = this.Service.AddDisposalSurveySubject(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}