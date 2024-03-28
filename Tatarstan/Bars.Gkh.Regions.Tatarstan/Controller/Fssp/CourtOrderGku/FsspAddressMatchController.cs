namespace Bars.Gkh.Regions.Tatarstan.Controller.Fssp.CourtOrderGku
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;
    using Bars.Gkh.Regions.Tatarstan.ViewModel.Fssp.CourtOrderGku;

    public class FsspAddressMatchController : B4.Alt.DataController<FsspAddressMatch>
    {
        public ActionResult ImportedAddressMatchingList(BaseParams baseParams)
        {
            return new JsonNetResult((this.ViewModel as FsspAddressMatchViewModel).ImportedAddressMatchingList(this.DomainService, baseParams));
        }
    }
}