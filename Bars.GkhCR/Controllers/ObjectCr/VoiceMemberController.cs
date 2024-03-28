namespace Bars.GkhCr.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    public class VoiceMemberController : B4.Alt.DataController<VoiceMember>
    {
        public ActionResult SaveVoiceMembers(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IVoiceMemberService>().SaveVoiceMembers(baseParams);
            return result.Success ? new JsonNetResult(new { succes = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}