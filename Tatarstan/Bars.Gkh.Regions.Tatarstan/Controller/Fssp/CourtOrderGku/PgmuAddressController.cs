namespace Bars.Gkh.Regions.Tatarstan.Controller.Fssp.CourtOrderGku
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;

    /// <summary>
    /// Контроллер для <see cref="PgmuAddress"/>
    /// </summary>
    public class PgmuAddressController : B4.Alt.DataController<PgmuAddress>
    {
        /// <summary>
        /// Получить список значений адресных объектов по совпадающей строке
        /// </summary>
        public ActionResult GetAddressObjectList(BaseParams baseParams)
        {
            var pgmuAddressService = this.Container.Resolve<IPgmuAddressService>();
            var parentValuesDict = baseParams.Params.GetAs<Dictionary<string, string>>("parentValuesDict");
            var value = baseParams.Params.GetAs<string>("value");
            var propertyName = baseParams.Params.GetAs<string>("propertyName");

            using (this.Container.Using(pgmuAddressService))
            {
                return new JsonListResult(pgmuAddressService.GetPgmuAddressObjectValue(propertyName, value, parentValuesDict));
            }
        }

        /// <summary>
        /// Получить идентификатор адреса ПГМУ по его составляющим
        /// </summary>
        public ActionResult GetAddressId(BaseParams baseParams)
        {
            var pgmuAddressService = this.Container.Resolve<IPgmuAddressService>();
            var addressObjectDict = baseParams.Params.GetAs<Dictionary<string, string>>("valuesDict");
            
            using (this.Container.Using(pgmuAddressService))
            {
                return new JsonNetResult(pgmuAddressService.GetPgmuAddressId(addressObjectDict));
            }
        }
    }
}