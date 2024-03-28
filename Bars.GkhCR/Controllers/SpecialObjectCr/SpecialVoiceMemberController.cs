namespace Bars.GkhCr.Controllers
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class SpecialVoiceMemberController : B4.Alt.DataController<SpecialVoiceMember>
    {
        public ActionResult SaveVoiceMembers(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ISpecialVoiceMemberService>();
            using (this.Container.Using(service))
            {
                var result = (BaseDataResult) service.SaveVoiceMembers(baseParams);
                return result.Success ? new JsonNetResult(new { succes = true }) : JsonNetResult.Failure(result.Message);
            }
        }
    }
}