using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.IoC;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Utils;
using Bars.B4.Utils.Web;
using Bars.Gkh.Utils;

namespace Bars.Gkh.Controllers
{
    /// <summary>
    /// Контроллер для работы с файлами
    /// </summary>
    public class FilePreviewController : BaseController
    {
        private readonly long bigSize = 1024 * 1024 * 1024; 


        /// <summary>
        /// Скачать файл
        /// </summary>
        /// <param name="id">идентификатор FileInfo</param>
        public ActionResult Download(long id)
        {
            var fileManager = Container.Resolve<IFileManager>();
            var fileDomain = Container.Resolve<IDomainService<FileInfo>>();
            var fileInfo = fileDomain.Get(id);
            using (Container.Using(fileManager))
            {
                var file = fileManager.GetFile(fileInfo);
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileInfo.FullName}\"");
                return new FileStreamResult(file, "attachment");
            }
        }

        /// <summary>
        /// Данный метод проверяет как наличие записи в таблице файлов, так и сам файл на диске
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>
        ///     { success: true } - если файл существует и доступен
        ///     { success: false } - если файл отсутствует на диске или как запись в таблице
        /// </returns>
        //public ActionResult CheckFile(BaseParams baseParams)
        //{
        //    var id = baseParams.Params.GetAs<long>("id");
        //    var fileManager = Container.Resolve<IRisFileManager>();

        //    using (Container.Using(fileManager))
        //    {
        //        var checkFileResult = fileManager.CheckFile(id);
        //        return new JsonNetResult(new { success = checkFileResult.Success });
        //    }
        //}

        /// <summary>
        /// Предпросмотр файла
        /// </summary>
        public ActionResult PreviewFile(long id)
        {
            var fileManager = Container.Resolve<IFileManager>();
            var fileDomain = Container.Resolve<IDomainService<FileInfo>>();

            using (Container.Using(fileDomain, fileManager))
            {
                var fileInfo = fileDomain.Get(id);
                byte[] data = null;
                var downloadedName = fileInfo.FullName;
                var extension = downloadedName.Split('.').Last();
                var file = fileManager.GetFile(fileInfo);
                data = file.ReadAllBytes();
                //Если выполняется предпросмотр excel файла, то конвертируем его в изображение
                if (extension == "xls" || extension == "xlsx")
                {
                    //using (var fileMetadata = fileManager.Export(id))
                    //{
                    //    var excelProvider = Container.Resolve<IExcelProvider>();

                    //    using (Container.Using(excelProvider))
                    //    {
                    //        if (extension == "xlsx")
                    //        {
                    //            excelProvider.UseVersionXlsx();
                    //        }
                    //        using (var imageStream = excelProvider.ConvertToImage(fileMetadata.Content))
                    //        {
                    //            if (imageStream != null)
                    //            {
                    //                data = imageStream.ReadAllBytes();
                    //            }
                    //        }
                    //    }
                    //}
                }
                else
                {
                    

                }

                return new DownloadPreviewResult
                {
                    Data = data,
                    FileDownloadName = fileInfo.FullName,
                    ResultCode = ResultCode.Success
                };
            }
        }
    }
}