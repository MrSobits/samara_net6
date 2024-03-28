﻿namespace Bars.GkhGji.Regions.Tatarstan.Controller.TatarstanProtocolGji
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiRealityObjectController : B4.Alt.DataController<TatarstanProtocolGjiRealityObject>
    {
        public ActionResult SaveRealityObjects(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ITatarstanProtocolGjiRealityObjectService>();
            using (this.Container.Using(service))
            {
                var result = service.SaveRealityObjects(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
        }
    }
}
