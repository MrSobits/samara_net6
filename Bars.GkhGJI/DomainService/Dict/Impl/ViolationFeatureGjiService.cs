namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ViolationFeatureGjiService : IViolationFeatureGjiService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddFeatureViolations(BaseParams baseParams)
        {
            var violationFeatureGjiDomain = Container.ResolveDomain<ViolationFeatureGji>();

            try
            {
                var featureId = baseParams.Params.GetAsId("featureId");
                var violationIds = baseParams.Params.GetAs<string>("violationIds").ToLongArray();

                var existViols = violationFeatureGjiDomain.GetAll()
                    .Where(x => x.FeatureViolGji.Id == featureId)
                    .Select(x => x.ViolationGji.Id)
                    .ToList();

                foreach (var id in violationIds)
                {
                    if (!existViols.Contains(id))
                    {
                        var newViolationFeatureGji = new ViolationFeatureGji
                        {
                            ViolationGji = new ViolationGji {Id = id},
                            FeatureViolGji = new  FeatureViolGji {Id = featureId}
                        };

                        violationFeatureGjiDomain.Save(newViolationFeatureGji);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new {success = false, message = e.Message});
            }
            finally
            {
                Container.Release(violationFeatureGjiDomain);
            }
        }

        public IDataResult SaveViolationGroups(BaseParams baseParams)
        {
            try
            {
                var violationId = baseParams.Params.ContainsKey("violationId") ? baseParams.Params["violationId"].ToLong() : 0;
                var featuresString = baseParams.Params.ContainsKey("groupIds") ? baseParams.Params["groupIds"].ToStr() : string.Empty;

                if (string.IsNullOrEmpty(featuresString))
                {
                    return new BaseDataResult();
                }

                var featuresIds = featuresString.Split(',').Select(item => item.ToLong()).ToList();

                var serviceViolationFeatureGji = Container.Resolve<IDomainService<ViolationFeatureGji>>();

                var existingGroups = serviceViolationFeatureGji.GetAll()
                    .Where(x => x.ViolationGji.Id == violationId)
                    .Select(x => new
                    {
                        x.Id,
                        FeatureId = x.FeatureViolGji.Id
                    }).ToList();

                var newGroups = featuresIds.Where(id => existingGroups.All(groupId => groupId.FeatureId != id)).ToList();
                var removedGroups = existingGroups.Where(id => featuresIds.All(groupId => groupId != id.FeatureId)).ToList();
                foreach (var removedGroup in removedGroups)
                {
                    serviceViolationFeatureGji.Delete(removedGroup.Id);
                }

                foreach (var newGroup in newGroups)
                {
                    serviceViolationFeatureGji.Save(new ViolationFeatureGji
                    {
                        FeatureViolGji = new  FeatureViolGji
                        {
                            Id = newGroup
                        },
                        ViolationGji = new ViolationGji
                        {
                            Id = violationId
                        }
                    });
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new { success = false, message = e.Message });
            }
        }
    }
}