namespace Bars.GkhCr.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections;
    using Bars.B4;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    public class QualificationMemberController : B4.Alt.DataController<QualificationMember>
    {
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IQualificationMemberService>().GetInfo(baseParams);
            if (result.Success)
            {
                return new JsonNetResult(result.Data);
            }

            return JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddRoles(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IQualificationMemberService>().AddRoles(baseParams);
            if (result.Success)
            {
                return new JsonNetResult(new { success = true });
            }

            return JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListRoles(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<IQualificationMemberService>().ListRoles(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}
