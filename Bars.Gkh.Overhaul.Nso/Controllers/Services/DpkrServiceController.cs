namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.Gkh.Overhaul.Nso.DomainService;

    using Entities;

    public class DpkrServiceController : B4.Alt.DataController<ProgramVersion>
    {
        public ActionResult GetYears(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IDpkrService>().GetYears(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetMunicipality(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IDpkrService>().GetMunicipality(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetRealityObjects(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IDpkrService>().GetRealityObjects(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
        public ActionResult GetRecords(BaseParams baseParams)
        {
            var result = Resolve<IDpkrService>().GetRecords(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }
    }
}

