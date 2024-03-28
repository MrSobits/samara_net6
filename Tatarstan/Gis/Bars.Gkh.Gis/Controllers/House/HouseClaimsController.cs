namespace Bars.Gkh.Gis.Controllers.House
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.House.Claims;

    public class HouseClaimsController : BaseController
    {
        protected IHouseClaimsService Service;

        public HouseClaimsController(IHouseClaimsService service)
        {
            Service = service;
        }

        /// <summary>
        /// Список претензий из Открытой Казани
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult OrderList(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.OrderList(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}