namespace Bars.GkhCr.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
	/// Контроллер для Акт выполненных работ
	/// </summary>
	public class SpecialPerformedWorkActController : FileStorageDataController<SpecialPerformedWorkAct>
    {
		/// <summary>
		/// Сервис для Акт выполненных работ
		/// </summary>
		public ISpecialPerformedWorkActService PerformedWorkActService { get; set; }

		/// <summary>
		/// Получить информацию по акту
		/// </summary>
		public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = this.PerformedWorkActService.GetInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

		/// <summary>
		/// Получить список актов по новым активным программам
		/// </summary>
		public ActionResult ListByActiveNewOpenPrograms(BaseParams baseParams)
        {
            var result = this.PerformedWorkActService.ListByActiveNewOpenPrograms(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
