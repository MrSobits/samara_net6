using Bars.B4;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Bars.GkhGji.Entities;
using Bars.GkhGji.NumberValidation;

namespace Bars.GkhGji.Controllers
{
    public class DocNumValidationRuleController : B4.Alt.DataController<DocNumValidationRule>
    {
        public ActionResult ListRules(BaseParams baseParams)
        {
            return new JsonListResult(Container.ResolveAll<INumberValidationRule>().Select(x => new {x.Name, x.Id}));
        }
    }
}