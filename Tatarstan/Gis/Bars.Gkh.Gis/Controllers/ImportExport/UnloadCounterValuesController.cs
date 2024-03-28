namespace Bars.Gkh.Gis.Controllers.ImportExport
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.ImportExport;

    public class UnloadCounterValuesController : BaseController
    {
        /// <summary>
        /// Выгрузить показания ПУ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult Unload(BaseParams baseParams)
        {
            var service = Container.Resolve<IUnloadCounterValuesService>();

            try
            {
                var result = service.Unload(baseParams);
                return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        /// <summary>
        /// Получить список выгрузок показаний ПУ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetList(BaseParams baseParams)
        {
            var service = Container.Resolve<IUnloadCounterValuesService>();

            try
            {
                var result = (ListDataResult)service.GetList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}