namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class ContragentClwController : B4.Alt.DataController<ContragentClw>
    {
        public IContragentClwService Service { get; set; }

        public ActionResult AddMunicipalities(BaseParams baseParams)
        {
            var result = Service.AddMunicipalities(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}