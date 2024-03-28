namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using System.Collections;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Entities;
    using PriorityParams;

    public class MultiPriorityParamController : B4.Alt.DataController<MultiPriorityParam>
    {
        public ActionResult ListSelect(BaseParams baseParams)
        {
            var code = baseParams.Params.GetAs<string>("code");

            var priorParams = Container.ResolveAll<IMultiPriorityParam>().FirstOrDefault(x => x.Id == code);

            if (priorParams != null)
            {
                var viewModelType = typeof (IViewModel<>).MakeGenericType(priorParams.Type);
                var viewModel = Container.Resolve(viewModelType);

                var domainServiceType = typeof (IDomainService<>).MakeGenericType(priorParams.Type);
                var domain = Container.Resolve(domainServiceType);

                var method = viewModelType.GetMethod("List");

                var result = (ListDataResult)method.Invoke(viewModel, new[] { domain, baseParams });
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }

            return new JsonListResult(null);
        }

        public ActionResult ListSelected(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("Id");

            if (id > 0)
            {
                var loadParam = baseParams.GetLoadParam();
                var domainService = Container.Resolve<IDomainService<MultiPriorityParam>>();

                var obj = domainService.Get(id);

                if (obj.Value != null)
                {
                    return new JsonListResult(obj.Value.Split(',').Select(x => new { Code = x }).AsQueryable().Order(loadParam).ToList());
                }
            }

            return new JsonListResult(null);
        }
    }
}