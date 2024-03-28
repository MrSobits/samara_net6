namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.DomainService;

    using Entities;

    public class ShortProgramDeficitController : B4.Alt.DataController<ShortProgramDifitsit>
    {
        public IShortProgramDeficitService Service { get; set; }

        public ActionResult SaveDeficit(BaseParams baseParams)
        {
            return new JsonNetResult(Service.SaveDeficit(baseParams));
        }

        public ActionResult CreateShortProgram(BaseParams baseParams)
        {
            return new JsonNetResult(Service.CreateShortProgram(baseParams));
        }
    }
}
