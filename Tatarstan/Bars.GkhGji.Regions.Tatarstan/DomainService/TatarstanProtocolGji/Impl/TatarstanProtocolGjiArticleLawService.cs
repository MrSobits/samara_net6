namespace Bars.GkhGji.Regions.Tatarstan.DomainService.TatarstanProtocolGji.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    using Castle.Windsor;

    public class TatarstanProtocolGjiArticleLawService : ITatarstanProtocolGjiArticleLawService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult SaveArticles(BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAsId("protocolId");
            var articleIds = baseParams.Params.GetAs<long[]>("articleIds");

            if (protocolId == default(long) || articleIds == null || articleIds.Length == 0)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = "Некорректные входные данные"
                };
            }

            var serviceArticles = this.Container.Resolve<IDomainService<TatarstanProtocolGjiArticleLaw>>();
            using (this.Container.Using(serviceArticles))
            {
                var articleLawIdsHash = serviceArticles.GetAll()
                    .Where(x => x.TatarstanProtocolGji.Id == protocolId)
                    .Select(x => x.ArticleLaw.Id)
                    .ToHashSet();

                foreach (var id in articleIds)
                {
                    // Если среди существующих статей уже есть такая статья, то пропускаем
                    if (id == 0 || articleLawIdsHash.Contains(id))
                    {
                        continue;
                    }

                    var newObj = new TatarstanProtocolGjiArticleLaw
                    {
                        TatarstanProtocolGji = new TatarstanProtocolGji { Id = protocolId },
                        ArticleLaw = new ArticleLawGji { Id = id }
                    };

                    serviceArticles.Save(newObj);
                }

                return new BaseDataResult();
            }
        }
    }
}
