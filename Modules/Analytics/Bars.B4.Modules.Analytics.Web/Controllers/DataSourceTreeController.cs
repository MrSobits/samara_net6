namespace Bars.B4.Modules.Analytics.Web.Controllers
{
    using System.Linq;
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Analytics.Web.Data;

    using Microsoft.AspNetCore.Mvc;

    public class DataSourceTreeController : BaseController
    {
        public ActionResult GetTree(BaseParams baseParams)
        {
            var treeBuilder = new DataSourceTreeBuilder();
            var dataSourceDomain = Container.Resolve<IDomainService<DataSource>>();
            var dataSourceId = baseParams.Params.GetAs<long>("dataSourceId", ignoreCase: true);
            var checkable = baseParams.Params.GetAs<bool>("checkable", ignoreCase: true, defaultValue: true);
            DataSourceTreeBuilder.Node result;
            if (dataSourceId > 0)
            {
                var dataSource = dataSourceDomain.Get(dataSourceId);
                result = treeBuilder.BuildTree(dataSource, checkable: checkable);
            }
            else
            {
                result = treeBuilder.BuildTree(dataSourceDomain.GetAll().ToList(), checkable: checkable);
            }
            Container.Release(dataSourceDomain);
            return JsSuccess(result.children);
        }
    }
}
