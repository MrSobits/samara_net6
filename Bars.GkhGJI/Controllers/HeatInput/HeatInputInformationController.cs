namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class HeatInputInformationController : B4.Alt.DataController<HeatInputInformation>
    {
        public IHeatInputService Service { get; set; }

        public ActionResult GetBoilerInfo(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<IHeatInputService>().GetBoilerInfo(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SaveHeatInputInfo(BaseParams baseParams)
        {
            return new JsonNetResult(Service.SaveHeatInputInfo(baseParams));
        }
    }
}