namespace Bars.GkhGji.Regions.Tyumen.Controllers
{
    using Bars.Gkh.Entities;
    using DomainService;
    using Microsoft.AspNetCore.Mvc;
    using B4;


    public class ManOrgLicenseGisController : B4.Alt.DataController<ManOrgLicense>
    {

        public virtual IManOrgLicenseGisService Service { get; set; }

        public ActionResult GetListWithRO(BaseParams baseParams)
        {
            var totalCount = 0;
            var result = Service.GetListWithRO(baseParams, true, ref totalCount);
            return new JsonListResult(result, totalCount);
        }

        public override ActionResult Get(BaseParams baseParams)
        {
            var totalCount = 0;

            var result = Service.GetRO(baseParams, true, ref totalCount);

            return new JsonListResult(result, totalCount);
        }
    }
}