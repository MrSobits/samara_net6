namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class ContragentMunicipalityController : B4.Alt.DataController<ContragentMunicipality>
    {
        public IContragentService Service { get; set; }

        public ActionResult ListAvailableMunicipality(BaseParams baseParams)
        {
            var data = Service.ListAvailableMunicipality(baseParams) as ListDataResult;
            return new JsonNetResult(new { data =  data.Data, totalCount = data.TotalCount});
        }
    }
}