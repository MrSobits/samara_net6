namespace Bars.Gkh1468.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Utils;
    using Bars.Gkh1468.DataFiller;
    using Bars.Gkh1468.Enums;

    public class DataFillerController : BaseController
    {
        public ActionResult List(BaseParams baseParams)
        {
            var loadParam = baseParams.Params.Read((LoadParam)null).Execute(Converter.ToLoadParam);

            var metaType = baseParams.Params.GetAs<MetaAttributeType>("Type");
            var valueType = baseParams.Params.GetAs<ValueType>("ValueType");

            var multiple = metaType == MetaAttributeType.GroupedComplex;

            var allFillers = Container.ResolveAll<IDataFiller>()
                                      .Select(x => new { x.Name, x.Code, x.Multiple, x.ValueType })
                                      .AsQueryable()
                                      .WhereIf(multiple, x => x.Multiple)
                                      .Where(x => x.ValueType == valueType)
                                      .FilterPrimitive(loadParam, Container);

            return new JsonListResult(allFillers.Order(loadParam).Paging(loadParam), allFillers.Count());
        }

        public ActionResult ListAll(BaseParams baseParams)
        {
            var allFillers = Container.ResolveAll<IDataFiller>()
                                      .Select(x => new { x.Name, x.Code, x.Multiple, x.ValueType })
                                      .ToArray();

            return new JsonListResult(allFillers);
        }
    }
}