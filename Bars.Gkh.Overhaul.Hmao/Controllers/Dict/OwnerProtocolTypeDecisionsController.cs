namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Hmao.DomainService.Impl;
    using DomainService;
    using Entities;

    public class OwnerProtocolTypeDecisionsController : B4.Alt.DataController<OwnerProtocolTypeDecision>
    {

        public ActionResult SelectDecision(BaseParams baseParams)
        {
            var ownerProtocolsService = Container.Resolve<IOwnerProtocolTypeDecisionsService>();
            try
            {
                return ownerProtocolsService.SelectDecision(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }
    }
}