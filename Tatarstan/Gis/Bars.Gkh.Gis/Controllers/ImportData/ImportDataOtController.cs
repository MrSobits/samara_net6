namespace Bars.Gkh.Gis.Controllers.ImportData
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.IoC;
    using DomainService.ImportData;

    public class ImportDataOtController : BaseController
    {
        public ActionResult ImportIndicators(BaseParams baseParams)
        {
            var result = JsonNetResult.Success;

            Container.UsingForResolved<IImportDataOtService>((container, service) =>
            {
                var importResult = service.ImportIndicator(baseParams);

                result = new JsonNetResult(new
                {
                    success = true,
                    message = ""
                }) { ContentType = "text/html; charset=utf-8" };
            });

            return result;
        }

        //Список выгруженных файлов
        public ActionResult LoadedFileList(BaseParams baseParams)
        {
            var result = JsonNetResult.Success;

            Container.UsingForResolved<IImportDataService>((container, service) =>
            {
                try
                {
                    var fileList = service.GetLoadedFilesList(baseParams);

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

        public ActionResult OpenTatastanImportData(BaseParams baseParams)
        {
            var result = JsonNetResult.Success;

            Container.UsingForResolved<IImportDataService>((container, service) =>
            {
                try
                {
                    var fileList = service.GetOpenTatarstanData(baseParams);

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

        public ActionResult DeleteOtData(BaseParams baseParams)
        {
            var result = JsonNetResult.Success;

            Container.UsingForResolved<IImportDataService>((container, service) =>
            {
                try
                {
                    var response = (BaseDataResult)service.DeleteOtData(baseParams);
                    result = new JsonNetResult(new
                    {
                        success = response.Success,
                        data = response.Data,
                        message = response.Message
                    });
                }
                catch (Exception ex)
                {
                    result = JsonNetResult.Failure(ex.Message);
                }
            });

            return result;
        }

        public ActionResult DeleteLoadedData(BaseParams baseParams)
        {
            var result = JsonNetResult.Success;

            Container.UsingForResolved<IImportDataService>((container, service) =>
            {
                try
                {
                    var response = (BaseDataResult)service.DeleteLoadedData(baseParams);
                    result = new JsonNetResult(new
                    {
                        success = response.Success,
                        data = response.Data,
                        message = response.Message
                    });
                }
                catch (Exception ex)
                {
                    result = JsonNetResult.Failure(ex.Message);
                }
            });

            return result;
        }
    }
}
