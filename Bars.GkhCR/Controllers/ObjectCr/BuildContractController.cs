namespace Bars.GkhCr.Controllers
{
    using System.Collections;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class BuildContractController : FileStorageDataController<BuildContract>
    {
        public ActionResult AddTypeWorks(BaseParams baseParams)
        {
            var service = Container.Resolve<IBuildContractService>();
            try
            {
                var result = (BaseDataResult)service.AddTypeWorks(baseParams);
                return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult AddTermination(BaseParams baseParams)
        {
            var service = Container.Resolve<IBuildContractService>();
            try
            {
                var result = (BaseDataResult)service.AddTermination(baseParams);
                return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public override ActionResult Delete(BaseParams baseParams)
        {
            var domain = Container.Resolve<IDomainService<BuildContract>>();
            try
            {
                var result = domain.Delete(baseParams);
                return new JsonNetResult(new { success = result.Success, message = result.Message, data = result.Data }) { ContentType = "text/html; charset=utf-8" };
            }
            catch (ValidationException exc)
            {
                var result = JsonNetResult.Failure(exc.Message);
                result.ContentType = "text/html; charset=utf-8";
                return result;
            }
            finally
            {
                Container.Release(domain);
            }
        }

        /// <summary>
        /// Вернуть статусы, связанные с договорами
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListAvailableStates(BaseParams baseParams)
        {
            var service = Container.Resolve<IBuildContractService>();
            try
            {
                var result = (ListDataResult)service.ListAvailableStates(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ListAvailableBuilders(BaseParams baseParams)
        {
            var service = Container.Resolve<IBuildContractService>();
            try
            {
                var result = service.ListAvailableBuilders(baseParams);
                return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult GetForMap(BaseParams baseParams)
        {
            var ros = Container.Resolve<IBuildContractService>();
            using (Container.Using(ros))
            {
                var res = ros.GetForMap(baseParams);
                return res.Success ? new JsonGetResult(res.Data) : null;
            }
        }
    }
}