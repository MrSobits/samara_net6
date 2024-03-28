namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System.Collections;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Bars.B4.IoC;
    using Entities;
    using Gkh.Domain;
    using PriorityParams;

    public class MultiPriorityParamController : B4.Alt.DataController<MultiPriorityParam>
    {
        public ActionResult ListSelect(BaseParams baseParams)
        {
            var code = baseParams.Params.GetAs<string>("code");

            var multiPriorParams = Container.ResolveAll<IMultiPriorityParam>();

            IMultiPriorityParam priorParams;

            using (Container.Using(multiPriorParams))
            {
                priorParams = multiPriorParams.FirstOrDefault(x => x.Id == code);
            }

            if (priorParams != null)
            {
                var viewModelType = typeof (IViewModel<>).MakeGenericType(priorParams.Type);
                var viewModel = Container.Resolve(viewModelType);

                var domainServiceType = typeof (IDomainService<>).MakeGenericType(priorParams.Type);
                var domain = Container.Resolve(domainServiceType);

                var method = viewModelType.GetMethod("List");

                using (Container.Using(viewModel, domain))
                {
                    var result = (ListDataResult)method.Invoke(viewModel, new[] { domain, baseParams });
                    return new JsonListResult((IList)result.Data, result.TotalCount);
                }
            }

            return new JsonListResult(null);
        }

        public ActionResult ListSelected(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("id");

            if (id > 0)
            {
                var loadParam = baseParams.GetLoadParam();
                var multiPriorParams = Container.ResolveAll<IMultiPriorityParam>();

                using (Container.Using(multiPriorParams))
                {
                    var obj = DomainService.Get(id);

                    if (obj.StoredValues != null)
                    {
                        return new JsonListResult(obj.StoredValues.AsQueryable().Order(loadParam).ToList());
                    }
                }
            }

            return new JsonListResult(null);
        }
    }
}