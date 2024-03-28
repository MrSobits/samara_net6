using System;
using System.Collections.Generic;
using System.Linq;
using Bars.Gkh.Gasu.DomainService;
using Bars.Gkh.Gasu.Entities;
using NHibernate.Mapping;

namespace Bars.Gkh.Gasu.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    public class GasuIndicatorValueController : B4.Alt.DataController<GasuIndicatorValue>
    {

        public ActionResult GetListYears(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IGasuIndicatorValueService>();
            try
            {
                var result = service.GetListYears(baseParams);

                if (!result.Success)
                {
                    return JsFailure(result.Message);
                }

                var list = (List<int>)result.Data;

                return new JsonListResult(list.Select(x => new { Year = x }).ToList(), list.Count);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult CreateRecords(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IGasuIndicatorValueService>();
            try
            {
                var result = service.CreateRecords(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsFailure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult SendService(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IGasuImportExportService>();
            try
            {
                baseParams.Params.Add("exportName", "GasuIndicatorExport");
                baseParams.Params.Add("gasuAddress", Server.MapPath("~/gasu.config"));

                var result = service.SendGasu(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}