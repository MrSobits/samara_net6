namespace Bars.Gkh.Regions.Chelyabinsk.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Import;

    public class RosRegExtractImportController : BaseController
    {
        public ActionResult GetImportList(BaseParams baseParams)
        {
            try
            {
                var importProvider = Container.Resolve<IGkhImportService>();
                var authorizationServices = Container.ResolveAll<IAuthorizationService>();
                var userIdentity = Container.Resolve<IUserIdentity>();

                var service = authorizationServices.FirstOrDefault();

                if (service == null)
                {
                    throw new ArgumentNullException();
                }

                var importList = importProvider.GetImportInfoList(baseParams);

                using (Container.Using(importList, importProvider, authorizationServices, userIdentity))
                {
                    var items = importList
                        .Where(x => service.Grant(userIdentity, x.PermissionName))
                        .Select(x => new {x.Key, x.Name, x.PossibleFileExtensions})
                        .ToList();

                    return new JsonListResult(items);
                }
            }
            catch (ValidationException exc)
            {
                return JsonNetResult.Failure(exc.Message);
            }
        }

        public ActionResult Import(BaseParams baseParams)
        {
            var importProvider = Container.Resolve<IGkhImportService>();

            using (Container.Using(importProvider))
            {
                ActionResult actionResult;

                try
                {
                    var result = importProvider.Import(baseParams);

                    if (result.Success)
                    {
                        actionResult = new JsonNetResult(new
                        {
                            success = result.Success,
                            title = string.Empty,
                            message = "Задачи успешно поставлены в очередь на выполнение"
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
