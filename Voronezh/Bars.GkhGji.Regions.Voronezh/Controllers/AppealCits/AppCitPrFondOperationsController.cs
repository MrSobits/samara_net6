namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Entities;
    using Bars.B4;
    using DomainService;
    using System.Collections;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;

    public class AppCitPrFondOperationsController : BaseController
    {
        public IAppCitPrFondOperationsService service { get; set; }
        
        public ActionResult GetListObjectCr(BaseParams baseParams)
        {
           
            try
            {
                return service.GetListObjectCr(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

    }
 
}