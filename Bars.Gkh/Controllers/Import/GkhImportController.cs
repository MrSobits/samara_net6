namespace Bars.Gkh.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Import;

    /// <summary>
    /// Контроллер для работы с импортами
    /// </summary>
    public class GkhImportController : BaseController
    {
        public IGkhImportService ImportService { get; set; }

        /// <summary>
        /// Получить список импортов
        /// </summary>
        public ActionResult GetImportList(BaseParams baseParams)
        {
            try
            {
                var authorizationServices = this.Container.ResolveAll<IAuthorizationService>();
                var userIdentity = this.Container.Resolve<IUserIdentity>();

                var service = authorizationServices.FirstOrDefault();

                if (service == null)
                {
                    throw new ArgumentNullException();
                }

                var importList = this.ImportService.GetImportInfoList(baseParams);

                using (this.Container.Using(importList, authorizationServices, userIdentity))
                {
                    var items = importList
                        .Where(x => service.Grant(userIdentity, x.PermissionName))
                        .Select(x => new {x.Key, x.Name, x.PossibleFileExtensions})
                        .ToList();
                    items.Add(new { Key = "AmirsImport", Name = "Импорт постановлений АМИРС", PossibleFileExtensions = "xlsx" });

                    return new JsonListResult(items);
                }
            }
            catch (ValidationException exc)
            {
                return JsonNetResult.Failure(exc.Message);
            }
        }

        /// <summary>
        /// Запуск задачи импорта
        /// </summary>
        public ActionResult Import(BaseParams baseParams)
        {
            try
            {
                var result = this.ImportService.Import(baseParams);
                if (result.Success)
                {
                    return new JsonNetResult(new
                    {
                        success = result.Success,
                        title = string.Empty,
                        message = "Задачи успешно поставлены в очередь на выполнение"
                    });
                }

                return JsonNetResult.Failure(result.Message);
            }
            catch (ValidationException e)
            {
                return JsonNetResult.Failure(e.Message);
            }
            catch (NotImplementedException e)
            {
                return JsonNetResult.Failure(e.Message);
            }
            catch (Exception)
            {
                return JsonNetResult.Failure("Произошла ошибка при выполнении импорта");
            }
        }
        
        /// <summary>
        /// Запуск множественного импорта
        /// </summary>
        public ActionResult MultiImport(BaseParams baseParams)
        {
            try
            {
                var result = this.ImportService.MultiImport(baseParams);
                if (result.Success)
                {
                    return new JsonNetResult(new
                    {
                        success = result.Success,
                        title = string.Empty,
                        message = "Задачи успешно поставлены в очередь на выполнение"
                    });
                }

                return JsonNetResult.Failure(result.Message);
            }
            catch (ValidationException e)
            {
                return JsonNetResult.Failure(e.Message);
            }
            catch (NotImplementedException e)
            {
                return JsonNetResult.Failure(e.Message);
            }
            catch (Exception)
            {
                return JsonNetResult.Failure("Произошла ошибка при выполнении импорта");
            }
        }
        
        /// <summary>
        /// Метод для дебага импортов, просто в js правим урл импорта
        /// </summary>
        public ActionResult ImportNow(BaseParams baseParams)
        {
            var importId = baseParams.Params.GetAs("importId", "");
            var importer = this.Container.Resolve<IGkhImport>(importId);

            using (this.Container.Using(importer))
            {
                ActionResult actionResult;

                try
                {
                    var result = importer.Import(baseParams);

                    if (result.Success)
                    {
                        actionResult = new JsonNetResult(new
                        {
                            success = result.Success,
                            title = string.Empty,
                            message = string.Format("Импорт завершился {0}", result.StatusImport)
                        });
                    }
                    else
                    {
                        actionResult = JsonNetResult.Failure(result.Message);
                    }
                }
                catch (NotImplementedException e)
                {
                    actionResult = JsonNetResult.Failure(e.Message);
                }

                return actionResult;
            }
        }
    }
}
