namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.DomainService;
    using Bars.GkhGji.Regions.Tomsk.Entities;

	/// <summary>
	/// Контроллер для Предмет проверки для приказа лицензирование
	/// </summary>
	public class DisposalVerificationSubjectLicensingController : B4.Alt.DataController<DisposalVerificationSubjectLicensing>
    {
		/// <summary>
		/// Сервис для Предмет проверки для приказа лицензирование
		/// </summary>
		public IDisposalVerificationSubjectLicensingService Service { get; set; }

		/// <summary>
		/// Добавить предмет проверки для приказа лицензирование
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult AddDisposalVerificationSubjectLicensing(BaseParams baseParams)
        {
            var result = this.Service.AddDisposalVerificationSubjectLicensing(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
