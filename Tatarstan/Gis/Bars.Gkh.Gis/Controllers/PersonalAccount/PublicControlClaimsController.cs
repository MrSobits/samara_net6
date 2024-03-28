namespace Bars.Gkh.Gis.Controllers.House
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.House.Claims;

    public class PublicControlClaimsController : BaseController
    {
        protected IPublicControlClaimsService Service;

        public PublicControlClaimsController(IPublicControlClaimsService service)
        {
            Service = service;
        }

        /// <summary>
        /// Список претензий из Народного Контроля
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