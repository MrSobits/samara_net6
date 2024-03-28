namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Castle.Windsor;

    public class RequirementService : IRequirementService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            var reqId = baseParams.Params.GetAs<long>("reqId");
            var requirementDomain = Container.Resolve<IDomainService<Requirement>>();
            var requirementArticleLawDomain = Container.Resolve<IDomainService<RequirementArticleLaw>>();

            using (Container.Using(requirementDomain, requirementArticleLawDomain))
            {
                var requirement = requirementDomain.Load(reqId);
                var typeBase = requirement != null ? requirement.Document.Inspection.TypeBase : 0;

                var articles = requirementArticleLawDomain.GetAll()
                    .Where(x => x.Requirement.Id == reqId)
                    .Select(x => new
                    {
                        x.ArticleLaw.Id,
                        x.ArticleLaw.Name
                    })
                    .ToArray();

                return new BaseDataResult(new
                {
                    artIds = articles.AggregateWithSeparator(x => x.Id, ","),
                    artNames = articles.AggregateWithSeparator(x => x.Name, ","),
                    typeBase
                });
            }
        }
    }
}