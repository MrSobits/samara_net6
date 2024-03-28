namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ActivityTsjProtocolRealObjController : B4.Alt.DataController<ActivityTsjProtocolRealObj>
    {
        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var result = Container.Resolve<IActivityTsjProtocolRealObjService>().AddRealityObjects(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /*public ActionResult GetInfo()
        {
            try
            {
                var activityTSJIdId = Request["activityTSJ"].ToLong();

                var serviceActivityTsjGji = this.Resolve<IDomainService<ActivityTsjGji>>();
                var serviceManOrgRealObj = this.Resolve<IDomainService<ManagingOrgRealityObject>>();

                //Получим упр организацию
                var manOrg = serviceActivityTsjGji.Get(activityTSJIdId) != null ? serviceActivityTsjGji.Get(activityTSJIdId).ManagingOrganization.Id : 0;
                //Получим дома в управление по этой упр орг
                var manOrgRealObjArray = serviceManOrgRealObj
                    .GetAll()
                    .Where(x => x.ManagingOrganization.Id == manOrg)
                    .Select(x => x.RealityObject.Id)
                    .Distinct()
                    .ToArray();

                var realityObjIds = manOrgRealObjArray.Length > 0 ? manOrgRealObjArray.Select(x => x.ToStr()).Aggregate((current, next) => current + ", " + next) : "0";

                return new JsonNetResult(new { success = true, realityObjIds });
            }
            catch (ValidationException exc)
            {
                return JsonNetResult.Failure(exc.Message);
            }
        }*/
    }
}
