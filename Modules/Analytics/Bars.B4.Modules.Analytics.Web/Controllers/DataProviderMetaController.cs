namespace Bars.B4.Modules.Analytics.Web.Controllers
{
    using System;
    using System.Linq;
    using Bars.B4.Modules.Analytics.Domain;

    using Microsoft.AspNetCore.Mvc;

    public class DataProviderMetaController : BaseController
    {
        public ActionResult List(StoreLoadParams storeParams)
        {
            var dataProviders = Container.Resolve<IDataProviderService>().GetAll();
            var totalCount = dataProviders.Count();
            dataProviders = dataProviders.Order(storeParams).Paging(storeParams);

            return
                new JsonListResult(
                    dataProviders.Select(
                        x =>
                            new
                            {
                                x.Name,
                                x.Key,
                                //Fields =
                                //    x.ProvideMetaData().Select(y => new { text = y.Key, code = y.Key, type = GetType(y.Value) })
                            })
                        .ToList(),
                    totalCount);
        }

        public ActionResult Get(string key)
        {
            var dataProvider = Container.Resolve<IDataProviderService>().Get(key);
            return new JsonGetResult(new
                            {
                                dataProvider.Name,
                                //Fields =
                                //    dataProvider.ProvideMetaData().Select(y => new { text = y.Key, code = y.Key, type = GetType(y.Value) })
                            });
        }

        private string GetType(Type type)
        {
            return "string";
        }
    }
}
