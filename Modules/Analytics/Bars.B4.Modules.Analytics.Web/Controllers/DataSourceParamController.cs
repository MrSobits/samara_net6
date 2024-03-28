namespace Bars.B4.Modules.Analytics.Web.Controllers
{
    using System.Linq;
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Analytics.Utils;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class DataSourceParamController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult List(BaseParams baseParams)
        {
            var dataSourcesIds = baseParams.Params.GetAs("dataSourcesIds", string.Empty, ignoreCase:true).ToLongArray();
            var dataSourceDomain = Container.Resolve<IDomainService<DataSource>>();

            var dataSources = dataSourceDomain.GetAll()
                    .Where(x => dataSourcesIds.Contains(x.Id))
                    .ToList();

            var @params = new List<object>();

            foreach(var ds in dataSources){
                @params.AddRange(ds.GetParentDataProvider().Params.Select(x => new { 
                    x.Additional,
                    x.DefaultValue,
                    x.Label,
                    x.OwnerType,
                    x.Name,
                    x.ParamType,
                    x.Required,
                    x.Multiselect,
                    DataSourceId = ds.Id
                }));
            }

            Container.Release(dataSourceDomain);
            return new JsonGetResult(@params);
        }
    }
}
