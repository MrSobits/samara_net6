namespace Bars.GkhGji.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.B4;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    public class ViolationFeatureGjiController : B4.Alt.DataController<ViolationFeatureGji>
    {
        public ActionResult AddFeatureViolations(BaseParams baseParams)
        {
            var result = Container.Resolve<IViolationFeatureGjiService>().AddFeatureViolations(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SaveViolationGroups(BaseParams baseParams)
        {
            var result = Container.Resolve<IViolationFeatureGjiService>().SaveViolationGroups(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CreateFeature(BaseParams baseParams)
        {
            var repo = Container.ResolveRepository<FeatureViolGji>();

            var name = baseParams.Params["Name"];
            var code = baseParams.Params["Code"];
            var id = baseParams.Params["ParentId"].ToString();
            var idValue = string.IsNullOrWhiteSpace(id) ? (long?)null : long.Parse(id);

            var parent = idValue.HasValue ? repo.Get(idValue.Value) : null;

            repo.Save(new FeatureViolGji
            {
                Name = name.ToString(),
                Code = code.ToString(),
                Parent = parent
            });

            return JsSuccess();
        }

        public ActionResult ListGroups(BaseParams baseParams)
        {
            var repo = Container.ResolveRepository<FeatureViolGji>();
            var violationFeaturesRepo = Container.ResolveRepository<ViolationFeatureGji>();

            try
            {
                var violationId = baseParams.Params["violationId"].ToLong();
                var loadParam = baseParams.GetLoadParam();

                var violationFeatures = violationFeaturesRepo.GetAll()
                                        .Where(x => x.ViolationGji.Id == violationId)
                                        .Select(x => new
                                        {
                                            x.Id,
                                            FeatureViolGji = x.FeatureViolGji.Id,
                                            ViolationGji = x.ViolationGji.Id,
                                            FullName = x.FeatureViolGji.FullName ?? x.FeatureViolGji.Name,
                                            x.FeatureViolGji.Name,
                                            x.FeatureViolGji.Code
                                        })
                                        .Filter(loadParam, Container);

                int totalCount = violationFeatures.Count();

                return new JsonListResult(violationFeatures.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(repo);
                Container.Release(violationFeaturesRepo);
            }

        }

        public ActionResult ListParent(BaseParams baseParams)
        {
            var parents = Container.ResolveDomain<FeatureViolGji>().GetAll().Where(item => item.Parent == null).ToList();
            var withViolations = Container.ResolveDomain<ViolationFeatureGji>().GetAll().Select(item => item.FeatureViolGji).Where(item => item.Parent == null).Distinct().ToList();
            parents = parents.Where(item => withViolations.All(viol => viol.Id != item.Id)).ToList();
            var data = parents.Select(item =>
            new
            {
                item.Id,
                item.Name
            }).ToList();

            return new JsonNetResult(new
            {
                success = true,
                data = data,
                totalCount = data.Count
            });
        }

        public ActionResult ListViols(BaseParams baseParams)
        {
            var featureId = baseParams.Params.GetAs<long>("featureId");
            var loadParam = baseParams.GetLoadParam();

            var violNormativeDocItemDomain = Container.ResolveDomain<ViolationNormativeDocItemGji>();

            try
            {
                var viols = DomainService.GetAll().Where(x => x.FeatureViolGji.Id == featureId)
                    .Select(x => new
                    {
                        x.Id,
                        x.ViolationGji.Name,
                        NormDocNum = x.ViolationGji.NormativeDocNames
                    })
                    .AsQueryable()
                    .Filter(loadParam, Container);

                var totalCount = viols.Count();

                return new JsonListResult(viols.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally
            {
                Container.Release(violNormativeDocItemDomain);
            }
        }
    }
}