namespace Bars.B4.Modules.Analytics.Web.Controllers
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Domain;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class FilterExpressionController : BaseController
    {
        public ActionResult List(StoreLoadParams storeParams)
        {
            var dataProviders = Container.Resolve<IFilterExpressionService>().GetAll();
            var totalCount = dataProviders.Count();
            dataProviders = dataProviders.Order(storeParams).Paging(storeParams);

            return new JsonListResult(dataProviders.ToList(), totalCount);
        }

        public ActionResult Get(string key)
        {
            var dataProvider = Container.Resolve<IFilterExpressionService>().Get(key);
            return new JsonGetResult(dataProvider);
        }
    }
}
