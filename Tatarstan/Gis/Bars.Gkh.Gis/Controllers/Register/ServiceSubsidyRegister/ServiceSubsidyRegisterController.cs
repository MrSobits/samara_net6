namespace Bars.Gkh.Gis.Controllers.Register.ServiceSubsidyRegister
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.Register.ServiceSubsidyRegister;
    using Entities.Register.ServiceSubsidyRegister;

    public class ServiceSubsidyRegisterController: B4.Alt.DataController<ServiceSubsidyRegister>
    {
        protected IServiceSubsidyRegisterService Service;

        public ServiceSubsidyRegisterController(IServiceSubsidyRegisterService service)
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
