namespace Bars.GkhCr.Controllers
{
	using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

	/// <summary>
	/// Контроллер для Акт выполненных работ
	/// </summary>
	public class PerformedWorkActController : FileStorageDataController<PerformedWorkAct>
    {
		/// <summary>
		/// Сервис для Акт выполненных работ
		/// </summary>
		public IPerformedWorkActService PerformedWorkActService { get; set; }

		/// <summary>
		/// Получить список актов
		/// </summary>
		public ActionResult ListAct(BaseParams baseParams)
        {
            return new JsonNetResult(this.PerformedWorkActService.ListAct(baseParams));
        }

		/// <summary>
		/// Экспортировать акты
		/// </summary>
		public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("PerformedWorkActDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

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

		/// <summary>
		/// Проверить допустимые акты для сводной информации
		/// </summary>
		public ActionResult CheckActsForDetails(BaseParams baseParams)
		{
			return new JsonNetResult(this.PerformedWorkActService.CheckActsForDetails(baseParams));
		}

		/// <summary>
		/// Получить сводную информацию по актам
		/// </summary>
		public ActionResult ListDetails(BaseParams baseParams)
	    {
			return new JsonNetResult(this.PerformedWorkActService.ListDetails(baseParams));
		}
    }
}
