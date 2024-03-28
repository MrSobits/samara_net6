namespace Bars.Gkh.Controllers
{
    using B4;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Entities;

    public class FieldsRequirementController : BaseController
    {
        public ActionResult GetRequirements(string requirements)
        {
            if (requirements == null)
            {
                return JsonNetResult.Failure("Нет требований обязательности!");
            }

            var parsedRequirements = JsonNetConvert.DeserializeObject<List<string>>(Container, requirements);
            var dict = Container.Resolve<IDomainService<FieldRequirement>>().GetAll()
                .Select(x => x.RequirementId)
                .AsEnumerable()
                .Distinct()
                .ToDictionary(x => x);

            int[] result = parsedRequirements.Select(x => dict.ContainsKey(x) ? 1 : 0).ToArray();
            
            return new JsonNetResult(result);
        }
    }
}
