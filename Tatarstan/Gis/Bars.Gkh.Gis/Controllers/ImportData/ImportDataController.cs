namespace Bars.Gkh.Gis.Controllers.ImportData
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.IoC;
    using B4.Utils.Web.DynamicRequest;
    using DomainService.ImportData;

    using Microsoft.AspNetCore.Http;

    public class ImportDataController : BaseController
    {
        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <returns></returns>
     
        public ActionResult Import()
        {
            //т.к. BaseParams пока не потдерживает множественную загрузку файлов
            //собираем вручную
            var files = Request.Form.Files;
            var baseParams = new BaseParams
            {
                Params = DynamicRequestHelper.RequestAsDynamic(ReadRequest.All, Request),
                Files = files
                    .Select(
                        (x, index) => new { Name = string.Format("{0}_{1}", x.Name, index), File = x })
                    .Where(x => x.File != null && !string.IsNullOrEmpty(x.File.FileName))
                    .ToDictionary(
                        x => x.Name,
                        x =>
                        {
                            var file = x.File;
                            var data = ConvertToBytes(file);

                            var extension = Path.GetExtension(file.FileName);
                            if (string.IsNullOrEmpty(extension))
                            {
                                throw new BaseException(string.Format("Не удалось получить расширение файла \"{0}\"",
                                    x.File.FileName));
                            }

                            return new FileData(Path.GetFileNameWithoutExtension(x.File.FileName),
                                extension.Substring(1),
                                data);
                        })
            };

            var result = JsonNetResult.Success;

            Container.UsingForResolved<IImportDataService>((container, service) =>
            {
                var importResult = service.AddTask(baseParams);

                result = new JsonNetResult(new
                {
                    success = importResult.Success,
                    message = importResult.Message,
                    errors = importResult.Errors
                }) { ContentType = "text/html; charset=utf-8" };
            });

            return result;
        }
        
        private byte[] ConvertToBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.OpenReadStream().CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Получение списка всех загруженных файлов
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetAllLoadedFileList(BaseParams baseParams)
        {
            var result =JsonNetResult.Success;

            Container.UsingForResolved<IImportDataService>((container, service) =>
            {
                try
                {
                    var fileList = service.GetAllLoadedFiles(baseParams);

                    result = new JsonNetResult(new
                    {
                        success = true,
                        data = fileList.Data,
                        totalCount = fileList.TotalCount
                    }) { ContentType = "text/html; charset=utf-8" };
                }
                catch (Exception exc)
                {
                    result = JsonNetResult.Failure(exc.Message);
                    result.ContentType = "text/html; charset=utf-8";
                }
            });

            return result;
        }

        /// <summary>
        /// Получение списка загруженных файлов
        /// </summary>        
        public ActionResult LoadedFileList(BaseParams baseParams)
        {
            var service = Container.Resolve<IImportDataService>();

            try
            {
                var result = service.GetLoadedFilesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}
