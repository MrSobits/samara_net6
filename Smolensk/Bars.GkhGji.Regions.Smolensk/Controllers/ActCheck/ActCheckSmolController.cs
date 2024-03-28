namespace Bars.GkhGji.Regions.Smolensk.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    public class ActCheckSmolController : Bars.GkhGji.Controllers.ActCheckController<ActCheckSmol>
    {
        public ActionResult GetListRealityObjectByInspection(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var actDomain = Container.Resolve<IDomainService<ActCheck>>();
            var inspectionRoDomain = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var actCheckRoDomain = Container.Resolve<IDomainService<ActCheckRealityObject>>();

            try
            {
                var actId = baseParams.Params.GetAs("actCheckId", 0L);

                var act = actDomain.Get(actId);

                if (act == null)
                {
                    return JsonNetResult.Failure(string.Format("Не удалось получить акт проверки по Id {0}", actId));
                }

                var data = inspectionRoDomain.GetAll()
                    .Where(x => x.Inspection.Id == act.Inspection.Id)
                    .Where(x => !actCheckRoDomain.GetAll()
                        .Where(z => z.ActCheck.Id == actId)
                        .Any(y => y.RealityObject.Id == x.RealityObject.Id))
                    .Select(x => new
                    {
                        x.RealityObject.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        x.RealityObject.Address
                    })
                    .Filter(loadParams, Container);

                var count = data.Count();

                return new JsonListResult(data.Order(loadParams).Paging(loadParams).ToList(), count);
            }
            finally 
            {
                Container.Release(actDomain);
                Container.Release(inspectionRoDomain);
                Container.Release(actCheckRoDomain);
            }
        }

    }
}