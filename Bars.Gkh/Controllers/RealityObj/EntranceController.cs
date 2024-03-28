namespace Bars.Gkh.Controllers.RealityObj
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Repositories;

    public class EntranceController : B4.Alt.DataController<Entrance>
    {
        public ActionResult GetTariff(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAsId("roId");
            var retId = baseParams.Params.GetAsId("retId");

            var repo = Container.Resolve<IEntranceTariffRepository>();
            try
            {
                return JsSuccess(repo.GetRetTariff(roId, retId, DateTime.Today));
            }
            finally
            {
                Container.Release(repo);
            }
        }
    }
}