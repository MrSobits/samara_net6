namespace Bars.Gkh.RegOperator.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Modules.FileStorage;

    using DomainService;

    using Entities;

    /// <summary>
    /// Контроллер для Заявка на перечисление средств подрядчикам
    /// </summary>
    public class TransferCtrController : FileStorageDataController<TransferCtr>
    {
        public ITransferCtrService TransferCtrService { get; set; }

        /// <summary>
        /// Экспорт
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("TransferCtrDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

        /// <summary>
        /// Экспортировать в текстовый формат
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult ExportToTxt(BaseParams baseParams)
        {
            try
            {
                return new JsonNetResult(this.TransferCtrService.ExportToTxt(baseParams).Data);
            }
            catch (Exception e)
            {
                return this.JsFailure(e.Message);
            }
        }

        /// <summary>
        /// Сохранить с детальной информацией
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult SaveWithDetails(BaseParams baseParams)
        {
            var result = this.TransferCtrService.SaveWithDetails(baseParams);
            return result.Success
                ? new JsonNetResult(new { success = true, message = result.Message, data = result.Data })
                : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Скопировать заявку
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult Copy(BaseParams baseParams)
        {
            return new JsonNetResult(this.TransferCtrService.Copy(baseParams));
        }
    }
}