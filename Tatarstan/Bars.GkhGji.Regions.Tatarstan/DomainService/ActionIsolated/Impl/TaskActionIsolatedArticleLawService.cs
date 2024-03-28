namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    using Castle.Windsor;

    public class TaskActionIsolatedArticleLawService : ITaskActionIsolatedArticleLawService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult AddArticles(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            var articleLawIds = baseParams.Params.GetAs<long[]>("articleLawIds");

            var articleLawsToSave = articleLawIds.Select(articleLawId => new TaskActionIsolatedArticleLaw
            {
                Task = new TaskActionIsolated { Id = documentId },
                ArticleLaw = new ArticleLawGji { Id = articleLawId }
            });

            TransactionHelper.InsertInManyTransactions(this.Container, articleLawsToSave);

            return new BaseDataResult();
        }
    }
}
