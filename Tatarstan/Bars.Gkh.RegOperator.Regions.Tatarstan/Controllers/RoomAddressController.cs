namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.DomainService.ChangeRoomAddress;

    public class RoomAddressController : BaseController
    {
        public ActionResult SaveChange(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IChangeRoomAddressService>();

            using (this.Container.Using(service))
            {
                var result = service.SaveRoomAddress(baseParams);

                return new JsonNetResult(result);
            }
        }
    }
}