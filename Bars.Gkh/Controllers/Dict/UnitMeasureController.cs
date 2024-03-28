namespace Bars.Gkh.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class UnitMeasureController : B4.Alt.DataController<UnitMeasure>
    {
        public IUnitMeasureService Service { get; set; }

        public ActionResult ListNoPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListNoPaging(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}