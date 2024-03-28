using Newtonsoft.Json.Linq;

namespace Bars.GkhGji.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using Entities;
    using DomainService;
    using Bars.B4.Utils;

    /// <summary>
    /// Контроллер тематик обращения
    /// </summary>
    public class StatSubjectGjiController : B4.Alt.DataController<StatSubjectGji>
    {
        private readonly IStatSubjectGjiService statSubjectService;

        public StatSubjectGjiController(IStatSubjectGjiService _statSubjectService)
        {
            this.statSubjectService = _statSubjectService;
        }
        
        /// <summary>
        /// Добавление подтематик
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult AddSubsubject(BaseParams baseParams)
        {
            var result = statSubjectService.AddSubsubject(baseParams);
            return result.Success ? new JsonNetResult(new {success = true}) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListTree(BaseParams baseParams)
        {
            var filter = baseParams.Params.GetAs<string>("filter").ToStr().ToLowerInvariant();
            var serviceSubject = Container.Resolve<IDomainService<StatSubjectGji>>();
            var serviceSubsubject = Container.Resolve<IDomainService<StatSubjectSubsubjectGji>>();
            var serviceFeature = Container.Resolve<IDomainService<StatSubsubjectFeatureGji>>();

            var root = new JObject();

            var subjs = new JArray();

            var sibjIds = serviceSubsubject.GetAll().WhereIf(!string.IsNullOrEmpty(filter), x => x.Subsubject.Name.ToLowerInvariant().Contains(filter) || x.Subject.Name.ToLowerInvariant().Contains(filter)).Select(x => x.Subject.Id).ToList();


            foreach (var rec in serviceSubject.GetAll().WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.ToLowerInvariant().Contains(filter) || sibjIds.Contains(x.Id)).OrderBy(x=> x.Code).ToList())
            {
                var subj = new JObject();

                subj["id"] = rec.Id.ToString();
                subj["text"] = rec.Name;

                var subsubjs = new JArray();

                foreach (var rec1 in serviceSubsubject.GetAll().Where(x => x.Subject.Id == rec.Id).WhereIf(!string.IsNullOrEmpty(filter), x => x.Subsubject.Name.ToLowerInvariant().Contains(filter)).Select(x => x.Subsubject).ToList())
                {
                    var subsubj = new JObject();

                    subsubj["id"] = rec.Id.ToString() + "/"+ rec1.Id.ToString();
                    subsubj["text"] = rec1.Name;

                    var feats = new JArray();

                    foreach (var rec2 in serviceFeature.GetAll().Where(x => x.Subsubject.Id == rec1.Id).Select(x => x.FeatureViol).ToList())
                    {
                        var feat = new JObject();

                        feat["id"] = rec.Id.ToString() + "/" + rec1.Id.ToString() + "/" + rec2.Id.ToString();
                        feat["text"] = rec2.Name;
                        feat["leaf"] = true;
                        feat["checked"] = false;

                        feats.Add(feat);
                    }

                    if (feats.Count == 0)
                    {
                        subsubj["leaf"] = true;
                        subsubj["checked"] = false;
                    }
                    else
                        subsubj["children"] = feats;

                    subsubjs.Add(subsubj);
                }

                if (subsubjs.Count == 0)
                {
                    subj["leaf"] = true;
                    subj["checked"] = false;
                }
                else
                    subj["children"] = subsubjs;

                subjs.Add(subj);
            }

            root["children"] = subjs;

            return new JsonNetResult(root);
        }
        
        /// <summary>
        /// Список тематик для СОПР
        /// </summary>
        public ActionResult ListForSopr()
        {
            return new JsonListResult(statSubjectService.ListForSopr());
        }
    }
}