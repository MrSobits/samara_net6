namespace Bars.GkhGji.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections;

    using B4;
    using Entities;
    using B4.Modules.FileStorage;
    using DomainService;

    public class HeatSeasonDocController : FileStorageDataController<HeatSeasonDoc>
    {
        public ActionResult ListView(BaseParams baseParams)
        {
            var servcie = Container.Resolve<IHeatSeasonDocService>();

            try
            {
                var result = (ListDataResult)servcie.ListView(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(servcie);
            }
        }

        public ActionResult MassChangeState(BaseParams baseParams)
        {
            var service = Container.Resolve<IHeatSeasonDocService>();

            try
            {
                var result = service.MassChangeState(baseParams);
                return result.Success ? new JsonNetResult(new {success = true, message = result.Message}) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
            
        }

        public ActionResult ListDocumentTypes(BaseParams baseParams)
        {
            var service = Container.Resolve<IHeatSeasonDocService>();
            try
            {
                var result = (ListDataResult)service.ListDocumentTypes(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}