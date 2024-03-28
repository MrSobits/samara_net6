namespace Bars.Gkh.Regions.Tatarstan.Controller.Fssp.CourtOrderGku
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;

    public class FsspAddressController : B4.Alt.DataController<FsspAddress>
    {
        public ActionResult AddressMatch(BaseParams baseParams)
        {
            var fsspAddressId = baseParams.Params.GetAs<long>("fsspAddressId");
            var pgmuAddressId = baseParams.Params.GetAs<long>("pgmuAddressId");
            var fsspAddressService = this.Container.Resolve<IFsspAddressService>();
            
            using (this.Container.Using(fsspAddressService))
            {
                var result = fsspAddressService.AddressMatch(fsspAddressId, pgmuAddressId);

                if (result.Success)
                {
                    return JsonNetResult.Success;
                }

                return JsonNetResult.Failure(result.Message);
            }
        }
        
        public ActionResult AddressMismatch(BaseParams baseParams)
        {
            var fsspAddressId = baseParams.Params.GetAsId();
            var fsspAddressService = this.Container.Resolve<IFsspAddressService>();

            using (this.Container.Using(fsspAddressService))
            {
                var result = fsspAddressService.AddressMismatch(fsspAddressId);

                if (result.Success)
                {
                    return JsonNetResult.Success;
                }

                return JsonNetResult.Failure(result.Message);
            }
        }
    }
}