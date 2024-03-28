namespace Bars.Gkh.Gis.Controllers.Register.TenantSubsidyRegister
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.Register.TenantSubsidyRegister;
    using Entities.Register.TenantSubsidyRegister;

    public class TenantSubsidyRegisterController: B4.Alt.DataController<TenantSubsidyRegister>
    {
        protected ITenantSubsidyRegisterService Service;

        public TenantSubsidyRegisterController(ITenantSubsidyRegisterService service)
        {
            Service = service;
        }

        public ActionResult ListByApartmentId(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListByApartmentId(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}
