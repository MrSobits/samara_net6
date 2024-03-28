
namespace Bars.B4.Modules.Analytics.Reports.Web.Controllers
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Domain;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class CodedReportController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeParams"></param>
        /// <returns></returns>
        public ActionResult List(StoreLoadParams storeParams)
        {
            var dataProviders = Container.Resolve<ICodedReportService>().GetAll();
            var totalCount = dataProviders.Count();
            dataProviders = dataProviders.Order(storeParams).Paging(storeParams);

            return new JsonListResult(dataProviders.ToList(), totalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult Get(string key)
        {
            var dataProvider = Container.Resolve<ICodedReportService>().Get(key);
            return new JsonGetResult(dataProvider);
        }
    }
}
