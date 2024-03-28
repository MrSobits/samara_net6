namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ActivityTsjRealObjService : IActivityTsjRealObjService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddRealityObjects(BaseParams baseParams)
        {
            try
            {
                var activityTsjId = baseParams.Params.ContainsKey("activityTSJ") ? baseParams.Params["activityTSJ"].ToLong() : 0;

                var objIds = baseParams.Params.ContainsKey("objectIds") ? baseParams.Params["objectIds"].ToString() : string.Empty;

                if (!string.IsNullOrEmpty(objIds))
                {
                    var objectIds = objIds.Split(',');

                    var service = Container.Resolve<IDomainService<ActivityTsjRealObj>>();

                    // получаем у контроллера дома что бы не добавлять их повторно
                    var existingActivityTsjRealityObjects = service.GetAll().Where(x => x.ActivityTsj.Id == activityTsjId).Select(x => x.RealityObject.Id).ToList();

                    foreach (var id in objectIds)
                    {
                        if (existingActivityTsjRealityObjects.Contains(id.ToLong()))
                            continue;

                        var newId = id.ToLong();

                        var newActivityTsjRealityObjects = new ActivityTsjRealObj
                        {
                            RealityObject = new RealityObject { Id = newId },
                            ActivityTsj = new ActivityTsj { Id = activityTsjId }
                        };

                        service.Save(newActivityTsjRealityObjects);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult{Success = false, Message = e.Message};
            }
        }

        public IDataResult GetInfo(long? activityTsjId)
        {
            try
            {
                var serviceActivityTsjGji = Container.Resolve<IDomainService<ActivityTsj>>();
                var serviceManOrgRealObj = Container.Resolve<IDomainService<ManagingOrgRealityObject>>();

                // Получим упр организацию
                var manOrg = serviceActivityTsjGji.Get(activityTsjId) != null ? serviceActivityTsjGji.Get(activityTsjId).ManagingOrganization.Id : 0;

                // Получим дома в управление по этой упр орг
                var manOrgRealObjArray = serviceManOrgRealObj
                    .GetAll()
                    .Where(x => x.ManagingOrganization.Id == manOrg)
                    .Select(x => x.RealityObject.Id)
                    .Distinct()
                    .ToArray();

                var realityObjIds = manOrgRealObjArray.Length > 0 ? manOrgRealObjArray.Select(x => x.ToString()).Aggregate((current, next) => current + ", " + next) : "0";

                return new BaseDataResult(new {realityObjIds});
            }
            catch (ValidationException e)
            {
                return new BaseDataResult {Success = false, Message = e.Message};
            }
        }
    }
}