namespace Bars.Gkh.Gis.Controllers.GisRealEstate
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.RealEstate;
    using Entities.RealEstate.GisRealEstateType;

    public class GisRealEstateTypeController :  B4.Alt.DataController<GisRealEstateType>
    {
        protected IRealEstateTypeService Service { get; set; }

        public GisRealEstateTypeController(IRealEstateTypeService service)
        {
            Service = service;
        }

        public ActionResult GroupedTypeList(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.GroupedTypeList(baseParams);
            return new JsonNetResult(new
            {
                success = true, 
                children = result.Data
            });
        }

        public override ActionResult Delete(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.Delete(baseParams);
            base.Delete(baseParams);
            return new JsonNetResult(new
            {
                success = true
            });
        }
    }
}