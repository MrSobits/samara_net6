namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Controllers;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Chelyabinsk.DomainService;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;

    public class AppealCitsTransferResultController : BaseGkhDataController<AppealCitsTransferResult>
    {
        public ActionResult Restart(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var client = this.Container.Resolve<ICitizensAppealServiceClient>();
            using (this.Container.Using(client))
            {
                return client.RestartAppealCitsTransfer(id).ToJsonResult();
            }
        }
    }
}