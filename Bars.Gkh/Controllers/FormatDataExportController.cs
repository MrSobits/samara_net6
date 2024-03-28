namespace Bars.Gkh.Controllers
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Utils.Web;
    using Bars.Gkh.Domain;
    using Bars.Gkh.FormatDataExport.Domain;

    /// <summary>
    /// Контроллер экспорта в формате 4.0
    /// </summary>
    public class FormatDataExportController : BaseController
    {
        /// <summary>
        /// Интерфейс сервиса экспорта в формате 4.0
        /// </summary>
        public IFormatDataExportService FormatDataExportService { get; set; }

        /// <summary>
        /// Метод возвращает список доступных выгружаемых секций
        /// </summary>
        public ActionResult ListAvailableSection(BaseParams baseParams)
        {
            return this.FormatDataExportService.ListAvailableSection(baseParams).ToJsonResult();
        }


        /// <summary>
        /// Получить результат загрузки
        /// </summary>
        public ActionResult GetRemoteStatus(BaseParams baseParams)
        {
            return this.FormatDataExportService.GetRemoteStatus(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Запустить удаленный импорт
        /// </summary>
        public ActionResult StartRemoteImport(BaseParams baseParams)
        {
            return this.FormatDataExportService.StartRemoteImport(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Получить удаленный файл
        /// </summary>
        public ActionResult GetRemoteFile(BaseParams baseParams)
        {
            try
            {
                var remoteFile = this.FormatDataExportService.GetRemoteFile(baseParams);

                if (remoteFile.Success)
                {
                    var filePath = remoteFile.Data as string;
                    return new DownloadResult
                    {
                        ResultCode = ResultCode.Success,
                        FileDownloadName = Path.GetFileName(filePath),
                        Path = filePath
                    };
                }

                return remoteFile.ToJsonResult();
            }
            catch (Exception e)
            {
                return this.NotFound(e.Message);
            }
        }

        /// <summary>
        /// Обновить статус удаленной задачи
        /// </summary>
        public ActionResult UpdateRemoteStatus(BaseParams baseParams)
        {
            return this.FormatDataExportService.UpdateRemoteStatus(baseParams).ToJsonResult();
        }
    }
}