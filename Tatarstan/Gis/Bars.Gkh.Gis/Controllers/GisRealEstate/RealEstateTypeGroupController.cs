namespace Bars.Gkh.Gis.Controllers.GisRealEstate
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.RealEstate;
    using Entities.RealEstate.GisRealEstateType;

    public class GisRealEstateTypeGroupController : B4.Alt.DataController<GisRealEstateTypeGroup>
    {
        protected IRealEstateTypeGroupService Service;

        public GisRealEstateTypeGroupController(IRealEstateTypeGroupService service)
        {
            Service = service;
        }

        /// <summary>
        /// Список групп без пейджинга
        /// </summary>
        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var res = Service.ListWithoutPaging(baseParams);
            return Json(res);
        }
    }
}