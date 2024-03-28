namespace Bars.Gkh.Controllers.RealityObj
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.RealityObj;

    public class RealityObjectBuildingFeatureController : B4.Alt.BaseDataController<RealityObjectBuildingFeature>
    {
        public ActionResult SaveFeatures(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAs<long>("objectId");
            var featuresIds = baseParams.Params.GetAs("featureIds", string.Empty).Split(',').Select(x => x.To<long>()).ToArray();

            var roDomain = Container.Resolve<IDomainService<RealityObject>>();
            var featuresDomain = Container.Resolve<IDomainService<BuildingFeature>>();

            using (Container.Using(roDomain, featuresDomain))
            {
                var ro = roDomain.FirstOrDefault(x => x.Id == roId);
                var features = featuresDomain.GetAll().Where(x => featuresIds.Contains(x.Id));

                return new JsonResult(InternalSaveFeatures(ro, features));
            }

        }

        private IDataResult InternalSaveFeatures(RealityObject ro, IEnumerable<BuildingFeature> features)
        {
            var roBuildFeatureDomain = Container.Resolve<IDomainService<RealityObjectBuildingFeature>>();
            using (Container.Using(roBuildFeatureDomain))
            {
                var existFeatures = roBuildFeatureDomain.GetAll().Where(x => x.RealityObject.Id == ro.Id).ToList();
                var existFeaturesIds = existFeatures.Select(x => x.BuildingFeature.Id);
                var addingFeatures = features.Where(x => !existFeaturesIds.Contains(x.Id));
                var removingRoFeature = existFeatures.Where(x => !existFeaturesIds.Contains(x.Id));

                using (var transaction = Container.Resolve<IDataTransaction>())
                {

                    foreach (var roFeature in removingRoFeature)
                    {
                        roBuildFeatureDomain.Delete(roFeature.Id);
                    }

                    foreach (var buildingFeature in addingFeatures)
                    {
                        roBuildFeatureDomain.Save(new RealityObjectBuildingFeature
                        {
                            RealityObject = ro,
                            BuildingFeature = buildingFeature
                        });
                    }
                    transaction.Commit();
                }
            }
            return new BaseDataResult();
        }
    }
}
