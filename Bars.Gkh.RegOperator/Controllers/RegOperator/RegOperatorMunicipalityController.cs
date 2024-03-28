namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.RegOperator.DomainService;
    using Entities;

    public class RegOperatorMunicipalityController : B4.Alt.DataController<RegOperatorMunicipality>
    {
        public ActionResult AddMunicipalities(BaseParams baseParams)
        {
            var result = Container.Resolve<IRegOperatorMunicipalityService>().AddMunicipalities(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListMuByRegOp(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<IRegOperatorMunicipalityService>().ListMuByRegOp(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}