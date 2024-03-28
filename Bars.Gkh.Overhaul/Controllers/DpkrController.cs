namespace Bars.Gkh.Overhaul.Controllers
{
    using System;
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.DomainService;

    using Castle.Windsor;

    public class DpkrController : B4.Alt.DataController<RealityObject>
    {
        public IWindsorContainer Container { get; set; }
        
        public ActionResult CreateProgramCrByDpkr(BaseParams baseParams)
        {
            var service = Container.Resolve<IDpkrService>();

            try
            {
                if (service == null)
                {
                    return JsonNetResult.Failure("Не найдена реализация формирования программы КР на основе Региональной");
                }

                var result = service.CreateProgramCrByDpkr(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult GetListForAddInObjectCr(BaseParams baseParams)
        {
            var service = Container.Resolve<IObjectCrIntegrationService>();

            try
            {
                if (service == null)
                {
                    return JsonNetResult.Failure("Не найдена реализация интеграции ДПКР с Объектом КР");
                }

                var totalCount = 0;
                var result = service.GetListWorksForObjectCr(baseParams, ref totalCount);

                return new JsonListResult((IList)result.Data, totalCount);

            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult AddWorks(BaseParams baseParams)
        {
            var service = Container.Resolve<IObjectCrIntegrationService>();

            try
            {
                if (service == null)
                {
                    return JsonNetResult.Failure("Не найдена реализация интеграции ДПКР с Объектом КР");
                }

                var result = service.AddWorks(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}
