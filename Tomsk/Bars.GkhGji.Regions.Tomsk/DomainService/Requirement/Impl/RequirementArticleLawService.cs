namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Castle.Windsor;

    public class RequirementArticleLawService : IRequirementArticleLawService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddArticles(BaseParams baseParams)
        {
            var reqId = baseParams.Params.GetAs<long>("reqId", 0);

            if (reqId == 0)
            {
                return new BaseDataResult(false, "Не удалось получить требование");
            }

            var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

            var service = Container.Resolve<IDomainService<RequirementArticleLaw>>();
            var serviceArticle = Container.Resolve<IDomainService<ArticleLawGji>>();
            var serviceReq = Container.Resolve<IDomainService<Requirement>>();

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var existRecs = service.GetAll()
                        .Where(x => x.Requirement.Id == reqId)
                        .Select(x => new {ArtId = x.ArticleLaw.Id, x.Id})
                        .AsEnumerable()
                        .ToDictionary(x => x.ArtId, y => y.Id);

                    var req = serviceReq.Load(reqId);

                    foreach (var id in objectIds)
                    {
                        if (existRecs.ContainsKey(id))
                        {
                            existRecs.Remove(id);
                            continue;
                        }

                        var newRec = new RequirementArticleLaw
                        {
                            Requirement = req,
                            ArticleLaw = serviceArticle.Load(id)
                        };

                        service.Save(newRec);
                    }

                    foreach (var existRec in existRecs)
                    {
                        service.Delete(existRec.Value);
                    }

                    tr.Commit();
                }
                catch (ValidationException e)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(service);
                    Container.Release(serviceArticle);
                    Container.Release(serviceReq);
                }
            }

            return new BaseDataResult();
        }
    }
}
